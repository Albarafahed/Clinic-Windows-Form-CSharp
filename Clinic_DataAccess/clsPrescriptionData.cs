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
        public static int SavePrescription(int ?VisitID, string Notes, DateTime Date, DataTable dtMedicines, byte PrescriptionStatus, byte Prescriptiontype)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. حفظ رأس الوصفة واسترجاع الـ ID
                        string queryMaster = @"INSERT INTO Prescriptions (VisitID, PrescriptionDate, PrescriptionNotes,PrescriptionStatus,Prescriptiontype) 
                                      VALUES (@VisitID, @Date, @Notes,@PrescriptionStatus,@Prescriptiontype);
                                      SELECT SCOPE_IDENTITY();";

                        int prescriptionID = -1;
                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, connection, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@VisitID",(object)VisitID??DBNull.Value);
                            cmdMaster.Parameters.AddWithValue("@Date", Date);
                            cmdMaster.Parameters.AddWithValue("@Notes", Notes.ToDBValue());
                            cmdMaster.Parameters.AddWithValue("@PrescriptionStatus", PrescriptionStatus);
                            cmdMaster.Parameters.AddWithValue("@Prescriptiontype", Prescriptiontype);

                            prescriptionID = Convert.ToInt32(cmdMaster.ExecuteScalar());
                        }

                        dtMedicines.Columns.Add("PrescriptionID", typeof(int), prescriptionID.ToString());

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "PrescriptionDetails";

                            // ربط الأعمدة (يجب أن تتطابق مع أسماء الأعمدة في DataTable و SQL)
                            bulkCopy.ColumnMappings.Add("PrescriptionID", "PrescriptionID");
                            bulkCopy.ColumnMappings.Add("MedicineID", "MedicineID");
                            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                            bulkCopy.ColumnMappings.Add("Dosage", "Dosage");
                            bulkCopy.ColumnMappings.Add("Instructions", "Instructions");
                            bulkCopy.ColumnMappings.Add("SavedMedicineName", "SavedMedicineName");
                            bulkCopy.ColumnMappings.Add("SavedMedicinePrice", "SavedMedicinePrice");
                            bulkCopy.ColumnMappings.Add("Frequency", "Frequency");
                            bulkCopy.ColumnMappings.Add("DiscountAmount", "DiscountAmount");
                            bulkCopy.WriteToServer(dtMedicines);
                        }
                        if(VisitID.HasValue)
                        clsVisitData.UpdateVisitTotalAmount(VisitID.Value, transaction);
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

        public static bool UpdatePrescription(int PrescriptionID, int VisitID, string Notes, DateTime Date, byte PrescriptionStatus, DataTable dtMedicines)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryMaster = @"UPDATE Prescriptions 
                                       SET PrescriptionDate = @Date, 
                                           PrescriptionNotes = @Notes ,
                                            PrescriptionStatus=@PrescriptionStatus
                                       WHERE PrescriptionID = @PrescriptionID";

                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, connection, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
                            cmdMaster.Parameters.AddWithValue("@Date", Date);
                            cmdMaster.Parameters.AddWithValue("@Notes", Notes.ToDBValue());
                            cmdMaster.Parameters.AddWithValue("@PrescriptionStatus", PrescriptionStatus);

                            cmdMaster.ExecuteNonQuery();
                        }

                        // 2. حذف التفاصيل القديمة (عن طريق الـ PrescriptionID)
                        string queryDeleteDetails = "DELETE FROM PrescriptionDetails WHERE PrescriptionID = @PrescriptionID";
                        using (SqlCommand cmdDelete = new SqlCommand(queryDeleteDetails, connection, transaction))
                        {
                            cmdDelete.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
                            cmdDelete.ExecuteNonQuery();
                        }


                        if (!dtMedicines.Columns.Contains("PrescriptionID"))
                            dtMedicines.Columns.Add("PrescriptionID", typeof(int), PrescriptionID.ToString());

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "PrescriptionDetails";

                            bulkCopy.ColumnMappings.Add("PrescriptionID", "PrescriptionID");
                            bulkCopy.ColumnMappings.Add("MedicineID", "MedicineID");
                            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                            bulkCopy.ColumnMappings.Add("Dosage", "Dosage");
                            bulkCopy.ColumnMappings.Add("Instructions", "Instructions");
                            bulkCopy.ColumnMappings.Add("SavedMedicineName", "SavedMedicineName");
                            bulkCopy.ColumnMappings.Add("SavedMedicinePrice", "SavedMedicinePrice");
                            bulkCopy.ColumnMappings.Add("Frequency", "Frequency");
                            bulkCopy.ColumnMappings.Add("DiscountAmount", "DiscountAmount");
                            bulkCopy.WriteToServer(dtMedicines);
                        }
                        clsVisitData.UpdateVisitTotalAmount(VisitID, transaction);

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
                                    PPD.SavedMedicineName, 
                                    PPD.Dosage, 
                                    PPD.Instructions,
                                    PPD.SavedMedicinePrice, 
                                    PPD.Quantity,
                                    PPD.Frequency,
                                    PPD.DiscountAmount
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

        public static DataTable GetAllActivePrescriptions()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @" SELECT 
                                        P.PrescriptionID,
                                        PP.FullName AS PatientName,
                                        PD.FullName AS DoctorName,
                                        P.PrescriptionDate,
                                        CASE P.PrescriptionStatus
                                        WHEN 1 THEN 'Pending'
                                        WHEN 2 THEN 'Waiting For Payment'
                                        WHEN 3 THEN 'Ready For Dispensing'
                                        WHEN 4 THEN 'Dispensed'
                                        WHEN 5 THEN 'Partially Dispensed'
                                        WHEN 6 THEN 'Cancelled'
                                        ELSE 'Unknown'
                                    END AS Status,
                                        P.VisitID,
                                         V.AppointmentID
                                    FROM Prescriptions AS P
                                    INNER JOIN Visits AS V ON P.VisitID = V.VisitID
                                    INNER JOIN Patients AS Pat ON V.PatientID = Pat.PatientID
                                    INNER JOIN Persons AS PP ON Pat.PersonID = PP.PersonID
                                    INNER JOIN Doctors AS D ON V.DoctorID = D.DoctorID
                                    INNER JOIN Persons AS PD ON D.PersonID = PD.PersonID
                                    WHERE P.PrescriptionStatus NOT IN (4, 6) 
                                      AND P.Prescriptiontype = 1;";


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

        public static DataTable GetAllPrescriptionDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 
                                        PD.PrescriptionID,
                                        PD.SavedMedicineName AS MedicineName,
                                        PD.Dosage,
                                        PD.Frequency,
                                        PD.Quantity AS RequiredQuantity, -- الكمية المطلوبة في الوصفة
                                        PD.Instructions,
                                        PD.DiscountAmount,
                                        CASE 
                                            WHEN M.StockQuantity IS NULL OR M.StockQuantity <= 0 THEN 0           -- غير متوفر
                                            WHEN M.StockQuantity >= PD.Quantity THEN 1                            -- متوفر بالكامل
                                            ELSE M.StockQuantity                                                 -- متوفر لكن الكمية أقل من المطلوب (يرد الكمية المتاحة)
                                        END AS AvailableStatus
                                    FROM PrescriptionDetails PD
                                    LEFT JOIN Medicines M ON PD.SavedMedicineName = M.MedicineName; -- الربط هنا يتم باسم الدواء";


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
        public static DataTable GetAllPrescriptionRecords()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_PrescriptionDetails;";


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

        public static DataTable GetPrescriptionItemsRaw()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 
                                        PrescriptionID,
                                        SavedMedicineName AS MedicineName,
                                        Dosage,
                                        Frequency,
                                        Quantity,
                                        Instructions,
                                        DiscountAmount
                                    FROM PrescriptionDetails;";


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

        public static bool Find(int VisitID, ref int PrescriptionID, ref string PrescriptionNotes, ref DateTime PrescriptionDate, ref byte PrescriptionStatus)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT PrescriptionID,
                                            PrescriptionDate,
                                          PrescriptionNotes,
                                            PrescriptionStatus
                                      FROM  Prescriptions
                                      WHERE VisitID=@VisitID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PrescriptionID = (int)reader["PrescriptionID"];
                                PrescriptionDate = (DateTime)reader["PrescriptionDate"];
                                PrescriptionNotes = reader["PrescriptionNotes"].ToStringOrEmpty();
                                PrescriptionStatus = (byte)reader["PrescriptionStatus"];

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


        public static bool UpdatePrescriptionStatus(int prescriptionId, int newStatus, SqlTransaction transaction)
        {
            string query = @"UPDATE Prescriptions 
                     SET PrescriptionStatus = @Status 
                     WHERE PrescriptionID = @PrescriptionID";

            using (SqlCommand command = new SqlCommand(query, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                command.Parameters.AddWithValue("@Status", newStatus);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool UpdatePrescriptionStatus(int prescriptionId, int newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Prescriptions 
                         SET PrescriptionStatus = @Status 
                         WHERE PrescriptionID = @PrescriptionID";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                        command.Parameters.AddWithValue("@Status", newStatus);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
        }

        public static decimal GetPrescriptionTotalForCashier(int prescriptionId)
        {
            decimal total = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT SUM((Quantity * SavedMedicinePrice) - DiscountAmount) 
                         FROM PrescriptionDetails 
                         WHERE PrescriptionID = @PrescriptionID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PrescriptionID", prescriptionId);


                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            total = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return -1;
            }

            return total;
        }
    }
}
