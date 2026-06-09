using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Clinic_DataAccess
{

    public class clsDoctorData
    {
        public static bool GetDoctorByID(int DoctorID, ref int PersonID,
            ref float ConsultationFees, ref string LicenseNumber, ref bool IsActive,
                               ref DateTime CreatedDate, ref int CreatedByUserID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT  PersonID, ConsultationFees,
                        LicenseNumber, CreatedByUserID,
                            IsActive ,CreatedDate
                             FROM Doctors 
                             WHERE DoctorID = @DoctorID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        {
                            command.Parameters.AddWithValue("@DoctorID", DoctorID);
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    PersonID = reader.GetInt32(0);
                                    ConsultationFees = Convert.ToSingle(reader["ConsultationFees"]);
                                    LicenseNumber = reader.GetString(2);
                                    CreatedByUserID = reader.GetInt32(3);
                                    IsActive = reader.GetBoolean(4);
                                    CreatedDate = reader.GetDateTime(5);
                                    IsFound = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }

            return IsFound;
        }
        private static List<int> GetGenericList(int DoctorID, string query, string parameterName)
        {
            List<int> listIDs = new List<int>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(parameterName, DoctorID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listIDs.Add(reader.GetInt32(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

            }

            return listIDs;
        }

        public static List<int> GetDoctorSpecialtyIDs(int DoctorID)
        {
            string query = "SELECT SpecializationID FROM DoctorSpecialties WHERE DoctorID = @DoctorID";
            return GetGenericList(DoctorID, query, "@DoctorID");
        }

        public static bool DeleteDoctor(int doctorID)
        {
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.AppendLine("DELETE FROM DoctorSpecialties WHERE DoctorID = @DoctorID;");
            queryBuilder.AppendLine("DELETE FROM DoctorWorkingDays WHERE DoctorID = @DoctorID;");
            queryBuilder.AppendLine("DELETE FROM Doctors WHERE DoctorID = @DoctorID;");

            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    using (transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                        {
                            command.Parameters.AddWithValue("@DoctorID", doctorID);

                            // تنفيذ حزمة الحذف الشاملة
                            command.ExecuteNonQuery();

                            // إذا وصلنا هنا بدون أي SqlException، نعتمد الحذف فوراً
                            transaction.Commit();
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }



        }
        public static DataTable GetAllDoctors()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_DoctorsDetails;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
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

        public static DataRow GetDoctorByID(int DoctorID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_DoctorsDetails WHERE DoctorID=@DoctorID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
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

            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        private static string GetGenericString(int DoctorID, string query, string parameterName, string defaultValue)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(parameterName, DoctorID);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        // نتحقق إذا كانت النتيجة فارغة (null) أو فارغة نصياً (DBNull)
                        return (result != null && result != DBNull.Value) ? result.ToString() : defaultValue;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return defaultValue;
            }


        }

        public static string GetSpecializations(int DoctorID)
        {
            string query = @"SELECT STRING_AGG(S.SpecializationName, ', ') AS Specializations
                     FROM DoctorSpecialties DS 
                     INNER JOIN Specializations S ON S.SpecializationID = DS.SpecializationID 
                     WHERE DS.DoctorID = @DoctorID;";

            return GetGenericString(DoctorID, query, "@DoctorID", "No Specializations Found");
        }

        public static string GetWorkingDays(int DoctorID)
        {
            string query = @"SELECT STRING_AGG(DA.DayName, ', ') AS WorkingDays
                     FROM DoctorWorkingDays DW 
                     INNER JOIN Days DA ON DA.DayID = DW.DayID 
                     WHERE DW.DoctorID = @DoctorID;";

            return GetGenericString(DoctorID, query, "@DoctorID", "No Working Days Set");
        }

        public static float GetConsultationFees(int DoctorID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "Select ConsultationFees From Doctors Where DoctorID=@DoctorID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        connection.Open ();
                        object result = command.ExecuteScalar();
                        if (result != null)
                            return Convert.ToSingle(result);

                    }
                }

            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

            }
            return 0.0F;
        }
        public static bool IsDoctorExistForPersonID(int PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT TOP 1 1 FROM Doctors WHERE PersonID = @PersonID;";
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

    
        public static bool IsDoctorWorkingOnThisDay(int DoctorID, DateTime AppointmentDate)
        {
            // تحويل يوم C# نظامي (السبت=1، الأحد=2... الجمعة=7)
            int dayOfWeekCS = (int)AppointmentDate.DayOfWeek; // 0=Sun, 1=Mon ... 6=Sat
            int MyDayID = (dayOfWeekCS == 6) ? 1 : (dayOfWeekCS + 2);

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 1 FROM DoctorWorkingDays 
                         WHERE DoctorID = @DoctorID AND DayID = @DayID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@DayID", MyDayID);
                        connection.Open();
                        return command.ExecuteScalar() != null;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }

        }

        public static bool IsDoctorWorkingAtThisTime(int DoctorID, DateTime AppointmentDateTime)
        {
            int dayOfWeekCS = (int)AppointmentDateTime.DayOfWeek;
            int dayOfWeek = (dayOfWeekCS == 6) ? 1 : (dayOfWeekCS + 2);
            TimeSpan appointmentTime = AppointmentDateTime.TimeOfDay;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 1 FROM DoctorWorkingDays 
                         WHERE DoctorID = @DoctorID 
                         AND DayID = @DayID 
                         AND @Time BETWEEN StartTime AND EndTime";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@DayID", dayOfWeek);
                        command.Parameters.AddWithValue("@Time", appointmentTime);


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

        public static DataTable GetAllDays()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Days order by DayID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
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

        public static DataTable GetDoctorShifts(int DoctorID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT DW.DayID,
                                    D.DayName, DW.StartTime, DW.EndTime
                                FROM DoctorWorkingDays DW INNER JOIN Days D ON D.DayID = DW.DayID
                            WHERE DW.DoctorID = @DoctorID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
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

        public static int AddNewDoctor(int PersonID, float ConsultationFees, string LicenseNumber,
                                int CreatedByUserID, bool IsActive,
                                List<int> SelectedSpecialtyIDs,
                                List<int> DayIDs, List<TimeSpan> StartTimes, List<TimeSpan> EndTimes)
        {
            StringBuilder queryBuilder = new StringBuilder();

            // 1. إضافة الطبيب الأساسي
            queryBuilder.AppendLine(@"INSERT INTO Doctors (PersonID, ConsultationFees, LicenseNumber, CreatedByUserID, IsActive) 
                             VALUES (@PersonID, @ConsultationFees, @LicenseNumber, @CreatedByUserID, @IsActive);
                             DECLARE @NewDoctorID INT = SCOPE_IDENTITY();");

            // 2. إضافة التخصصات
            for (int i = 0; i < (SelectedSpecialtyIDs?.Count ?? 0); i++)
                queryBuilder.AppendLine($"INSERT INTO DoctorSpecialties (DoctorID, SpecializationID) VALUES (@NewDoctorID, @SpecID{i});");

            // 3. إضافة فترات العمل (باستخدام المصفوفات المنفصلة)
            for (int i = 0; i < (DayIDs?.Count ?? 0); i++)
                queryBuilder.AppendLine($"INSERT INTO DoctorWorkingDays (DoctorID, DayID, StartTime, EndTime) VALUES (@NewDoctorID, @DayID{i}, @Start{i}, @End{i});");

            queryBuilder.AppendLine("SELECT @NewDoctorID;");

            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    using (transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                        {
                            // إضافة البارامترات الأساسية
                            cmd.Parameters.AddWithValue("@PersonID", PersonID);
                            cmd.Parameters.AddWithValue("@ConsultationFees", ConsultationFees);
                            cmd.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                            cmd.Parameters.AddWithValue("@IsActive", IsActive);

                            // إضافة بارامترات التخصصات
                            for (int i = 0; i < (SelectedSpecialtyIDs?.Count ?? 0); i++)
                                cmd.Parameters.AddWithValue($"@SpecID{i}", SelectedSpecialtyIDs[i]);

                            // إضافة بارامترات فترات العمل
                            for (int i = 0; i < (DayIDs?.Count ?? 0); i++)
                            {
                                cmd.Parameters.AddWithValue($"@DayID{i}", DayIDs[i]);
                                cmd.Parameters.AddWithValue($"@Start{i}", StartTimes[i]);
                                cmd.Parameters.AddWithValue($"@End{i}", EndTimes[i]);
                            }
                            object result = cmd.ExecuteScalar();
                            transaction.Commit();
                            return (result != null) ? Convert.ToInt32(result) : -1;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
                return -1;
            }


        }

        public static bool UpdateDoctor(int DoctorID, int PersonID, float ConsultationFees,
                                string LicenseNumber, int CreatedByUserID, bool IsActive,
                                List<int> SelectedSpecialtyIDs,
                                List<int> DayIDs, List<TimeSpan> StartTimes, List<TimeSpan> EndTimes)
        {
            StringBuilder queryBuilder = new StringBuilder();

            // 1. تحديث البيانات الأساسية
            queryBuilder.AppendLine(@"UPDATE Doctors 
                              SET PersonID=@PersonID, ConsultationFees=@ConsultationFees, 
                                  LicenseNumber=@LicenseNumber, CreatedByUserID=@CreatedByUserID, 
                                  IsActive=@IsActive 
                              WHERE DoctorID=@DoctorID;");

            // 2. حذف البيانات القديمة لضمان عدم التكرار (Clean and Re-insert)
            queryBuilder.AppendLine("DELETE FROM DoctorSpecialties WHERE DoctorID = @DoctorID;");
            queryBuilder.AppendLine("DELETE FROM DoctorWorkingDays WHERE DoctorID = @DoctorID;");

            // 3. إعادة إدراج التخصصات
            for (int i = 0; i < (SelectedSpecialtyIDs?.Count ?? 0); i++)
                queryBuilder.AppendLine($"INSERT INTO DoctorSpecialties (DoctorID, SpecializationID) VALUES (@DoctorID, @SpecID{i});");

            // 4. إعادة إدراج الفترات (Shifts)
            for (int i = 0; i < (DayIDs?.Count ?? 0); i++)
                queryBuilder.AppendLine($"INSERT INTO DoctorWorkingDays (DoctorID, DayID, StartTime, EndTime) VALUES (@DoctorID, @DayID{i}, @Start{i}, @End{i});");
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    using (transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                            cmd.Parameters.AddWithValue("@PersonID", PersonID);
                            cmd.Parameters.AddWithValue("@ConsultationFees", ConsultationFees);
                            cmd.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                            cmd.Parameters.AddWithValue("@IsActive", IsActive);

                            for (int i = 0; i < (SelectedSpecialtyIDs?.Count ?? 0); i++)
                                cmd.Parameters.AddWithValue($"@SpecID{i}", SelectedSpecialtyIDs[i]);

                            for (int i = 0; i < (DayIDs?.Count ?? 0); i++)
                            {
                                cmd.Parameters.AddWithValue($"@DayID{i}", DayIDs[i]);
                                cmd.Parameters.AddWithValue($"@Start{i}", StartTimes[i]);
                                cmd.Parameters.AddWithValue($"@End{i}", EndTimes[i]);
                            }


                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
                return false;

            }

        }

        public static DataTable LoadQueueData()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = @"SELECT
                                        ROW_NUMBER() OVER (ORDER BY A.AppointmentDate ASC) AS QueueNumber, 
                                        Pe.FullName AS PatientName
                                    FROM Appointments A inner join Patients P ON A.PatientID=P.PatientID
                                      INNER JOIN Persons Pe ON P.PersonID=Pe.PersonID
                                    WHERE A.AppointmentStatus = 1;";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
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

        public static bool IsDoctorAvailable(int DoctorID, DateTime AppointmentDateTime)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // دمجنا الفحص: هل يعمل اليوم؟ + هل الوقت ضمن الدوام؟ + هل لديه موعد آخر؟
                    string query = @"
                -- 1. التأكد من يوم العمل وساعات الدوام
                IF EXISTS (
                    SELECT 1 FROM DoctorWorkingDays 
                    WHERE DoctorID = @DoctorID 
                    AND DayID = @DayID 
                    AND @Time BETWEEN StartTime AND EndTime
                )
                -- 2. التأكد من عدم وجود تضارب (موعد في نفس اللحظة)
                AND NOT EXISTS (
                    SELECT 1 FROM Appointments 
                    WHERE DoctorID = @DoctorID 
                    AND AppointmentDate = @AppointmentDateTime 
                    AND AppointmentStatus != 6
                )
                SELECT 1 ELSE SELECT 0;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int dayOfWeekCS = (int)AppointmentDateTime.DayOfWeek;
                        int dayID = (dayOfWeekCS == 6) ? 1 : (dayOfWeekCS + 2);

                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@DayID", dayID);
                        command.Parameters.AddWithValue("@Time", AppointmentDateTime.TimeOfDay);
                        command.Parameters.AddWithValue("@AppointmentDateTime", AppointmentDateTime);

                        connection.Open();
                        return Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
        }


        public static DataTable GetAllDoctorsForQueue()
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT D.DoctorID, P.FullName 
                         FROM Doctors D
                         INNER JOIN Persons P ON D.PersonID = P.PersonID;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
                        }
                    }
                }

            }
            catch(SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return dt;
        }
    }
}