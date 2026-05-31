using Clinic_DataAccess.SaveException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsSpecializationData
    {      
        public static bool GetSpecializationByID(int SpecializationID, ref string SpecializationName)
        {
            bool Isfound = false;

            SqlConnection connection=new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT SpecializationName
                        FROM Specializations WHERE SpecializationID=@SpecializationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SpecializationID", SpecializationID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    SpecializationName = result.ToString();
                    Isfound = true;
                }

            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                Isfound = false;
            }
            finally
            {
                connection.Close();
            }
            return Isfound;
        }

        public static bool GetSpecializationByName(string SpecializationName, ref int SpecializationID)
        {
            bool Isfound = false;
            SqlConnection connection=new SqlConnection( clsDataAccessSettings.ConnectionString);
            string query = @"SELECT SpecializationID
                        FROM Specializations WHERE SpecializationName=@SpecializationName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SpecializationName", SpecializationName);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    SpecializationID = Convert.ToInt32(result);
                    Isfound = true;
                }
            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                Isfound = false;
            }
            finally
            {
                connection.Close();
            }
            return Isfound;
        }

        public static DataTable GetAllSpecializations()
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT *
                        FROM Specializations";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                 if(reader.HasRows)
                {
                    table.Load(reader);
                }
            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
            }
            finally
            {
                connection.Close();
            }
            return table;
        }

        public static int AddNewSpecialization(string SpecializationName)
        {
             int SpecializationID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Specializations (SpecializationName) 
                VALUES (@SpecializationName);
                    Select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SpecializationName", SpecializationName);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    SpecializationID = id;
                }
            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                SpecializationID = -1;
            }
            finally
            {
                connection.Close();
            }
            return SpecializationID;
        }

        public static bool UpdateSpecialization(int SpecializationID, string SpecializationName)
        {
            bool IsUpdated = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE Specializations 
                        SET SpecializationName=@SpecializationName
                        WHERE SpecializationID=@SpecializationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SpecializationID", SpecializationID);
            command.Parameters.AddWithValue("@SpecializationName", SpecializationName);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                IsUpdated = false;
            }
            finally
            {
                connection.Close();
            }
            return IsUpdated;
        }

        public static bool DeleteSpecialization(int SpecializationID)
        {
            bool IsDeleted = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"DELETE FROM Specializations 
                        WHERE SpecializationID=@SpecializationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SpecializationID", SpecializationID);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                clsGlobalLogger.LogException(ex, clsGlobalLogger.LogLevel.Error);
                IsDeleted = false;
            }
            finally
            {
                connection.Close();
            }
            return IsDeleted;
        }
        }
}
