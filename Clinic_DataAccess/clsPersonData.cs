using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonByPersonID(int PersonID, ref string FullName, ref DateTime DateOfBirth, ref short Gendor, ref string PhoneNumber,ref string Email,
                                               ref string Address, ref int NationalityCountryID, ref string ImagePath)
        {
            bool Isfound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * From Persons WHERE PersonID = @PersonID and IsDeleted = 0";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    FullName = reader["FullName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = Convert.ToInt16(reader["Gendor"]);
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
                reader.Close();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                Isfound = false;
            }
            finally
            {
                connection.Close();
            }

            return Isfound;
        }

        public static int AddNewPerson(string FullName, DateTime DateOfBirth, short Gendor, string PhoneNumber, string Email,
                                               string Address, string ImagePath, int NationalityCountryID)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Persons (FullName, DateOfBirth, Gendor, PhoneNumber, Email, Address, ImagePath, NationalityCountryID)
                             VALUES (@FullName, @DateOfBirth, @Gendor, @PhoneNumber, @Email, @Address, @ImagePath, @NationalityCountryID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", FullName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    PersonID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                PersonID = -1;
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string FullName, DateTime DateOfBirth, short Gendor, string PhoneNumber, string Email,
                                               string Address, string ImagePath, int NationalityCountryID)
        {
            bool IsUpdated = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE Persons SET FullName = @FullName, DateOfBirth = @DateOfBirth, Gendor = @Gendor,
                             PhoneNumber = @PhoneNumber, Email = @Email, Address = @Address, ImagePath = @ImagePath,
                             NationalityCountryID = @NationalityCountryID WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FullName", FullName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                IsUpdated = false;
            }
            finally
            {
                connection.Close();
            }
            return IsUpdated;
        }
        public static bool DeletePerson(int PersonID)
        {
            bool IsDeleted = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"DELETE FROM Persons WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                IsDeleted = false;
            }
            finally
            {
                connection.Close();
            }
            return IsDeleted;
        }

        public static DataTable GetAllPersons()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT Persons.PersonID, Persons.FullName, Persons.DateOfBirth, 
               GenderCaption = CASE Persons.Gendor WHEN 0 THEN 'Male' ELSE 'Female' END,
               Persons.PhoneNumber, Persons.Email, Persons.Address
                    , Countries.CountryName FROM Persons
                    INNER JOIN Countries ON Persons.NationalityCountryID = Countries.CountryID
                    WHERE Persons.IsDeleted = 0";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                dt = null;
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static DataRow GetPerson(int PersonID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT Persons.PersonID, Persons.FullName, Persons.DateOfBirth, 
               GenderCaption = CASE Persons.Gendor WHEN 0 THEN 'Male' ELSE 'Female' END,
               Persons.PhoneNumber, Persons.Email, Persons.Address
                    , Countries.CountryName FROM Persons
                    INNER JOIN Countries ON Persons.NationalityCountryID = Countries.CountryID
                        Where Persons.PersonID=@PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                dt = null;
            }
            finally
            {
                connection.Close();
            }
            return dt.Rows[0];
        }

        public static bool SoftDeletePerson(int PersonID)
        {
            bool IsDeleted = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE Persons SET IsDeleted = 1 WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                IsDeleted = false;
            }
            finally
            {
                connection.Close();
            }
            return IsDeleted;
        }

        public static bool RestorePerson(int PersonID)
        {
            bool IsRestored = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE Persons SET IsDeleted = 0 WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsRestored = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                IsRestored = false;
            }
            finally
            {
                connection.Close();
            }
            return IsRestored;
        }
        }
    }
