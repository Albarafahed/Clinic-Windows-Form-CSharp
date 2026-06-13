using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Clinic_DataAccess
{
    public class clsVisitData
    {
        public static int SaveVisitAndLinkVitals(int AppointmentID, int PatientID, string Diagnosis,
                                                 string VisitNotes, int DoctorID, DateTime VisitDate,
                                                 int CreatedByUserID, decimal TotalAmount)
        {
            int VisitID = -1;

            string query = @"INSERT INTO Visits (PatientID, Diagnosis, VisitDate, AppointmentID, VisitNotes, DoctorID, VisitStatus, TotalAmount, CreatedByUserID)
                             VALUES (@PatientID, @Diagnosis, @VisitDate, @AppointmentID, @VisitNotes, @DoctorID, 2, @TotalAmount, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);
                    cmd.Parameters.AddWithValue("@Diagnosis", Diagnosis );
                    cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmd.Parameters.AddWithValue("@VisitNotes", VisitNotes.ToDBValue());
                    cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                    cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            cmd.Transaction = transaction;
                            object result = cmd.ExecuteScalar();

                            if (result != null && int.TryParse(result.ToString(), out VisitID))
                            {
                                string queryVitals = "UPDATE Vitals SET VisitID = @VisitID WHERE AppointmentID = @AppointmentID AND VisitID IS NULL";
                                using (SqlCommand cmdVitals = new SqlCommand(queryVitals, connection, transaction))
                                {
                                    cmdVitals.Parameters.AddWithValue("@VisitID", VisitID);
                                    cmdVitals.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                                    cmdVitals.ExecuteNonQuery();
                                }

                                string queryBils = "UPDATE Bills SET VisitID = @VisitID WHERE AppointmentID = @AppointmentID AND VisitID IS NULL";
                                using (SqlCommand cmdVitals = new SqlCommand(queryBils, connection, transaction))
                                {
                                    cmdVitals.Parameters.AddWithValue("@VisitID", VisitID);
                                    cmdVitals.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                                    cmdVitals.ExecuteNonQuery();
                                }

                                // تحديث حالة الموعد
                                string queryAppt = "UPDATE Appointments SET AppointmentStatus = 9 WHERE AppointmentID = @AppointmentID";
                                using (SqlCommand cmdAppt = new SqlCommand(queryAppt, connection, transaction))
                                {
                                    cmdAppt.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                                    cmdAppt.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        clsGlobalLogger.LogSqlException(ex,clsGlobalLogger.LogLevel.Error);
                        VisitID = -1;
                    }
                }
            }
            return VisitID;
        }

        public static bool GetVisitByID(int VisitID, ref int PatientID, ref string Diagnosis, ref DateTime VisitDate,
                                        ref int AppointmentID, ref string VisitNotes, ref int DoctorID,
                                        ref byte VisitStatus, ref decimal TotalAmount, ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = "SELECT * FROM Visits WHERE VisitID = @VisitID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VisitID", VisitID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PatientID = (int)reader["PatientID"];
                            Diagnosis = reader["Diagnosis"].ToString();
                            VisitDate = (DateTime)reader["VisitDate"];
                            AppointmentID = (int)reader["AppointmentID"];
                            VisitNotes = reader["VisitNotes"].ToStringOrEmpty();
                            DoctorID = (int)reader["DoctorID"];
                            VisitStatus = (byte)reader["VisitStatus"];
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsFound = true;
                        }
                    }
                }
                catch (SqlException ex) { 
                    clsGlobalLogger.LogSqlException(ex,clsGlobalLogger.LogLevel.Error);
                    IsFound = false;
                }
            }
            return IsFound;
        }

        
        public static bool UpdateVisit(int VisitID, string Diagnosis, string VisitNotes, byte VisitStatus,int CreatedByUserID)
        {
            if (IsVisitClosed(VisitID)) return false;

           try
            {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Visits SET Diagnosis = @Diagnosis,
                  VisitNotes = @VisitNotes, 
                  VisitStatus = @VisitStatus ,
                  CreatedByUserID=@CreatedByUserID
                   WHERE VisitID = @VisitID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        command.Parameters.AddWithValue("@Diagnosis", Diagnosis);
                        command.Parameters.AddWithValue("@VisitNotes", VisitNotes.ToDBValue());
                        command.Parameters.AddWithValue("@VisitStatus", VisitStatus);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);

                    }
                }


            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }


        }

        public static bool UpdateVisitTotalAmount(int VisitID, SqlTransaction transaction)
        {
           
            string query = @"
                            UPDATE Visits 
                            SET TotalAmount = TotalAmount+
                                (SELECT ISNULL(SUM(PPD.SavedMedicinePrice * PPD.Quantity), 0) 
                                 FROM PrescriptionDetails PPD 
                                 WHERE PPD.PrescriptionID = (SELECT PrescriptionID FROM Visits WHERE VisitID = @VisitID))
                                +
                                (SELECT ISNULL(SUM(SD.SavedServicePrice * SD.Quantity - SD.Discount), 0) 
                                 FROM VisitServices SD 
                                 WHERE SD.VisitID = @VisitID)
                            WHERE VisitID = @VisitID";

            // استخدم الـ transaction.Connection مباشرة
            using (SqlCommand cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@VisitID", VisitID);
                return cmd.ExecuteNonQuery() > 0;
            }
        
        
        }

        public static bool IsVisitClosed(int VisitID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT COUNT(*) FROM Visits WHERE VisitID = @VisitID AND VisitStatus = 3";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@VisitID", VisitID);
                        connection.Open();
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }

           
        }

        public static DataTable GetPatientsWaitingForDoctors(int DoctorPersonID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT AppointmentID
                                          ,PatientName
                                          ,CheckInTime
                                          ,StatusText
                                          ,IsCalled
                                      FROM View_PatientsWaitingForDoctors
                                      WHERE DoctorPersonID=@DoctorPersonID and StatusText in('Ready_For_Doctor','In-Progress')
                                      ORDER BY CheckInTime;";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@DoctorPersonID", DoctorPersonID);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
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


        public static bool DeleteVisit(int VisitID, int AppointmentID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Vitals 
                                      SET VisitID=NULL
                                      WHERE AppointmentID=@AppointmentID;
                        DELETE FROM Visits WHERE VisitID = @VisitID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
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
            return rowsAffected > 0;
        }

        public static DataTable GetAllVisits()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_VisitDetails
                                  ORDER BY VisitDate DESC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
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