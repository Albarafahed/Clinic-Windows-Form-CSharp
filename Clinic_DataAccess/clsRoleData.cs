using Clinic_DataAccess;
using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

public static class clsRoleData
{
    public static bool FindByName(string RoleName, ref int RoleID)
    {
        bool IsFound = false;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT RoleID FROM Roles WHERE RoleName = @RoleName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleName", RoleName);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            RoleID = (int)reader["RoleID"];
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

    public static bool FindByID(int RoleID, ref string RoleName)
    {
        bool IsFound = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT RoleName FROM Roles WHERE RoleID = @RoleID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", RoleID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : "";
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

    public static DataTable GetAllRoles()
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Roles ORDER BY RoleName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader); // شحن الـ DataTable برمجياً من الـ Reader بسرعة فائقة
                        }
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
}