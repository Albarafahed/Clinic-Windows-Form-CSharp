
using Clinic_DataAccess.SaveSqlException;
using System.Data;
using System.Data.SqlClient;


namespace Clinic_DataAccess
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT CountryName 
                            FROM Countries WHERE
                                CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", ID);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            // The record was found
                            isFound = true;
                            CountryName = result.ToString();
                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }
                    }
                }
            }


            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
                isFound = false;
            }

            return isFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT CountryID 
                            FROM Countries WHERE
                                CountryName = @CountryName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", CountryName);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int parsedValue))
                        {
                            // The record was found
                            isFound = true;
                            ID = parsedValue;
                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);

                isFound = false;
            }

            return isFound;
        }

        public static DataTable GetAllCountries()
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Countries order by CountryName";
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
