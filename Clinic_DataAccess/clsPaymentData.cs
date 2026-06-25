using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsPaymentData
    {
        public static bool AddPaymentAndSetStatus(int billID, decimal paymentAmount, string paymentMethod, int userID, byte newStatus)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // 1. إضافة الدفعة
                    string insertPayment = @"
                                        INSERT INTO Payments (BillID, PaymentAmount, PaymentDate, PaymentMethod, CreatedByUserID)
                                        VALUES (@BillID, @PaymentAmount, GETDATE(), @PaymentMethod, @UserID)";

                    SqlCommand cmdInsert = new SqlCommand(insertPayment, connection, transaction);
                    cmdInsert.Parameters.AddWithValue("@BillID", billID);
                    cmdInsert.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                    cmdInsert.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmdInsert.Parameters.AddWithValue("@UserID", userID);
                    cmdInsert.ExecuteNonQuery();

                    // 2. تحديث الحالة التي أرسلتها أنت من الشاشة
                    string updateBill = "UPDATE Bills SET PaymentStatus = @Status WHERE BillID = @BillID";
                    SqlCommand cmdUpdate = new SqlCommand(updateBill, connection, transaction);
                    cmdUpdate.Parameters.AddWithValue("@Status", newStatus);
                    cmdUpdate.Parameters.AddWithValue("@BillID", billID);
                    cmdUpdate.ExecuteNonQuery();

                    if (!clsPrescriptionData.UpdatePrescriptionStatusByBill(billID, 3, transaction))
                    {
                        throw new Exception("Failed to update prescription status.");
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // سجل الخطأ
                    return false;
                }
            }
        }

    }
}
