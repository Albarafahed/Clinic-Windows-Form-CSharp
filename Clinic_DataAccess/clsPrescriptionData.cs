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
    public class clsPrescriptionData
    {
        public static int SavePrescription(int VisitID, string Notes, DateTime Date, DataTable dtMedicines)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. حفظ رأس الوصفة واسترجاع الـ ID
                        string queryMaster = @"INSERT INTO Prescriptions (VisitID, PrescriptionDate, PrescriptionNotes) 
                                      VALUES (@VisitID, @Date, @Notes);
                                      SELECT SCOPE_IDENTITY();";

                        int prescriptionID = -1;
                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, connection, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@VisitID", VisitID);
                            cmdMaster.Parameters.AddWithValue("@Date", Date);
                            cmdMaster.Parameters.AddWithValue("@Notes", Notes.ToDBValue());
                            prescriptionID = Convert.ToInt32(cmdMaster.ExecuteScalar());
                        }

                        // 2. إضافة الأعمدة المطلوبة كـ Expression (سريع جداً وبدون foreach)
                        // الـ Expression يقوم بتوزيع القيمة على كل الصفوف داخلياً
                        dtMedicines.Columns.Add("PrescriptionID", typeof(int), prescriptionID.ToString());
                        dtMedicines.Columns.Add("VisitID", typeof(int), VisitID.ToString());

                        // 3. الحفظ السريع للأدوية
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "PrescriptionDetails";

                            // ربط الأعمدة (يجب أن تتطابق مع أسماء الأعمدة في DataTable و SQL)
                            bulkCopy.ColumnMappings.Add("PrescriptionID", "PrescriptionID");
                            bulkCopy.ColumnMappings.Add("VisitID", "VisitID");
                            bulkCopy.ColumnMappings.Add("MedicineID", "MedicineID");
                            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                            bulkCopy.ColumnMappings.Add("Dosage", "Dosage");
                            bulkCopy.ColumnMappings.Add("Instructions", "Instructions");
                            bulkCopy.ColumnMappings.Add("SavedMedicineName", "SavedMedicineName");
                            bulkCopy.ColumnMappings.Add("SavedMedicinePrice", "SavedMedicinePrice");

                            bulkCopy.WriteToServer(dtMedicines);
                        }

                        transaction.Commit();
                        return prescriptionID;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // يُفضل تسجيل الخطأ هنا باستخدام Logger الخاص بك
                        return -1;
                    }
                }
            }
        }

        public static bool UpdatePrescription(int PrescriptionID, int VisitID, string Notes, DateTime Date, DataTable dtMedicines)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. تحديث بيانات رأس الوصفة (Master)
                        string queryMaster = @"UPDATE Prescriptions 
                                       SET PrescriptionDate = @Date, 
                                           PrescriptionNotes = @Notes 
                                       WHERE PrescriptionID = @PrescriptionID";

                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, connection, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
                            cmdMaster.Parameters.AddWithValue("@Date", Date);
                            cmdMaster.Parameters.AddWithValue("@Notes", Notes.ToDBValue());
                            cmdMaster.ExecuteNonQuery();
                        }

                        // 2. حذف التفاصيل القديمة (عن طريق الـ PrescriptionID)
                        string queryDeleteDetails = "DELETE FROM PrescriptionDetails WHERE PrescriptionID = @PrescriptionID";
                        using (SqlCommand cmdDelete = new SqlCommand(queryDeleteDetails, connection, transaction))
                        {
                            cmdDelete.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
                            cmdDelete.ExecuteNonQuery();
                        }

                        // 3. إعادة إدخال التفاصيل الجديدة (BulkCopy)
                        // تأكد من إضافة الأعمدة كـ Expression لتناسب الحفظ الجماعي
                        if (!dtMedicines.Columns.Contains("PrescriptionID"))
                            dtMedicines.Columns.Add("PrescriptionID", typeof(int), PrescriptionID.ToString());

                        if (!dtMedicines.Columns.Contains("VisitID"))
                            dtMedicines.Columns.Add("VisitID", typeof(int), VisitID.ToString());

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "PrescriptionDetails";

                            bulkCopy.ColumnMappings.Add("PrescriptionID", "PrescriptionID");
                            bulkCopy.ColumnMappings.Add("VisitID", "VisitID");
                            bulkCopy.ColumnMappings.Add("MedicineID", "MedicineID");
                            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                            bulkCopy.ColumnMappings.Add("Dosage", "Dosage");
                            bulkCopy.ColumnMappings.Add("Instructions", "Instructions");
                            bulkCopy.ColumnMappings.Add("SavedMedicineName", "SavedMedicineName");
                            bulkCopy.ColumnMappings.Add("SavedMedicinePrice", "SavedMedicinePrice");

                            bulkCopy.WriteToServer(dtMedicines);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public static DataTable GetVisitMedicines(int VisitID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"
                                      SELECT
                                    PPD.MedicineID, 
                                    PP.VisitID, 
                                    PPD.SavedMedicineName 
                                    PPD.Dosage, 
                                    PPD.Instructions,
                                    PPD.SavedMedicinePrice, 
                                    PPD.Quantity
                                FROM PrescriptionS PP
                                INNER JOIN PrescriptionDetails PPD ON 
                                    PP.PrescriptionID = PPD.PrescriptionID
                                WHERE PP.VisitID = @VisitID;";
                   

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
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

        public static bool Find(int VisitID, ref int PrescriptionID,ref string PrescriptionNotes, ref DateTime PrescriptionDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT PrescriptionID,
                                            PrescriptionDate,
                                          PrescriptionNotes
                                      FROM  Prescriptions
                                      WHERE VisitID=@VisitID;";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PrescriptionID = (int)reader["PrescriptionID"];
                                PrescriptionDate = (DateTime)reader["PrescriptionDate"];
                                PrescriptionNotes = reader["PrescriptionNotes"].ToStringOrEmpty();
                                return true;

                            }
                            else
                                return false;
                        }
                    }
                                    
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
        }
    }
}
