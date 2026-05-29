using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsBloodTypeData
    {
        public static bool FindBloodTypeByID(int BloodTypeID, ref string BloodTypeName)
        {
            bool Isfound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT BloodTypeName FROM BloodTypes WHERE BloodTypeID = @BloodTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BloodTypeID", BloodTypeID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    BloodTypeName = result.ToString();
                    Isfound = true;
                }
            }
            catch
            {
                Isfound = false;
            }
            finally
            {
                connection.Close();
            }
            return Isfound;
        }

        public static bool FindBloodTypeByName(string BloodTypeName, ref int BloodTypeID)
        {
            bool Isfound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT BloodTypeID FROM BloodTypes WHERE BloodTypeName = @BloodTypeName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BloodTypeName", BloodTypeName);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int parsedValue))
                {
                    BloodTypeID = parsedValue;
                    Isfound = true;
                }
            }
            catch
            {
                Isfound = false;
            }
            finally
            {
                connection.Close();
            }
            return Isfound;
        }

        public static DataTable GetAllBloodTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM BloodTypes";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch
            {
                // 💡 ترك الجدول فارغاً بدلاً من إرجاع null يمنع انهيار الواجهات (UI Crashes)
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
    }
}