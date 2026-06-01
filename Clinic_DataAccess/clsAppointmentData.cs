using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsAppointmentData
    {
        public static bool GetAppointmentByID(int AppointmentID, ref int PatientID, ref int DoctorID,
            ref int CreatedByUserID, ref short AppointmentStatus, ref int AppointmentTypeID,
            ref decimal AppointmentFees, ref DateTime AppointmentDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM Appointments WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                PatientID = (int)reader["PatientID"];
                                DoctorID = (int)reader["DoctorID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                AppointmentStatus = (short)reader["AppointmentStatus"];
                                AppointmentTypeID = (int)reader["AppointmentTypeID"];
                                AppointmentFees = (decimal)reader["AppointmentFees"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                return true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
            return false;
        }


        public static DataRow GetAppointmentInfoByID(int AppointmentID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_AppointmentsDetails WHERE AppointmentID = @AppointmentID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

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

        public static int AddNewAppointment(int PatientID, int DoctorID, int CreatedByUserID,
            short AppointmentStatus, int AppointmentTypeID, decimal AppointmentFees, DateTime AppointmentDate)
        {
            int AppointmentID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Appointments (PatientID, DoctorID, CreatedByUserID, AppointmentStatus, AppointmentTypeID, AppointmentFees, AppointmentDate)
                                 VALUES (@PatientID, @DoctorID, @CreatedByUserID, @AppointmentStatus, @AppointmentTypeID, @AppointmentFees, @AppointmentDate);
                                 SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PatientID", PatientID);
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@AppointmentStatus", AppointmentStatus);
                        command.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);
                        command.Parameters.AddWithValue("@AppointmentFees", AppointmentFees);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);


                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            AppointmentID = insertedID;
                    }
                }

            }

            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
            }

            return AppointmentID;
        }

        public static bool UpdateAppointment(int AppointmentID, int PatientID, int DoctorID, int CreatedByUserID,
            short AppointmentStatus, int AppointmentTypeID, decimal AppointmentFees, DateTime AppointmentDate)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Appointments SET PatientID = @PatientID, DoctorID = @DoctorID, 
                                 CreatedByUserID = @CreatedByUserID, AppointmentStatus = @AppointmentStatus, 
                                 AppointmentTypeID = @AppointmentTypeID, AppointmentFees = @AppointmentFees, 
                                 AppointmentDate = @AppointmentDate
                                 WHERE AppointmentID = @AppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        command.Parameters.AddWithValue("@PatientID", PatientID);
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@AppointmentStatus", AppointmentStatus);
                        command.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);
                        command.Parameters.AddWithValue("@AppointmentFees", AppointmentFees);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);


                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteAppointment(int AppointmentID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);


                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllAppointments()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_AppointmentsDetails";
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


        public static bool DoesAppointmentExist(int AppointmentID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT Found = 1 FROM Appointments WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null) IsFound = true;

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

        public static bool UpdateAppointmentStatus(int AppointmentID, short NewStatus)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "UPDATE Appointments SET AppointmentStatus = @NewStatus WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }

            return (rowsAffected > 0);
        }
    }
}