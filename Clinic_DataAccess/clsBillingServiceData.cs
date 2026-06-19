using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsBillingServiceData
    {
        public static bool CheckInPatient(int AppointmentID, decimal Fees, decimal Discount, int UserID, string PaymentMethod)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. حساب الضريبة بعد الخصم
                        float taxRate = clsSystemSettingsData.GetTaxRateFromSettings();
                        decimal amountAfterDiscount = Fees - Discount;

                        // حساب الضريبة: (المبلغ بعد الخصم) - (المبلغ بعد الخصم / 1 + النسبة)
                        decimal taxAmount = amountAfterDiscount - (amountAfterDiscount / (1 + (decimal)taxRate));

                        decimal TotalCost = amountAfterDiscount + taxAmount;
                        // 2. إنشاء الفاتورة (مع إدراج الخصم والضريبة)
                        string queryBill = @"INSERT INTO Bills (AppointmentID, TotalCost, DiscountAmount, TaxAmount, PaymentStatus, CreatedByUserID, BillDate,IsVoid) 
                                     VALUES (@AppointmentID, @TotalCost, @DiscountAmount, @TaxAmount, 2, @UserID, GETDATE(),1); 
                                     SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdBill = new SqlCommand(queryBill, connection, transaction);
                        cmdBill.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        cmdBill.Parameters.AddWithValue("@TotalCost", TotalCost); 
                        cmdBill.Parameters.AddWithValue("@DiscountAmount", Discount);
                        cmdBill.Parameters.AddWithValue("@TaxAmount", taxAmount);
                        cmdBill.Parameters.AddWithValue("@UserID", UserID);

                        int BillID = Convert.ToInt32(cmdBill.ExecuteScalar());

                        // 3. توليد رقم الفاتورة

                        UpdateBillNumber(BillID, transaction);

                        // 4. تسجيل الدفعة (المبلغ الفعلي المدفوع = الإجمالي - الخصم)
                        decimal finalPaymentAmount = amountAfterDiscount;

                        string queryPay = @"INSERT INTO Payments (BillID, PaymentAmount, PaymentDate, PaymentMethod, CreatedByUserID) 
                                    VALUES (@BillID, @Amount, GETDATE(), @Method, @UserID)";

                        SqlCommand cmdPay = new SqlCommand(queryPay, connection, transaction);
                        cmdPay.Parameters.AddWithValue("@BillID", BillID);
                        cmdPay.Parameters.AddWithValue("@Amount", finalPaymentAmount);
                        cmdPay.Parameters.AddWithValue("@Method", PaymentMethod);
                        cmdPay.Parameters.AddWithValue("@UserID", UserID);
                        cmdPay.ExecuteNonQuery();

                        // 5. تحديث حالة الموعد
                        string queryUpdateApp = @"UPDATE Appointments SET AppointmentStatus = 2, CheckInTime = GETDATE() WHERE AppointmentID = @AppID";
                        SqlCommand cmdApp = new SqlCommand(queryUpdateApp, connection, transaction);
                        cmdApp.Parameters.AddWithValue("@AppID", AppointmentID);
                        cmdApp.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // يفضل تسجيل الخطأ في ملف Log
                        return false;
                    }
                }
            }
        }

       public static bool UpdateBillNumber(int  BillID,SqlTransaction transaction)
        {
            string queryUpdateBillNumber = @"UPDATE Bills SET BillNumber = 'INV-' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-' + RIGHT('0000' + CAST(@BillID AS VARCHAR), 4) 
                                                 WHERE BillID = @BillID";
            SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, transaction.Connection,transaction);
            cmdUpdate.Parameters.AddWithValue("@BillID", BillID);
          return  cmdUpdate.ExecuteNonQuery()>0;
        }
    }
}
