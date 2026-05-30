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
            string query = "SELECT AppointmentTypeID, TypeName, DefaultFees FROM AppointmentTypes";
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it)
                        throw new ApplicationException("An error occurred while fetching appointment types.", ex);
                    }
                }
            }


            return dt;
        }

        public static bool GetAppointmentTypeByID(int AppointmentTypeID, ref string TypeName, ref float DefaultFees)
        {
            bool isFound = false;
            string query = "SELECT TypeName, DefaultFees FROM AppointmentTypes WHERE AppointmentTypeID = @AppointmentTypeID";

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TypeName = reader["TypeName"].ToString();
                                DefaultFees = Convert.ToSingle(reader["DefaultFees"]);
                                isFound = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it)
                        throw new ApplicationException("An error occurred while fetching appointment type.", ex);
                    }
                }
            }

            return isFound;
        }

        public static int AddAppointmentType(string TypeName, float DefaultFees)
        {
            int newID = -1;
            string query =@"INSERT INTO AppointmentTypes 
                            (TypeName, DefaultFees) 
                                VALUES (@TypeName, @DefaultFees)";
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TypeName", TypeName);
                    cmd.Parameters.AddWithValue("@DefaultFees", DefaultFees);
                    try
                    {
                        conn.Open();
                        newID = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it)
                        throw new ApplicationException("An error occurred while adding appointment type.", ex);
                    }
                }
            }
            return newID;
        }
        public static bool UpdateAppointmentType(int AppointmentTypeID, string TypeName, float DefaultFees)
        {
            bool isUpdated = false;
            string query = "UPDATE AppointmentTypes SET TypeName = @TypeName, DefaultFees = @DefaultFees WHERE AppointmentTypeID = @AppointmentTypeID";
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentTypeID", AppointmentTypeID);
                    cmd.Parameters.AddWithValue("@TypeName", TypeName);
                    cmd.Parameters.AddWithValue("@DefaultFees", DefaultFees);
                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it)
                        throw new ApplicationException("An error occurred while updating appointment type.", ex);
                    }
                }
            }
            return isUpdated;
        }

    }

}
