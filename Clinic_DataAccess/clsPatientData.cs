using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsPatientData
    {
        public static bool GetPatientByPatientID(int PatientID, ref int PersonID, ref string EmergencyContact, ref int BloodTypeID, ref string MedicalHistory, ref DateTime CreatedDate, ref int CreatedByUserID)
        {
            // إضافة الفاصلة المفقودة بين الأعمدة
            string query = @"SELECT Patients.PersonID, Patients.EmergencyContact, Patients.BloodTypeID, 
                            Patients.MedicalHistory, Patients.CreatedDate, Patients.CreatedByUserID 
                     FROM Patients 
                     INNER JOIN Persons ON Patients.PersonID = Persons.PersonID 
                     WHERE Patients.PatientID = @PatientID AND Persons.IsDeleted = 0";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PersonID = (int)reader["PersonID"];
                            EmergencyContact = reader["EmergencyContact"] != DBNull.Value ? (string)reader["EmergencyContact"] : string.Empty;
                            BloodTypeID = reader["BloodTypeID"] != DBNull.Value ? (int)reader["BloodTypeID"] : -1;
                            MedicalHistory = reader["MedicalHistory"] != DBNull.Value ? (string)reader["MedicalHistory"] : string.Empty;
                            // تحويل آمن للتاريخ
                            CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.Now;
                            CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int)reader["CreatedByUserID"] : -1;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex) { /* يفضل تسجيل الخطأ: log.Error(ex.Message); */ }
            return false;
        }

        public static int AddNewPatient(int PersonID, string EmergencyContact, int BloodTypeID, string MedicalHistory,DateTime CreatedDate, int CreatedByUserID)
        {
            string query = @"INSERT INTO Patients (PersonID, EmergencyContact, BloodTypeID, MedicalHistory,CreatedDate, CreatedByUserID)
                             VALUES (@PersonID, @EmergencyContact, @BloodTypeID, @MedicalHistory,@CreatedDate, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@EmergencyContact", string.IsNullOrEmpty(EmergencyContact) ? (object)DBNull.Value : EmergencyContact);
                    cmd.Parameters.AddWithValue("@BloodTypeID", BloodTypeID <= 0 ? (object)DBNull.Value : BloodTypeID);
                    cmd.Parameters.AddWithValue("@MedicalHistory", string.IsNullOrEmpty(MedicalHistory) ? (object)DBNull.Value : MedicalHistory);
                    cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    // تحويل وإرجاع مباشر بدون حجز متغير مؤقت
                    if (result != null) return Convert.ToInt32(result);
                }
            }
            catch (Exception) { }

            return -1;
        }

        public static bool UpdatePatient(int PatientID, int PersonID, string EmergencyContact, int BloodTypeID, string MedicalHistory, int CreatedByUserID)
        {
            string query = @"UPDATE Patients SET PersonID = @PersonID, EmergencyContact = @EmergencyContact,
                             BloodTypeID = @BloodTypeID, MedicalHistory = @MedicalHistory, CreatedByUserID = @CreatedByUserID 
                             WHERE PatientID = @PatientID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@EmergencyContact", string.IsNullOrEmpty(EmergencyContact) ? (object)DBNull.Value : EmergencyContact);
                    cmd.Parameters.AddWithValue("@BloodTypeID", BloodTypeID <= 0 ? (object)DBNull.Value : BloodTypeID);
                    cmd.Parameters.AddWithValue("@MedicalHistory", string.IsNullOrEmpty(MedicalHistory) ? (object)DBNull.Value : MedicalHistory);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    conn.Open();
                    // الاختصار المستهدف: خطوة واحدة للكومبايلر
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeletePatient(int PatientID)
        {
            string query = "DELETE FROM Patients WHERE PatientID = @PatientID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);
                    conn.Open();

                    // خطوة واحدة مدمجة للـ Delete أيضاً
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static DataTable GetAllPatients()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT Patients.PatientID, Persons.PersonID, Persons.FullName,
                             Patients.MedicalHistory,Patients.CreatedDate, Countries.CountryName,
                             BloodTypes.BloodTypeName, Patients.EmergencyContact
                             FROM Persons 
                             INNER JOIN Patients ON Persons.PersonID = Patients.PersonID
                             INNER JOIN Countries ON Persons.NationalityCountryID = Countries.CountryID
                             INNER JOIN BloodTypes ON Patients.BloodTypeID = BloodTypes.BloodTypeID
                             WHERE Persons.IsDeleted = 0
                             ORDER BY Persons.PersonID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) dt.Load(reader);
                    }
                }
            }
            catch (Exception) { }

            return dt;
        }

        public static DataRow GetPatientByID(int PatientID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT Patients.PatientID, Persons.PersonID, Persons.FullName,
                             Patients.MedicalHistory,Patients.CreatedDate, Countries.CountryName,
                             BloodTypes.BloodTypeName, Patients.EmergencyContact
                             FROM Persons 
                             INNER JOIN Patients ON Persons.PersonID = Patients.PersonID
                             INNER JOIN Countries ON Persons.NationalityCountryID = Countries.CountryID
                             INNER JOIN BloodTypes ON Patients.BloodTypeID = BloodTypes.BloodTypeID
                             WHERE Patients.PatientID = @PatientID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
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
            catch (Exception) { }

            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        public static bool IsPatientExistForPersonID(int PersonID)
        {
            // استخدام TOP 1 1 هي الطريقة الأكثر احترافية للتحقق من الوجود
            string query = "SELECT TOP 1 1 FROM Patients WHERE PersonID = @PersonID;";

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