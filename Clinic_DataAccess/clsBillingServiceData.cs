using Clinic_DataAccess.SaveSqlException;
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
        public static bool CheckInPatient(int AppointmentID,int DoctorID, int PatientID, decimal Fees, decimal Discount, int UserID, string PaymentMethod)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. إنشاء سجل الزيارة (Visit)
                        string queryVisit = @"INSERT INTO Visits (PatientID, AppointmentID, DoctorID, VisitDate, VisitStatus, CreatedByUserID)
                                         VALUES (@PatientID, @AppointmentID, @DoctorID, GETDATE(), 1, @UserID);
                                         SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdVisit = new SqlCommand(queryVisit, connection, transaction);
                        cmdVisit.Parameters.AddWithValue("@PatientID", PatientID);
                        cmdVisit.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        cmdVisit.Parameters.AddWithValue("@UserID", UserID);
                        cmdVisit.Parameters.AddWithValue("@DoctorID", DoctorID);

                        int VisitID = Convert.ToInt32(cmdVisit.ExecuteScalar());

                        // 2. حساب الضريبة (بافتراض أن Fees هي المبلغ قبل الضريبة)
                        decimal taxRate = clsSystemSettingsData.GetTaxRateFromSettings(); // تأكد أن هذه الدالة تعيد decimal
                        decimal amountAfterDiscount = Fees - Discount;
                        decimal taxAmount = amountAfterDiscount * taxRate; // أو المعادلة الضريبية المعتمدة في بلدك
                        decimal totalCost = amountAfterDiscount + taxAmount;

                        string queryBill = @"INSERT INTO Bills (VisitID, TotalCost, DiscountAmount, TaxAmount, PaymentStatus, CreatedByUserID, BillDate, IsPendingPrint)
                                     VALUES (@VisitID, @TotalCost, @DiscountAmount, @TaxAmount, 2, @UserID, GETDATE(), 0);
                                     SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdBill = new SqlCommand(queryBill, connection, transaction);
                        cmdBill.Parameters.AddWithValue("@VisitID", VisitID);
                        cmdBill.Parameters.AddWithValue("@TotalCost", totalCost);
                        cmdBill.Parameters.AddWithValue("@DiscountAmount", Discount);
                        cmdBill.Parameters.AddWithValue("@TaxAmount", taxAmount);
                        cmdBill.Parameters.AddWithValue("@UserID", UserID);

                        int BillID = Convert.ToInt32(cmdBill.ExecuteScalar());
                        UpdateBillNumber(BillID, transaction); // توليد الرقم

                        // 4. تسجيل الدفعة
                        string queryPay = @"INSERT INTO Payments (BillID, PaymentAmount, PaymentDate, PaymentMethod, CreatedByUserID)
                                    VALUES (@BillID, @Amount, GETDATE(), @Method, @UserID)";

                        SqlCommand cmdPay = new SqlCommand(queryPay, connection, transaction);
                        cmdPay.Parameters.AddWithValue("@BillID", BillID);
                        cmdPay.Parameters.AddWithValue("@Amount", totalCost); // المبلغ المدفوع بالكامل
                        cmdPay.Parameters.AddWithValue("@Method", PaymentMethod);
                        cmdPay.Parameters.AddWithValue("@UserID", UserID);
                        cmdPay.ExecuteNonQuery();

                        // 5. تحديث حالة الموعد
                        clsAppointmentData.UpdateAppointmentStatus(AppointmentID, 2,UserID, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
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
