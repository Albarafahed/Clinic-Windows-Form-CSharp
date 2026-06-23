using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsSystemSettingsData
    {
        public static decimal GetTaxRateFromSettings()
        {
            decimal taxRate = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    // افترضنا أن الجدول يحتوي على صف واحد فقط للإعدادات
                    string query = "SELECT TOP 1 TaxPercentage FROM SystemSettings";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            // تحويل النسبة المئوية (مثلاً 15) إلى كسر عشري (0.15)
                            taxRate = Convert.ToDecimal(result) / 100;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex,clsGlobalLogger.LogLevel.Error);
                taxRate = 0;
            }
            return taxRate;
            
        }
    }
}
