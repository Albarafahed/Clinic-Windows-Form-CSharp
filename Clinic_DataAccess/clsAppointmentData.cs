using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsAppointmentData
    {
        public static bool GetAppointmentByID(int AppointmentID, ref int PatientID, ref int DoctorID,
            ref int CreatedByUserID, ref int AppointmentStatus, ref int AppointmentTypeID,
            ref decimal AppointmentFees, ref DateTime AppointmentDate,ref DateTime CreatedDate)
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
                                AppointmentStatus = (int)reader["AppointmentStatus"];
                                AppointmentTypeID = (int)reader["AppointmentTypeID"];
                                AppointmentFees = (decimal)reader["AppointmentFees"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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
            int AppointmentStatus, int AppointmentTypeID, decimal AppointmentFees, DateTime AppointmentDate)
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
            int AppointmentStatus, int AppointmentTypeID, decimal AppointmentFees, DateTime AppointmentDate)
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

        public static bool IsDoctorBusy(int DoctorID, DateTime AppointmentDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 1 FROM Appointments 
                                    WHERE DoctorID = @DoctorID 
                                    AND AppointmentDate = @AppointmentDate 
                                     AND AppointmentStatus !=6"; // 6 = Canceled

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

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

        public static bool IsPatinentBlakListed(int PatientID)
        {
            bool IsBlacklisted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"Select 1 From PatientBlacklist
                              Where PatientID=@PatientID
                                and UnbanDate IS NULL";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PatientID", PatientID);
                        connection.Open();
                        return (command.ExecuteScalar() != null);

                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return IsBlacklisted;
        }

        private static bool _IsPatientAlreadyBookedInTimeRange(int PatientID, DateTime AppointmentDateTime, TimeSpan range)
        {
            bool HasAppointment = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"
                                SELECT TOP 1 1 
                                FROM Appointments 
                                WHERE PatientID = @PatientID 
                                AND AppointmentStatus != 6
                                AND AppointmentDate BETWEEN @StartRange AND @EndRange";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // حساب نطاق الوقت (قبل وبعد الموعد بـ 24 ساعة)
                        DateTime startRange = AppointmentDateTime.Subtract(range);
                        DateTime endRange = AppointmentDateTime.Add(range);

                        command.Parameters.AddWithValue("@PatientID", PatientID);
                        command.Parameters.AddWithValue("@StartRange", startRange);
                        command.Parameters.AddWithValue("@EndRange", endRange);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        return (result != null);
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return HasAppointment;
        }

        public static bool IsPatientHasConflict(int PatientID, DateTime AppointmentDateTime)
        {
            // نحدد النطاق الزمني الذي نعتبره "تضارباً" (مثلاً 24 ساعة كما اتفقنا)
            TimeSpan conflictRange = TimeSpan.FromHours(24);

            // استدعاء دالة التحقق بنطاق زمني
            return _IsPatientAlreadyBookedInTimeRange(PatientID, AppointmentDateTime, conflictRange);
        }
        private static int GetDayID(DateTime dateTime)
        {
            int DayOfWeek =(int) dateTime.DayOfWeek;

            return (DayOfWeek==6)?1:(DayOfWeek+2);
        }
        public static bool IsTimeCapacityAvailable(int DoctorID, DateTime AppointmentDate, int AppointmentTypeID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // الاستعلام المحدث ليتوافق مع جداولك الحالية
                    string query = @"
            -- 1. جلب مدة الموعد الجديد
            DECLARE @NewDuration INT = (SELECT TOP 1 DefaultDuration FROM AppointmentTypes WHERE AppointmentTypeID = @AppointmentTypeID);
            
            -- 2. حساب السعة المتبقية من اليوم (فقط الفترات المستقبلية)
            DECLARE @DailyCap INT = (SELECT ISNULL(SUM(DATEDIFF(MINUTE, 
                                     CASE 
                                        WHEN StartTime > CAST(@Date AS TIME) THEN StartTime 
                                        ELSE CAST(@Date AS TIME) 
                                     END, EndTime)), 0)
                                     FROM DoctorWorkingDays 
                                     WHERE DoctorID = @DoctorID AND DayID = @DayID
                                     AND EndTime > CAST(@Date AS TIME));

            -- 3. حساب المستهلك من المواعيد المستقبلية فقط
            DECLARE @Consumed INT = (SELECT ISNULL(SUM(AT.DefaultDuration), 0)
                                     FROM Appointments A
                                     INNER JOIN AppointmentTypes AT ON A.AppointmentTypeID = AT.AppointmentTypeID
                                     WHERE A.DoctorID = @DoctorID
                                     AND CAST(A.AppointmentDate AS DATE) = CAST(@Date AS DATE)
                                     AND A.AppointmentStatus != 6 
                                     AND A.AppointmentDate > GETDATE());
                                     
            SELECT CASE WHEN (@Consumed + @NewDuration) <= @DailyCap THEN 1 ELSE 0 END;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                        cmd.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);
                        cmd.Parameters.AddWithValue("@DayID", GetDayID(AppointmentDate));
                        cmd.Parameters.AddWithValue("@Date", AppointmentDate.Date);

                        conn.Open();
                        return Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex,clsGlobalLogger.LogLevel.Error);
                return false;
            }
                
            }

    }
    }
