using Clinic_DataAccess.SaveSqlException;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsBloodTypeData
    {
        public static bool FindBloodTypeByID(int BloodTypeID, ref string BloodTypeName)
        {
            bool Isfound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT BloodTypeName
                                FROM BloodTypes WHERE
                                BloodTypeID = @BloodTypeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BloodTypeID", BloodTypeID);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            BloodTypeName = result.ToString();
                            Isfound = true;
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
                Isfound = false;
            }

            return Isfound;
        }

        public static bool FindBloodTypeByName(string BloodTypeName, ref int BloodTypeID)
        {
            bool Isfound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT BloodTypeID
                                FROM BloodTypes WHERE
                                BloodTypeName = @BloodTypeName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BloodTypeName", BloodTypeName);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int parsedValue))
                        {
                            BloodTypeID = parsedValue;
                            Isfound = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
                Isfound = false;
            }
            return Isfound;
        }

        public static DataTable GetAllBloodTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM BloodTypes";
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
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);

            }
            return dt;
        }
    }
}