using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonByPersonID(int PersonID, ref string FullName, ref DateTime DateOfBirth, ref short Gender, ref string PhoneNumber, ref string Email,
                                               ref string Address, ref int NationalityCountryID, ref string ImagePath)
        {
            bool Isfound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * From Persons
                                WHERE PersonID = @PersonID 
                                    and IsDeleted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FullName = reader["FullName"].ToString();
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                                Gender = Convert.ToInt16(reader["Gender"]);
                                PhoneNumber = reader["PhoneNumber"].ToString();
                                if (reader["Email"] != DBNull.Value)
                                    Email = reader["Email"].ToString();
                                else
                                    Email = string.Empty;
                                Address = reader["Address"].ToString();
                                if (reader["ImagePath"] != DBNull.Value)
                                    ImagePath = reader["ImagePath"].ToString();
                                else
                                    ImagePath = string.Empty;
                                NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                                Isfound = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

                Isfound = false;
            }

            return Isfound;
        }

        public static int AddNewPerson(string FullName, DateTime DateOfBirth, short Gender, string PhoneNumber, string Email,
                                               string Address, string ImagePath, int NationalityCountryID)
        {
            int PersonID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Persons (FullName, DateOfBirth, Gender, PhoneNumber, Email, Address, ImagePath, NationalityCountryID)
                             VALUES (@FullName, @DateOfBirth, @Gender, @PhoneNumber, @Email, @Address, @ImagePath, @NationalityCountryID);
                             SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@FullName", FullName);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        command.Parameters.AddWithValue("@Gender", Gender);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        command.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int newPersonID))
                        {
                            PersonID = newPersonID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

                PersonID = -1;
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string FullName, DateTime DateOfBirth, short Gender, string PhoneNumber, string Email,
                                               string Address, string ImagePath, int NationalityCountryID)
        {
            bool IsUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Persons SET FullName = @FullName, DateOfBirth = @DateOfBirth, Gender = @Gender,
                             PhoneNumber = @PhoneNumber, Email = @Email, Address = @Address, ImagePath = @ImagePath,
                             NationalityCountryID = @NationalityCountryID WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@FullName", FullName);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        command.Parameters.AddWithValue("@Gender", Gender);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        command.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        IsUpdated = rowsAffected > 0;
                    }
                }
            }

            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

                IsUpdated = false;
            }

            return IsUpdated;
        }
        public static bool DeletePerson(int PersonID)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"DELETE FROM Persons WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        IsDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsDeleted = false;
            }
            return IsDeleted;
        }
        public static bool DeleteAllForTrash()
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"DELETE FROM Persons WHERE IsDeleted = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        IsDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                IsDeleted = false;
            }
            return IsDeleted;
        }

        public static DataTable GetAllPersons()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_PersonDetails;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
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

        public static DataTable GetAllPersonsTrash()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_PersonTrashDetails;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
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

        public static DataRow GetPersonByID(int PersonID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_PersonDetails
                        Where PersonID=@PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

            }

            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;

        }

        public static bool SoftDeletePerson(int PersonID)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Persons
                                SET IsDeleted = 1 
                                WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        IsDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

                IsDeleted = false;
            }
            return IsDeleted;
        }

        public static bool RestorePerson(int PersonID)
        {
            bool IsRestored = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Persons
                     SET IsDeleted = 0 
                     WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        IsRestored = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

                IsRestored = false;
            }
            return IsRestored;
        }
    }
}
