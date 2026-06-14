using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsDiscountData
    {
        // 1. جلب الحد الأقصى للخصم بناءً على دور المستخدم
        public static decimal GetMaxDiscountLimitByRole(int roleID)
        {
            decimal maxLimit = 0;
            string query = "SELECT MaxDiscountLimit FROM RoleDiscountLimits WHERE RoleID = @RoleID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoleID", roleID);
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        maxLimit = Convert.ToDecimal(result);
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                maxLimit = 0;
            }
            return maxLimit;
        }

        // 2. جلب الحد الأقصى للخصم بناءً على نوع الخدمة (TargetType)
        public static decimal GetMaxDiscountPolicy(byte categoryID)
        {
            decimal maxPolicy = 0;
            string query = "SELECT MaxPercentage FROM DiscountPolicies WHERE TargetType = @CategoryID AND IsActive = 1";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        maxPolicy = Convert.ToDecimal(result);
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                maxPolicy = 0;
            }
            return maxPolicy;
        }
    }
}