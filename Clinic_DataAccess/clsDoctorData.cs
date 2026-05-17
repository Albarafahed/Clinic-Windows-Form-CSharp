using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsDoctorData
    {

        public static bool AddDoctorSpecialties(int DoctorID, List<int> selectedSpecialtyIDs, bool deleteExistingFirst = false)
        {

            if (selectedSpecialtyIDs == null || selectedSpecialtyIDs.Count == 0)
            {
                return false; // لا توجد تخصصات لإضافتها
            }


            StringBuilder queryBuilder = new StringBuilder();
            if (deleteExistingFirst)
            {
                queryBuilder.AppendLine("DELETE FROM DoctorSpecialties WHERE DoctorID = @DoctorID;"); // حذف التخصصات القديمة
            }
            for (int i = 0; i < selectedSpecialtyIDs.Count; i++)
            {
                queryBuilder.AppendLine($"INSERT INTO DoctorSpecialties (DoctorID, SpecializationID) VALUES (@DoctorID, @SpecID{i});");
            }
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                // بدء الـ Transaction لضمان الذرية (إما نجاح الكل أو تراجع الكل)
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                    {
                        // إضافة المعامل الرئيسي لرقم الطبيب
                        cmd.Parameters.AddWithValue("@DoctorID", DoctorID);

                        // 3. تمرير قيم المعاملات المرقمة بأمان تآم
                        if (selectedSpecialtyIDs != null)
                        {
                            for (int i = 0; i < selectedSpecialtyIDs.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@SpecID{i}", selectedSpecialtyIDs[i]);
                            }
                        }

                        try
                        {
                            // هنا السحر: يطير النص الكامل (حذف + كل الإدخالات) في رسالة واحدة عبر الشبكة
                            cmd.ExecuteNonQuery();

                            transaction.Commit(); // اعتماد التغييرات فوراً
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback(); // تراجع آمن في حال حدوث أي خطأ
                            return false;
                        }

                    }
                }
            }
        }
        public static bool UpdateSpecialtiesInlineUltraFast(int doctorID, List<int> selectedSpecialtyIDs)
        {
            return AddDoctorSpecialties(doctorID, selectedSpecialtyIDs, true);
        }

        public static bool DeleteDoctorSpecialties(int doctorID)
        {
            bool IsDeleted = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"DELETE FROM DoctorSpecialties WHERE DoctorID = @DoctorID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorID", doctorID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
            }
            finally
            {
                connection.Close();
            }
            return IsDeleted;
        }

        public static bool AddDoctorWorkingDays(int DoctorID, List<int> selectedDayIDs, bool deleteExistingFirst = false)
        {
            if (selectedDayIDs == null || selectedDayIDs.Count == 0)
            {
                return false; // لا توجد أيام عمل لإضافتها
            }
            StringBuilder queryBuilder = new StringBuilder();
            if (deleteExistingFirst)
            {
                queryBuilder.AppendLine("DELETE FROM DoctorWorkingDays WHERE DoctorID = @DoctorID;"); // حذف أيام العمل القديمة
            }
            for (int i = 0; i < selectedDayIDs.Count; i++)
            {
                queryBuilder.AppendLine($"INSERT INTO DoctorWorkingDays (DoctorID, DayID) VALUES (@DoctorID, @DayID{i});");
            }
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                        if (selectedDayIDs != null)
                        {
                            for (int i = 0; i < selectedDayIDs.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@DayID{i}", selectedDayIDs[i]);
                            }
                        }
                        try
                        {
                            cmd.ExecuteNonQuery();
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
        }

        public static bool UpdateDoctorWorkingDaysInlineUltraFast(int doctorID, List<int> selectedDayIDs)

        {
            return AddDoctorWorkingDays(doctorID, selectedDayIDs, true);
        }

        public static bool DeleteDoctorWorkingDays(int doctorID)
        {
            bool IsDeleted = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"DELETE FROM DoctorWorkingDays WHERE DoctorID = @DoctorID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorID", doctorID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
            }
            finally
            {
                connection.Close();
            }
            return IsDeleted;
        }

        public static int AddNewDoctor(int PersonID, float ConsultationFees, List<int> SelectedDayIDs, List<int> SelectedSpecialtyIDs)
        {
            // 1. بناء نص الاستعلام المجمع
            StringBuilder queryBuilder = new StringBuilder();

            // سكريبت إدخال الطبيب الأساسي وتخزين معرفه
            queryBuilder.AppendLine(@"
        DECLARE @NewDoctorID INT;
        INSERT INTO Doctors (PersonID, ConsultationFees) 
        VALUES (@PersonID, @ConsultationFees); 
        SET @NewDoctorID = SCOPE_IDENTITY();
    ");

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

        public static bool UpdateDoctorWithSpecialtiesAndDaysInline(int DoctorID, float ConsultationFees, List<int> SelectedDayIDs, List<int> SelectedSpecialtyIDs)
        {
            // 1. بناء نص الاستعلام المجمع لتحديث البيانات والمسح وإعادة الإدخال
            StringBuilder queryBuilder = new StringBuilder();

            // أ. تحديث سعر الكشفية في جدول الأطباء الأساسي
            queryBuilder.AppendLine("UPDATE Doctors SET ConsultationFees = @ConsultationFees WHERE DoctorID = @DoctorID;");

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
                        cmd.Parameters.AddWithValue("@ConsultationFees", ConsultationFees);

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

       
    }
    }