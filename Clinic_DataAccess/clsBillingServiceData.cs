using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsBillingServiceData
    {
        public static bool CheckInPatient(int AppointmentID, int DoctorID, int PatientID, decimal Fees, int UserID, string PaymentMethod)
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

                        string queryBill = @"INSERT INTO Bills (VisitID, PaymentStatus, CreatedByUserID, BillDate) 
                             VALUES (@VisitID, 2, @UserID, GETDATE()); 
                             SELECT SCOPE_IDENTITY();";
                        int BillID = -1;
                        using (SqlCommand cmdBill = new SqlCommand(queryBill, connection, transaction))
                        {
                            cmdBill.Parameters.AddWithValue("@VisitID", VisitID);
                            cmdBill.Parameters.AddWithValue("@UserID", UserID);
                            BillID = Convert.ToInt32(cmdBill.ExecuteScalar());

                            UpdateBillNumber(BillID, transaction);
                        }

                        // 4. تسجيل الدفعة (هنا يتم استخدام PaymentMethod)
                        string queryPay = @"INSERT INTO Payments (BillID, PaymentAmount, PaymentDate, PaymentMethod, CreatedByUserID) 
                            VALUES (@BillID, @Amount, GETDATE(), @Method, @UserID)";

                        using (SqlCommand cmdPay = new SqlCommand(queryPay, connection, transaction))
                        {
                            cmdPay.Parameters.AddWithValue("@BillID", BillID);
                            cmdPay.Parameters.AddWithValue("@Amount", Fees);
                            cmdPay.Parameters.AddWithValue("@Method", PaymentMethod);
                            cmdPay.Parameters.AddWithValue("@UserID", UserID);
                            cmdPay.ExecuteNonQuery();
                        }

                        // 5. تحديث حالة الموعد
                        clsAppointmentData.UpdateAppointmentStatus(AppointmentID, 2, UserID, transaction);

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

        public static bool UpdateBillNumber(int BillID, SqlTransaction transaction)
        {
            string queryUpdateBillNumber = @"UPDATE Bills SET BillNumber = 'INV-' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-' + RIGHT('0000' + CAST(@BillID AS VARCHAR), 4) 
                                                 WHERE BillID = @BillID";
            SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, transaction.Connection, transaction);
            cmdUpdate.Parameters.AddWithValue("@BillID", BillID);
            return cmdUpdate.ExecuteNonQuery() > 0;
        }

        public static bool UpdatePaymentStatus(int VisitID, byte PaymentStatus, SqlTransaction transaction)
        {
            string queryUpdateBillNumber = @"UPDATE Bills SET PaymentStatus=@PaymentStatus 
                                                 WHERE VisitID = @VisitID";
            SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, transaction.Connection, transaction);
            cmdUpdate.Parameters.AddWithValue("@VisitID", VisitID);
            cmdUpdate.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);

            return cmdUpdate.ExecuteNonQuery() > 0;
        }

        public static DataTable GetAllBills()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SElECT * FROM View_Bills
                                     ORDER BY BillNumber DESC;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }

            return dt;
        }

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
                    string updateBill = "UPDATE Bills SET Status = @Status WHERE BillID = @BillID";
                    SqlCommand cmdUpdate = new SqlCommand(updateBill, connection, transaction);
                    cmdUpdate.Parameters.AddWithValue("@Status", newStatus);
                    cmdUpdate.Parameters.AddWithValue("@BillID", billID);
                    cmdUpdate.ExecuteNonQuery();

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
        public static bool GetBillSummariesToProcessPayment(int BillID, DataTable dt,
                  ref decimal appointmentFees, ref decimal totalMedicines,
                  ref decimal totalServices, ref decimal totalDiscount,
                             ref decimal totalTax,
                  ref decimal paymentAmount, ref decimal balanceDue)
        {

            dt.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // استدعاء الـ View بالكامل
                    string query = @"SELECT * FROM View_BillSummaries WHERE BillID = @BillID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 1. تعبئة المتغيرات المالية (للعمليات الحسابية في الـ C#)
                                appointmentFees = Convert.ToDecimal(reader["AppointmentFees"]);
                                totalMedicines = Convert.ToDecimal(reader["MedicinesTotal"]);
                                totalServices = Convert.ToDecimal(reader["ServicesTotal"]);
                                totalDiscount = Convert.ToDecimal(reader["TotalDiscount"]);
                                totalTax = Convert.ToDecimal(reader["TotalTax"]);
                                paymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                balanceDue = Convert.ToDecimal(reader["BalanceDue"]);

                                // 2. تعبئة الـ DataTable (للعرض في GridView أو التقرير)
                                DataRow row = dt.NewRow();
                                row["BillNumber"] = reader["BillNumber"];
                                row["PatientName"] = reader["PatientName"];
                                row["ItemsTotal"] = reader["ItemsTotal"];
                                row["Subtotal"] = reader["Subtotal"];

                                dt.Rows.Add(row);

                                return true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return false;
        }

        public static bool GetBillSummariesToIssueInvoice(int BillID, ref decimal FinalTotal, ref decimal Subtotal,
                                                                        ref decimal PaymentAmount, ref decimal TotalDiscount)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // استدعاء الـ View بالكامل
                    string query = @"  SELECT (Subtotal+TotalDiscount) AS Subtotal,PaymentAmount ,TotalDiscount,Subtotal
                                                FROM ClinicDB.dbo.View_BillSummaries
                                                 Where BillID=@BillID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 1. تعبئة المتغيرات المالية (للعمليات الحسابية في الـ C#)
                                TotalDiscount = Convert.ToDecimal(reader["TotalDiscount"]);
                                FinalTotal = Convert.ToDecimal(reader["FinalTotal"]);
                                Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                                PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);


                            }
                            return true;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return false;
        }
        public static DataTable GetBillItems(int BillID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_BillItems
                                         WHERE BillID=@BillID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return dt;
        }
        public static DataRow GetBillByID(int BillID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_Bills WHERE BillID = @BillID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }

            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        public static bool ProcessPartialPayment(DataTable dtDispensedItems, int billID, decimal paymentAmount, string paymentMethod, int userID, string status)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // --- الجزء الأول: تحديث صرف الأدوية (كودك) ---
                    using (SqlCommand cmdCreate = new SqlCommand("CREATE TABLE #TempDispensed (DetailsID INT, DispensedQty INT, IsDispensed BIT)", connection, transaction))
                    {
                        cmdCreate.ExecuteNonQuery();
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "#TempDispensed";
                        bulkCopy.ColumnMappings.Add("PrescriptionDetailsID", "DetailsID");
                        bulkCopy.ColumnMappings.Add("DispensedQuantity", "DispensedQty");
                        bulkCopy.ColumnMappings.Add("IsDispensed", "IsDispensed");
                        bulkCopy.WriteToServer(dtDispensedItems);
                    }

                    string updateQuery = @"UPDATE PD 
                                   SET PD.DispensedQuantity = T.DispensedQty, 
                                       PD.IsDispensed = T.IsDispensed 
                                   FROM PrescriptionDetails PD 
                                   INNER JOIN #TempDispensed T ON PD.PrescriptionDetailsID = T.DetailsID";

                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, connection, transaction))
                    {
                        cmdUpdate.ExecuteNonQuery();

                    }

                    transaction.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                    // التراجع في حال حدوث خطأ
                    transaction.Rollback();
                    clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                    return false;
                }
            }
        }




    }
}
