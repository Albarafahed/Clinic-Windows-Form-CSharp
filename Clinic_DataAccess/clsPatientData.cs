using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsPatientData
    {
        public static bool GetPatientByPatientID(int PatientID, ref int PersonID,
                                    ref string EmergencyContact, ref int BloodTypeID,
                                    ref string MedicalHistory, ref DateTime CreatedDate,
                                    ref int CreatedByUserID)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT Patients.PersonID, Patients.EmergencyContact, Patients.BloodTypeID, 
                            Patients.MedicalHistory, Patients.CreatedDate, Patients.CreatedByUserID 
                     FROM Patients 
                     INNER JOIN Persons ON Patients.PersonID = Persons.PersonID 
                     WHERE Patients.PatientID = @PatientID AND Persons.IsDeleted = 0";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", PatientID);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PersonID = (int)reader["PersonID"];
                                EmergencyContact = reader["EmergencyContact"].ToString();
                                BloodTypeID = (int)reader["BloodTypeID"];
                                MedicalHistory = reader["MedicalHistory"].ToStringOrEmpty();
                                // تحويل آمن للتاريخ
                                CreatedDate = (DateTime)reader["CreatedDate"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
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

        public static int AddNewPatient(int PersonID, string EmergencyContact, int BloodTypeID, string MedicalHistory, DateTime CreatedDate, int CreatedByUserID)
        {


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Patients (PersonID, EmergencyContact, BloodTypeID, MedicalHistory,CreatedDate, CreatedByUserID)
                             VALUES (@PersonID, @EmergencyContact, @BloodTypeID, @MedicalHistory,@CreatedDate, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PersonID", PersonID);
                        cmd.Parameters.AddWithValue("@EmergencyContact", EmergencyContact);
                        cmd.Parameters.AddWithValue("@BloodTypeID", BloodTypeID);
                        cmd.Parameters.AddWithValue("@MedicalHistory", MedicalHistory.ToDBValue());
                        cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        // تحويل وإرجاع مباشر بدون حجز متغير مؤقت
                        if (result != null && int.TryParse(result.ToString(), out int PatientID))
                            return PatientID;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
            }

            return -1;
        }

        public static bool UpdatePatient(int PatientID, int PersonID, string EmergencyContact, int BloodTypeID, string MedicalHistory, int CreatedByUserID)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Patients SET PersonID = @PersonID, EmergencyContact = @EmergencyContact,
                             BloodTypeID = @BloodTypeID, MedicalHistory = @MedicalHistory, CreatedByUserID = @CreatedByUserID 
                             WHERE PatientID = @PatientID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", PatientID);
                        cmd.Parameters.AddWithValue("@PersonID", PersonID);
                        cmd.Parameters.AddWithValue("@EmergencyContact", EmergencyContact);
                        cmd.Parameters.AddWithValue("@BloodTypeID", BloodTypeID);
                        cmd.Parameters.AddWithValue("@MedicalHistory", MedicalHistory.ToDBValue());
                        cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
                return false;
            }
        }

        public static bool DeletePatient(int PatientID)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Patients WHERE PatientID = @PatientID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", PatientID);
                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
        }

        public static DataTable GetAllPatients()
        {
            DataTable dt = new DataTable();


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @" SELECT * FROM View_PatientsDetails
                                ORDER BY PersonID;";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
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

        public static DataRow GetPatientByID(int PatientID)
        {
            DataTable dt = new DataTable();


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @" SELECT * FROM View_PatientsDetails
                             WHERE PatientID = @PatientID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", PatientID);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
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

        public static bool IsPatientExistForPersonID(int PersonID)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT TOP 1 1 FROM Patients WHERE PersonID = @PersonID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();

                        return (command.ExecuteScalar() != null);
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