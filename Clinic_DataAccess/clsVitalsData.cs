using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsVitalsData
    {
        public static int AddNewVitals(int AppointmentID, string BloodPressure, decimal Temperature, decimal Weight, short Pulse)
        {
            int VitalID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Vitals (AppointmentID, BloodPressure, Temperature, Weight, Pulse, RecordedDate)
                                     VALUES (@AppointmentID, @BloodPressure, @Temperature, @Weight, @Pulse, GETDATE());
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        command.Parameters.AddWithValue("@BloodPressure", BloodPressure);
                        command.Parameters.AddWithValue("@Temperature", Temperature);
                        command.Parameters.AddWithValue("@Weight", Weight);
                        command.Parameters.AddWithValue("@Pulse", Pulse);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int newVitalID))
                        {
                            VitalID = newVitalID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                VitalID = -1;
            }
            return VitalID;
        }

        public static bool GetVitalsByAppointmentID(int AppointmentID, ref int VitalID, ref int? VisitID, ref string BloodPressure,
                                                    ref decimal Temperature, ref decimal Weight, ref short Pulse, ref DateTime RecordedDate)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM Vitals WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                VitalID = (int)reader["VitalID"];
                                VisitID = (reader["VisitID"] == DBNull.Value) ? (int?)null : (int)reader["VisitID"];
                                BloodPressure = reader["BloodPressure"].ToString();
                                Temperature = (decimal)reader["Temperature"];
                                Weight = (decimal)reader["Weight"];
                                Pulse = (short)reader["Pulse"];
                                RecordedDate = (DateTime)reader["RecordedDate"];
                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsFound = false;
            }
            return IsFound;
        }
        public static bool UpdateVitalsVisitID(int VitalID, int VisitID)
        {
            bool IsUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Vitals SET VisitID = @VisitID WHERE VitalID = @VitalID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VitalID", VitalID);
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        connection.Open();
                        IsUpdated = command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsUpdated = false;
            }
            return IsUpdated;
        }

        public static bool UpdateVitals(int VitalID, string BloodPressure, decimal Temperature, decimal Weight, short Pulse)
        {
            bool IsUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // التحديث هنا يتم بناءً على الـ VitalID الذي حصلنا عليه عند الحفظ الأول
                    string query = @"UPDATE Vitals 
                                     SET BloodPressure = @BloodPressure, 
                                         Temperature = @Temperature, 
                                         Weight = @Weight, 
                                         Pulse = @Pulse 
                                     WHERE VitalID = @VitalID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VitalID", VitalID);
                        command.Parameters.AddWithValue("@BloodPressure", BloodPressure);
                        command.Parameters.AddWithValue("@Temperature", Temperature);
                        command.Parameters.AddWithValue("@Weight", Weight);
                        command.Parameters.AddWithValue("@Pulse", Pulse);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        IsUpdated = (rowsAffected > 0);
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsUpdated = false;
            }
            return IsUpdated;
        }

        public static DataTable GetPatientsWaitingForVitals()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_PatientsWaitingForVitals 
                             WHERE StatusText = 'Waiting_For_Vitals' 
                            ORDER BY CheckInTime ASC";

                                                                
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

    }
}