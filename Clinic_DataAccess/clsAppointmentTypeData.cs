using Clinic_DataAccess.SaveSqlException;
using Clinic_DataAccess.SaveExeption;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsAppointmentTypeData
    {
        public static DataTable GetAllAppintmentType()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT AppointmentTypeID, TypeName, DefaultFees FROM AppointmentTypes";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
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
        public static bool GetAppointmentTypeByID(int AppointmentTypeID, ref string TypeName, ref float DefaultFees)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT TypeName, DefaultFees FROM AppointmentTypes WHERE AppointmentTypeID = @AppointmentTypeID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TypeName = reader["TypeName"].ToString();
                                DefaultFees = Convert.ToSingle(reader["DefaultFees"]);
                                return true;
                            }
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
        public static int AddAppointmentType(string TypeName, float DefaultFees)
        {

            int AppointmentTypeID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO AppointmentTypes 
                            (TypeName, DefaultFees) 
                                VALUES (@TypeName, @DefaultFees);
                                    SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TypeName", TypeName);
                        cmd.Parameters.AddWithValue("@DefaultFees", DefaultFees);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int newID))
                        {
                            AppointmentTypeID = newID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
            }


            return AppointmentTypeID;
        }
        public static bool UpdateAppointmentType(int AppointmentTypeID, string TypeName, float DefaultFees)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE AppointmentTypes
                            SET TypeName = @TypeName,
                            DefaultFees = @DefaultFees
                            WHERE AppointmentTypeID = @AppointmentTypeID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);
                        cmd.Parameters.AddWithValue("@TypeName", TypeName);
                        cmd.Parameters.AddWithValue("@DefaultFees", DefaultFees);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
                isUpdated = false;
            }


            return isUpdated;
        }
        public static bool DeleteAppointmentType(int AppointmentTypeID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"DELETE FROM AppointmentTypes
                    WHERE AppointmentTypeID = @AppointmentTypeID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
                isDeleted = false;
            }


            return isDeleted;

        }

    }
}
