using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsPatinetData
    {
        public static bool GetPatientByPatientID(int PatientID, ref int PersonID, ref string EmergencyContact, ref int BloodTypeID, ref string MedicalHistory)
        {
            bool Isfound = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT Patients.* FROM Patients 
                             INNER JOIN Persons ON Patients.PersonID = Persons.PersonID 
                             WHERE Patients.PatientID = @PatientID AND Persons.IsDeleted = 0";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Isfound = true;
                    PersonID = Convert.ToInt32(reader["PersonID"]);
                    EmergencyContact = reader["EmergencyContact"] != DBNull.Value ? reader["EmergencyContact"].ToString() : string.Empty;
                    BloodTypeID = Convert.ToInt32(reader["BloodTypeID"]);
                    MedicalHistory = reader["MedicalHistory"] != DBNull.Value ? reader["MedicalHistory"].ToString() : string.Empty;
                }
                reader.Close();
            }
            catch (Exception)
            {
                Isfound = false;
            }
            finally
            {
                conn.Close();
            }
            return Isfound;
        }

        public static int AddNewPatient(int PersonID, string EmergencyContact, int BloodTypeID, string MedicalHistory)
        {
            int PatientID = -1;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Patients (PersonID, EmergencyContact, BloodTypeID, MedicalHistory)
                             VALUES (@PersonID, @EmergencyContact, @BloodTypeID, @MedicalHistory);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@EmergencyContact", EmergencyContact ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BloodTypeID", BloodTypeID);
            cmd.Parameters.AddWithValue("@MedicalHistory", MedicalHistory ?? (object)DBNull.Value);
            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int patientId))
                {
                    PatientID = patientId;
                }
            }
            catch (Exception)
            {
                PatientID = -1;
            }
            finally
            {
                conn.Close();
            }
            return PatientID;
        }

        public static bool UpdatePatient(int PatientID, int PersonID, string EmergencyContact, int BloodTypeID, string MedicalHistory)
        {
            bool IsUpdated = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE Patients SET PersonID = @PersonID, EmergencyContact = @EmergencyContact,
                             BloodTypeID = @BloodTypeID, MedicalHistory = @MedicalHistory WHERE PatientID = @PatientID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", PatientID);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@EmergencyContact", EmergencyContact ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BloodTypeID", BloodTypeID);
            cmd.Parameters.AddWithValue("@MedicalHistory", MedicalHistory ?? (object)DBNull.Value);
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                IsUpdated = rowsAffected > 0;
            }
            catch (Exception)
            {
                IsUpdated = false;
            }
            finally
            {
                conn.Close();
            }
            return IsUpdated;
        }

        public static bool DeletePatient(int PatientID)
        {
            bool IsDeleted = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "DELETE FROM Patients WHERE PatientID = @PatientID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
            catch (Exception)
            {
                IsDeleted = false;
            }
            finally
            {
                conn.Close();
            }
            return IsDeleted;
        }

        public static DataTable GetAllPationtes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // تم تعديل الشرط ليفحص حقل الإخفاء في جدول الأشخاص الأساسي لمنع مشاكل الـ Syntax
            string query = @"SELECT Patients.PatientID, Persons.PersonID, Persons.FullName,
                             Patients.MedicalHistory, Countries.CountryName,
                             BloodTypes.BloodTypeName, Patients.EmergencyContact
                             FROM Persons 
                             INNER JOIN Patients ON Persons.PersonID = Patients.PersonID
                             INNER JOIN Countries ON Persons.NationalityCountryID = Countries.CountryID
                             INNER JOIN BloodTypes ON Patients.BloodTypeID = BloodTypes.BloodTypeID
                             WHERE Persons.IsDeleted = 0
                             ORDER BY Persons.PersonID;";

            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception) { }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static DataRow GetPationte(int PatientID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT Patients.PatientID, Persons.PersonID, Persons.FullName,
                             Patients.MedicalHistory, Countries.CountryName,
                             BloodTypes.BloodTypeName, Patients.EmergencyContact
                             FROM Persons 
                             INNER JOIN Patients ON Persons.PersonID = Patients.PersonID
                             INNER JOIN Countries ON Persons.NationalityCountryID = Countries.CountryID
                             INNER JOIN BloodTypes ON Patients.BloodTypeID = BloodTypes.BloodTypeID
                             WHERE Patients.PatientID = @PatientID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@PatientID", PatientID);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception) { }
            finally
            {
                connection.Close();
            }

            // حماية صارمة: التأكد من وجود أسطر فعلياً قبل إرجاع الصف الأول لمنع الانهيار المفاجئ
            return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }
    }
}