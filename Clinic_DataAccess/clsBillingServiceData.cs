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

        public static bool UpdatePaymentStatus(int BillID, byte PaymentStatus)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

                {
                    string queryUpdateBillNumber = @"UPDATE Bills SET PaymentStatus=@PaymentStatus 
                                                 WHERE BillID = @BillID";
                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, connection ))
                    {
                        cmdUpdate.Parameters.AddWithValue("@BillID", BillID);
                        cmdUpdate.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);
                        connection.Open();
                        return cmdUpdate.ExecuteNonQuery() > 0;
                    }

                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }

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

       


        #region EDitPending Prescription ITems
        //public static bool GetPrescriptionsDetails

        #endregion


    }
}
