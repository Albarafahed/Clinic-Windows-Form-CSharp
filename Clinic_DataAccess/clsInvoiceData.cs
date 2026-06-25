using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsInvoiceData
    {
        public static DataSet GetInvoiceFullData(int billID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // استعلام واحد يجمع كل الجداول المطلوبة للفاتورة
                string query = @"
                                SELECT (Subtotal+TotalDiscount) AS Subtotal,PaymentAmount ,TotalDiscount,Subtotal AS FinalTotal FROM View_BillSummaries WHERE BillID = @BillID;
                              Select  B.BillNumber As InvoiceNumber,
                                B.BillDate As InvoiceDate,
                                ISNULL(Per.FullName,'Pharmysy Sales')AS PatienName,
                                ISNULL(Pat.PatientID,-1)AS PatientID
                                From Bills B
                                 Left JOIN Visits V ON B.VisitID=V.VisitID
                                 left Join Patients Pat ON V.PatientID=Pat.PatientID
                                 left join Persons Per ON Pat.PersonID=Per.PersonID
                                  Where B.BillID=@BillID;
                                SELECT * FROM View_BillItems WHERE BillID = @BillID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BillID", billID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        #region IssueInvoice
        public static bool GetBillSummariesToIssueInvoice(int BillID, ref decimal FinalTotal, ref decimal Subtotal,
                                                                        ref decimal PaymentAmount, ref decimal TotalDiscount)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // استدعاء الـ View بالكامل
                    string query = @"  SELECT (Subtotal+TotalDiscount) AS Subtotal,PaymentAmount ,TotalDiscount,Subtotal AS FinalTotal
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
        public static DataTable GetBillItemsToIssueInvoice(int BillID)
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

        public static bool GetINVoiceAndPatientDetalis(int BillID, ref int InVoiceNumber, ref DateTime InVoiceDate, ref int PateintID, ref string PatientName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // استدعاء الـ View بالكامل
                    string query = @"Select  Pay.PaymentID As InvoiceNumber,
                                        FORMAT(Pay.PaymentDate,'dd/mm/yyyy')As InvoiceDate,
                                         ISNULL(Per.FullName,'Pharmysy Sales')AS PatienName,
                                         ISNULL(Pat.PatientID,-1)AS PatientID
                                         From Payments Pay JOIN Bills B ON Pay.BillID=B.BillID
                                          Left JOIN Visits V ON B.VisitID=V.VisitID
                                          left Join Patients Pat ON V.PatientID=Pat.PatientID
                                          left join Persons Per ON Pat.PersonID=Per.PersonID
                                                  Where Pay.BillID=@BillID;
";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                InVoiceNumber = (int)reader["InvoiceNumber"];
                                InVoiceDate = (DateTime)reader["InvoiceDate"];
                                PateintID = (int)reader["PatientID"];
                                PatientName = reader["PatienName"].ToString();

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
        #endregion
    }
}
