using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsUserData
    {
        public static bool Find(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive, ref int RoleID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT UserID, PersonID,
                                    UserName, Password,
                                    IsActive, RoleID 
                                    FROM Users 
                                    WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                PersonID = (int)reader["PersonID"];
                                UserName = reader["UserName"].ToString();
                                Password = reader["Password"].ToString();
                                IsActive = (bool)reader["IsActive"];
                                RoleID = (int)reader["RoleID"];
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsFound = false;
            }
            return IsFound;
        }

        public static bool FindByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive, ref int RoleID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT UserID, PersonID,
                                    UserName, Password,
                                    IsActive, RoleID 
                                    FROM Users 
                                    WHERE PersonID = @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                UserID = (int)reader["UserID"];
                                UserName = reader["UserName"].ToString();
                                Password = reader["Password"].ToString();
                                IsActive = (bool)reader["IsActive"];
                                RoleID = (int)reader["RoleID"];
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsFound = false;
            }
            return IsFound;
        }

        public static bool FindByUsernameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive, ref int RoleID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT UserID, PersonID,
                                    IsActive, RoleID 
                                FROM Users WHERE UserName = @UserName
                                    AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                UserID = (int)reader["UserID"];
                                PersonID = (int)reader["PersonID"];
                                IsActive = (bool)reader["IsActive"];
                                RoleID = (int)reader["RoleID"];
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsFound = false;
            }
            return IsFound;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive, int RoleID)
        {
            int UserID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive, RoleID)
                                 VALUES (@PersonID, @UserName, @Password, @IsActive, @RoleID);
                                 SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@RoleID", RoleID);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            UserID = insertedID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                UserID = -1;
            }
            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive, int RoleID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Users 
                                 SET PersonID = @PersonID, 
                                     UserName = @UserName, 
                                     Password = @Password, 
                                     IsActive = @IsActive, 
                                     RoleID = @RoleID 
                                 WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@RoleID", RoleID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
            return rowsAffected > 0;
        }

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Users WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
            return rowsAffected > 0;
        }

        public static bool DeleteUserByPersonID(int PersonID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Users WHERE PersonID = @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
            return rowsAffected > 0;
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_UserDetails;";

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
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return dt;
        }

        public static DataRow GetUserByID(int UserID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_UserDetails
                                 WHERE UserID=@UserID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
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
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
            }
            return dt.Rows.Count > 0 && dt != null ? dt.Rows[0] : null;
        }

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT TOP 1 1 FROM Users WHERE UserName = @UserName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            isFound = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                isFound = false;
            }

            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT TOP 1 1 FROM Users WHERE PersonID = @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            isFound = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                isFound = false;
            }
            return isFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Users 
                                 SET Password = @Password
                                 WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Password", NewPassword);

                        {
                            connection.Open();
                            rowsAffected = command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
            return (rowsAffected > 0);
        }

        public static string GetFullName(int UserID)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT P.FullName 
                                   FROM Persons P 
                                   INNER JOIN Users U ON
                                    P.PersonID = U.PersonID 
                                   WHERE U.UserID = @UserID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);


                        connection.Open();

                        object result = command.ExecuteScalar();

                        return (result != null) ? result.ToString() : "";
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return "";
            }
        }

    }
}