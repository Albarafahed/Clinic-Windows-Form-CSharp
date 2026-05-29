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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // تحسين: تحديد أسماء الأعمدة صراحة بدلاً من * لرفع الأداء
                string query = "SELECT UserID, PersonID, UserName, Password, IsActive, RoleID FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
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
                    catch (Exception)
                    {
                        IsFound = false;
                    }
                }
            }
            return IsFound;
        }

        public static bool FindByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive, ref int RoleID)
        {
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // تحسين: تحديد الأعمدة بدقة
                string query = "SELECT UserID, PersonID, UserName, Password, IsActive, RoleID FROM Users WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
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
                    catch (Exception)
                    {
                        IsFound = false;
                    }
                }
            }
            return IsFound;
        }

        public static bool FindByUsernameAndPassword(string UserName,string Password,ref int UserID, ref int PersonID, ref bool IsActive, ref int RoleID)
        {
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // تحسين: تحديد الأعمدة بدقة
                string query = "SELECT UserID, PersonID, IsActive, RoleID FROM Users WHERE UserName = @UserName AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);

                    try
                    {
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
                    catch (Exception)
                    {
                        IsFound = false;
                    }
                }
            }
            return IsFound;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive, int RoleID)
        {
            int UserID = -1;

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

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            UserID = insertedID;
                        }
                    }
                    catch (Exception)
                    {
                        UserID = -1;
                    }
                }
            }
            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive, int RoleID)
        {
            int rowsAffected = 0;

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

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return rowsAffected > 0;
        }

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return rowsAffected > 0;
        }

        public static bool DeleteUserByPersonID(int PersonID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Users WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return rowsAffected > 0;
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT U.UserID, P.PersonID, P.FullName, U.UserName, R.RoleName, U.IsActive 
                                 FROM Users U 
                                 INNER JOIN Persons P ON U.PersonID = P.PersonID
                                 INNER JOIN Roles R ON U.RoleID = R.RoleID
                                 WHERE P.IsDeleted = 0;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
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
                    catch (Exception)
                    {
                        // إرجاع جدول فارغ بسلام
                    }
                }
            }
            return dt;
        }

        public static DataRow GetUserByID(int UserID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT U.UserID, P.PersonID, P.FullName, U.UserName, R.RoleName, U.IsActive 
                                 FROM Users U 
                                 INNER JOIN Persons P ON U.PersonID = P.PersonID
                                 INNER JOIN Roles R ON U.RoleID = R.RoleID
                                 WHERE U.UserID=@UserID and P.IsDeleted = 0;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
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
                    catch (Exception)
                    {
                        // إرجاع جدول فارغ بسلام
                    }
                }
            }
            return dt.Rows[0];
        }

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT TOP 1 1 FROM Users WHERE UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            isFound = true;
                        }
                    }
                    catch (Exception)
                    {
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT TOP 1 1 FROM Users WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            isFound = true;
                        }
                    }
                    catch (Exception)
                    {
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Users 
                                 SET Password = @Password
                                 WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    // إصلاح: تم إضافة البارامتر المفقود لضمان عمل الدالة دون انهيار
                    command.Parameters.AddWithValue("@Password", NewPassword);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static string GetFullName(int UserID)
        {
            string query = @"SELECT P.FullName 
                     FROM Persons P 
                     INNER JOIN Users U ON P.PersonID = U.PersonID 
                     WHERE U.UserID = @UserID;";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                   
                    connection.Open();

                    // 2. تخزين النتيجة في كائن للتحقق منه قبل تحويله لنص
                    object result = command.ExecuteScalar();

                    // 3. التحقق بأمان: إذا كانت النتيجة ليست فارغة نعيد الاسم، وإلا نعيد نص فارغ
                    return (result != null) ? result.ToString() : "";
                }
            }
            catch (Exception)
            {
                // في حال حدوث أي خطأ في الاتصال، يعود بنص فارغ بأمان دون توقف البرنامج
                return "";
            }
        }

    }
}