using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsServicesTypeData
    {
        public static int AddNewService(string ServiceName, string Description, float ServiceFees)
        {
            int ServiceID = -1;
            string query = @"INSERT INTO Services 
                    (ServiceName, Description, ServiceFees)
         VALUES (@ServiceName, @Description, @ServiceFees);
                        SELECT SCOPE_IDENTITY();";
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceName", ServiceName);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@ServiceFees", ServiceFees);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            ServiceID = id;
                        }
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                   
                }
            }

            return ServiceID;
        }
        public static bool UpdateService(int ServiceID, string ServiceName, string Description, float ServiceFees)
        {
           int rowsAffected = 0;
            string query =@"UPDATE Services
                        SET ServiceName = @ServiceName,
                            Description = @Description,
                            ServiceFees = @ServiceFees
                             WHERE ServiceID = @ServiceID";
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                    cmd.Parameters.AddWithValue("@ServiceName", ServiceName);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@ServiceFees", ServiceFees);
                    try
                    {
                        conn.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }
            }
            return rowsAffected > 0;
        }
        public static bool GetServiceByID(int ServiceID, ref string ServiceName, ref string Description, ref float ServiceFees)
        {
          string query = @"SELECT ServiceName, Description,
                            ServiceFees FROM Services
                                WHERE ServiceID = @ServiceID";
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ServiceName = reader["ServiceName"].ToString();
                                Description = reader["Description"].ToString();
                                ServiceFees = Convert.ToSingle(reader["ServiceFees"]);
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static DataTable GetServiceList()
        {
            DataTable serviceList = new DataTable();
            string query = @"SELECT ServiceID, ServiceName, 
                            Description, ServiceFees
                                FROM Services";
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.HasRows)
                            serviceList.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return serviceList;
        }
    }
}
