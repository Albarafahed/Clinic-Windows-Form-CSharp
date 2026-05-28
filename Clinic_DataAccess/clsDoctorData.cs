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

        public static bool GetDoctorByID(int DoctorID,ref int PersonID, 
            ref float ConsultationFees,ref string LicenseNumber, ref bool IsActive,
                               ref DateTime CreatedDate,  ref int CreatedByUserID )
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
                    ConsultationFees =Convert.ToSingle( reader["ConsultationFees"]);
                    LicenseNumber = reader.GetString(2);
                    CreatedByUserID = reader.GetInt32(3);
                    IsActive = reader.GetBoolean(4);
                    CreatedDate= reader.GetDateTime(5);
                    IsFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }
        // دالة عامة لجلب قائمة من الأرقام (IDs) لأي جدول فرعي مرتبط بالدكتور
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
                        // هنا يفضل تسجيل الخطأ في ملف Log
                    }
                }
            }
            return listIDs;
        }

        public static List<int> GetDoctorSpecialtyIDs(int DoctorID)
        {
            string query = "SELECT SpecializationID FROM DoctorSpecialties WHERE DoctorID = @DoctorID";
            return GetGenericList(DoctorID,query, "@DoctorID");
        }

        public static List<int> GetDoctorWorkingDayIDs(int DoctorID)
        {
            string query = "SELECT DayID FROM DoctorWorkingDays WHERE DoctorID = @DoctorID";
            return GetGenericList(DoctorID, query, "@DoctorID");
        }


        public static int AddNewDoctor(int PersonID,float ConsultationFees, 
                                       string LicenseNumber,int CreatedByUserID,
                                       bool IsActive,
                              List<int> SelectedDayIDs, List<int> SelectedSpecialtyIDs)
        {
            // 1. بناء نص الاستعلام المجمع
            StringBuilder queryBuilder = new StringBuilder();

            // سكريبت إدخال الطبيب الأساسي وتخزين معرفه
            queryBuilder.AppendLine(@"
                                DECLARE @NewDoctorID INT;
                                INSERT INTO Doctors (PersonID, ConsultationFees, LicenseNumber, CreatedByUserID, IsActive) 
                                VALUES (@PersonID, @ConsultationFees, @LicenseNumber, @CreatedByUserID, @IsActive); 
                                SET @NewDoctorID = SCOPE_IDENTITY();");

            // تم التصحيح: فصل شروط الـ IF لضمان بناء استعلام كل جدول بشكل مستقل وآمن
            if (SelectedSpecialtyIDs != null && SelectedSpecialtyIDs.Count > 0)
            {
                for (int i = 0; i < SelectedSpecialtyIDs.Count; i++)
                {
                    queryBuilder.AppendLine($"INSERT INTO DoctorSpecialties (DoctorID, SpecializationID) VALUES (@NewDoctorID, @SpecID{i});");
                }
            }

            if (SelectedDayIDs != null && SelectedDayIDs.Count > 0)
            {
                for (int i = 0; i < SelectedDayIDs.Count; i++)
                {
                    queryBuilder.AppendLine($"INSERT INTO DoctorWorkingDays (DoctorID, DayID) VALUES (@NewDoctorID, @DayID{i});");
                }
            }

            // تم التصحيح: إرجاع المعرف الجديد لكي تقرأه دالة ExecuteScalar بنجاح
            queryBuilder.AppendLine("SELECT @NewDoctorID;");

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                    {
                        // تمرير معاملات الطبيب الأساسية
                        cmd.Parameters.AddWithValue("@PersonID", PersonID);
                        cmd.Parameters.AddWithValue("@ConsultationFees", ConsultationFees);
                        cmd.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                        cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        cmd.Parameters.AddWithValue("@IsActive", IsActive);

                        // تمرير معاملات التخصصات المرقمة بأمان (مع فحص كل قائمة بشكل مستقل)
                        if (SelectedSpecialtyIDs != null)
                        {
                            for (int i = 0; i < SelectedSpecialtyIDs.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@SpecID{i}", SelectedSpecialtyIDs[i]);
                            }
                        }

                        if (SelectedDayIDs != null)
                        {
                            for (int i = 0; i < SelectedDayIDs.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@DayID{i}", SelectedDayIDs[i]);
                            }
                        }

                        try
                        {
                            // 2. قراءة القيمة المفردة المرتجعة (رقم الدكتور الجديد)
                            object result = cmd.ExecuteScalar();

                            // التحقق من أن النتيجة تمت قراءتها بنجاح وتحويلها لـ int
                            if (result != null && int.TryParse(result.ToString(), out int insertedDoctorID))
                            {
                                transaction.Commit(); // اعتماد العملية بالكامل للثلاثة جداول معاً!
                                return insertedDoctorID; // نعود بالمعرف الجديد بنجاح فائق
                            }

                            transaction.Rollback();
                            return -1;
                        }
                        catch (Exception)
                        {
                            // السحر هنا: لو انهار إدخال أي تخصص أو أي يوم عمل، يتم إلغاء الطبيب والتخصصات والأيام كلياً!
                            transaction.Rollback();
                            return -1;
                        }
                    }
                }
            }
        }

        public static bool UpdateDoctor(int DoctorID, int PersonID, float ConsultationFees,
                                       string LicenseNumber, int CreatedByUserID,
                                       bool IsActive, List<int> SelectedDayIDs, 
                                       List<int> SelectedSpecialtyIDs)
        {
            // 1. بناء نص الاستعلام المجمع لتحديث البيانات والمسح وإعادة الإدخال
            StringBuilder queryBuilder = new StringBuilder();

            // أ. تحديث سعر الكشفية في جدول الأطباء الأساسي
            queryBuilder.AppendLine(@"UPDATE Doctors SET
                                            PersonID = @PersonID,
                                            ConsultationFees = @ConsultationFees,
                                            LicenseNumber = @LicenseNumber,
                                            CreatedByUserID = @CreatedByUserID,
                                            IsActive = @IsActive
                                            WHERE DoctorID = @DoctorID;");

            // ب. مسح العلاقات القديمة تماماً لتجهيز الجدولين للاستقبال الجديد
            queryBuilder.AppendLine("DELETE FROM DoctorSpecialties WHERE DoctorID = @DoctorID;");
            queryBuilder.AppendLine("DELETE FROM DoctorWorkingDays WHERE DoctorID = @DoctorID;");

            // ج. بناء أوامر إدخال التخصصات الجديدة ديناميكياً بمعاملات مرقمة
            if (SelectedSpecialtyIDs != null && SelectedSpecialtyIDs.Count > 0)
            {
                for (int i = 0; i < SelectedSpecialtyIDs.Count; i++)
                {
                    queryBuilder.AppendLine($"INSERT INTO DoctorSpecialties (DoctorID, SpecializationID) VALUES (@DoctorID, @SpecID{i});");
                }
            }

            // د. بناء أوامر إدخال أيام العمل الجديدة ديناميكياً بمعاملات مرقمة
            if (SelectedDayIDs != null && SelectedDayIDs.Count > 0)
            {
                for (int i = 0; i < SelectedDayIDs.Count; i++)
                {
                    queryBuilder.AppendLine($"INSERT INTO DoctorWorkingDays (DoctorID, DayID) VALUES (@DoctorID, @DayID{i});");
                }
            }

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                // بدء الـ Transaction لضمان (الكل أو لا شيء)
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                    {
                        // تمرير المعاملات المركزية للطبيب
                        cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                        cmd.Parameters.AddWithValue("@PersonID", PersonID);
                        cmd.Parameters.AddWithValue("@ConsultationFees", ConsultationFees);
                        cmd.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                        cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        cmd.Parameters.AddWithValue("@IsActive", IsActive);

                        // تمرير قيم معاملات التخصصات بأمان
                        if (SelectedSpecialtyIDs != null)
                        {
                            for (int i = 0; i < SelectedSpecialtyIDs.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@SpecID{i}", SelectedSpecialtyIDs[i]);
                            }
                        }

                        // تمرير قيم معاملات الأيام بأمان
                        if (SelectedDayIDs != null)
                        {
                            for (int i = 0; i < SelectedDayIDs.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@DayID{i}", SelectedDayIDs[i]);
                            }
                        }

                        try
                        {
                            // تنفيذ السكريبت الكامل بضربة واحدة عبر الشبكة (تحديث + حذف قطاعي + إدخال مجمع)
                            cmd.ExecuteNonQuery();

                            transaction.Commit(); // اعتماد التعديلات بالكامل للجداول الثلاثة معاً
                            return true;
                        }
                        catch (Exception)
                        {
                            // لو انهار أي سطر أو قيد، يتراجع السيرفر فوراً عن المسح والتعديل ويعود لحالته الآمنة المستقرة
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
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
            catch (Exception)
            {
                return false;
            }
        }

    }

    }