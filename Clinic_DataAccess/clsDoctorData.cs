using Clinic_DataAccess.SaveException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsDoctorData
    {
        public static bool GetDoctorByID(int DoctorID, ref int PersonID,
            ref float ConsultationFees, ref string LicenseNumber, ref bool IsActive,
                               ref DateTime CreatedDate, ref int CreatedByUserID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT  PersonID, ConsultationFees, LicenseNumber, CreatedByUserID, IsActive ,CreatedDate
                             FROM Doctors 
                             WHERE DoctorID = @DoctorID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorID", DoctorID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }
        private static List<int> GetGenericList(int DoctorID, string query, string parameterName)
        {
            List<int> listIDs = new List<int>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(parameterName, DoctorID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listIDs.Add(reader.GetInt32(0));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);

                    }
                }
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

            // تم التصحيح: حذف الأبناء أولاً لتجنب قفل القواعد (Foreign Key Constraint Violation)
            queryBuilder.AppendLine("DELETE FROM DoctorSpecialties WHERE DoctorID = @DoctorID;");
            queryBuilder.AppendLine("DELETE FROM DoctorWorkingDays WHERE DoctorID = @DoctorID;");

            // حذف الأب في النهاية بعد إخلاء جعبته من العلاقات
            queryBuilder.AppendLine("DELETE FROM Doctors WHERE DoctorID = @DoctorID;");

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                    {
                        command.Parameters.AddWithValue("@DoctorID", doctorID);
                        try
                        {
                            // تنفيذ حزمة الحذف الشاملة
                            command.ExecuteNonQuery();

                            // إذا وصلنا هنا بدون أي Exception، نعتمد الحذف فوراً
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // لو فشل حذف أي جدول (مثلاً لو الطبيب مرتبط بفواتير أو زيارات مرضي في جداول أخرى)
                            // يتراجع السيرفر كلياً ولا يحذف أي شيء ويحافظ على سلامة البيانات
                            transaction.Rollback();
                            // يفضل هنا تسجيل الخطأ لمعرفة القيد المانع للحذف: Log(ex.Message);
                            clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                            return false;
                        }
                    }
                }
            }
        }


        public static DataTable GetAllDoctors()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM View_DoctorsDetails;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);

                    }
                }
            }

            return dt;
        }

        public static DataRow GetDoctorByID(int DoctorID)
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM View_DoctorsDetails WHERE DoctorID=@DoctorID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DoctorID", DoctorID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);

                    }
                }
            }

            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        private static string GetGenericString(int DoctorID, string query, string parameterName, string defaultValue)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(parameterName, DoctorID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        // نتحقق إذا كانت النتيجة فارغة (null) أو فارغة نصياً (DBNull)
                        return (result != null && result != DBNull.Value) ? result.ToString() : defaultValue;
                    }
                    catch (Exception ex)
                    {
                        // في حالة حدوث خطأ في قاعدة البيانات، نعيد القيمة الافتراضية أيضاً
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                        return defaultValue;
                    }
                }
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

        public static bool IsDoctorExistForPersonID(int PersonID)
        {
            // استخدام TOP 1 1 هي الطريقة الأكثر احترافية للتحقق من الوجود
            string query = "SELECT TOP 1 1 FROM Doctors WHERE PersonID = @PersonID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();

                    // ExecuteScalar هنا ستعيد 1 إذا وجد السجل، أو null إذا لم يوجد
                    return (command.ExecuteScalar() != null);
                }
            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);

                return false;
            }
        }

        public static bool IsDoctorBusy(int DoctorID, DateTime AppointmentDate)
        {
            bool IsBusy = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT 1 FROM Appointments 
                         WHERE DoctorID = @DoctorID 
                         AND AppointmentDate = @AppointmentDate 
                         AND AppointmentStatus = 1"; // 1 = Scheduled

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorID", DoctorID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null) IsBusy = true;
                }
                catch (Exception ex)
                {
                    clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                }
            }
            return IsBusy;
        }

        public static bool IsDoctorWorkingOnThisDay(int DoctorID, DateTime AppointmentDate)
        {
            // تحويل يوم C# نظامي (السبت=1، الأحد=2... الجمعة=7)
            int dayOfWeekCS = (int)AppointmentDate.DayOfWeek; // 0=Sun, 1=Mon ... 6=Sat
            int MyDayID = (dayOfWeekCS == 6) ? 1 : (dayOfWeekCS + 2);

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT 1 FROM DoctorWorkingDays 
                         WHERE DoctorID = @DoctorID AND DayID = @DayID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorID", DoctorID);
                command.Parameters.AddWithValue("@DayID", MyDayID);

                try
                {
                    connection.Open();
                    return command.ExecuteScalar() != null;
                }
                catch (Exception ex)
                {
                    clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                    return false;
                }
            }
        }

        public static bool IsDoctorWorkingAtThisTime(int DoctorID, DateTime AppointmentDateTime)
        {
            int dayOfWeekCS = (int)AppointmentDateTime.DayOfWeek;
            int dayOfWeek = (dayOfWeekCS == 6) ? 1 : (dayOfWeekCS + 2);
            TimeSpan appointmentTime = AppointmentDateTime.TimeOfDay;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // نتحقق هل الوقت يقع بين فترة بداية ونهاية الدوام في ذلك اليوم
                string query = @"SELECT 1 FROM DoctorWorkingDays 
                         WHERE DoctorID = @DoctorID 
                         AND DayID = @DayID 
                         AND @Time BETWEEN StartTime AND EndTime";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DoctorID", DoctorID);
                    command.Parameters.AddWithValue("@DayID", dayOfWeek);
                    command.Parameters.AddWithValue("@Time", appointmentTime);
                    try
                    {
                        connection.Open();
                        return command.ExecuteScalar() != null;
                    }
                    catch (Exception ex)
                    {
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                        return false;
                    }

                }
            }
        }

        public static DataTable GetAllDays()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Days order by DayID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                    }
                }
            }
            return dt;
        }


        public static DataTable GetDoctorShifts(int DoctorID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT DW.DayID,
                                    D.DayName, DW.StartTime, DW.EndTime
                                FROM DoctorWorkingDays DW INNER JOIN Days D ON D.DayID = DW.DayID
                            WHERE DW.DoctorID = @DoctorID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorID", DoctorID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dt.Load(reader);

                    }
                }
                catch (Exception ex)
                {
                    clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                }

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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
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

                    try
                    {
                        object result = cmd.ExecuteScalar();
                        transaction.Commit();
                        return (result != null) ? Convert.ToInt32(result) : -1;
                    }
                    catch (Exception ex)
                    {
                        // يمكنك طباعة الخطأ هنا للتشخيص: Debug.WriteLine(ex.Message);
                        transaction.Rollback();
                        clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
                        return -1;
                    }
                }
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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
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

                    try
                    {
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        {
                            transaction.Rollback();
                            clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error, CreatedByUserID);
                            return false;
                        }
                    }
                }
            }

        }
    }
}