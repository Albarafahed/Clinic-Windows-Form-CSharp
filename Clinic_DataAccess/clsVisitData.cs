using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsVisitData
    {
        public static int AddNewVisit(int PatientID, string Diagnosis, DateTime VisitDate, int AppointmentID, string VisitNotes, int DoctorID, bool VisitStatus)
        {
            int VisitID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Visits (PatientID, Diagnosis, VisitDate, AppointmentID, VisitNotes, DoctorID, VisitStatus)
                                     VALUES (@PatientID, @Diagnosis, @VisitDate, @AppointmentID, @VisitNotes, @DoctorID, @VisitStatus);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PatientID", PatientID);
                        command.Parameters.AddWithValue("@Diagnosis", (object)Diagnosis ?? DBNull.Value);
                        command.Parameters.AddWithValue("@VisitDate", VisitDate);
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        command.Parameters.AddWithValue("@VisitNotes", (object)VisitNotes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@VisitStatus", VisitStatus);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            VisitID = insertedID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                VisitID = -1;
            }
            return VisitID;
        }

        public static bool GetVisitByID(int VisitID, ref int PatientID, ref string Diagnosis, ref DateTime VisitDate,
                                        ref int AppointmentID, ref string VisitNotes, ref int DoctorID, ref bool VisitStatus)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Visits WHERE VisitID = @VisitID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PatientID = (int)reader["PatientID"];
                                Diagnosis = reader["Diagnosis"] == DBNull.Value ? "" : (string)reader["Diagnosis"];
                                VisitDate = (DateTime)reader["VisitDate"];
                                AppointmentID = (int)reader["AppointmentID"];
                                VisitNotes = reader["VisitNotes"] == DBNull.Value ? "" : (string)reader["VisitNotes"];
                                DoctorID = (int)reader["DoctorID"];
                                VisitStatus = (bool)reader["VisitStatus"];
                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return IsFound;
        }

        public static bool UpdateVisit(int VisitID, string Diagnosis, string VisitNotes, bool VisitStatus)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Visits 
                                     SET Diagnosis = @Diagnosis, 
                                         VisitNotes = @VisitNotes, 
                                         VisitStatus = @VisitStatus 
                                     WHERE VisitID = @VisitID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        command.Parameters.AddWithValue("@Diagnosis", (object)Diagnosis ?? DBNull.Value);
                        command.Parameters.AddWithValue("@VisitNotes", (object)VisitNotes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@VisitStatus", VisitStatus);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
            return (rowsAffected > 0);
        }

        public static DataTable GetPatientsWaitingForDoctors(int DoctorPersonID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_PatientsWaitingForDoctors 
                                         WHERE StatusText = 'Ready_For_Doctor' 
                                         AND DoctorPersonID = @DoctorPersonID
                                         ORDER BY CheckInTime ASC;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorPersonID", DoctorPersonID);
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

    }
}