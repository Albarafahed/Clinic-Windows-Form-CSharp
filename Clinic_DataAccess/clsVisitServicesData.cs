using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsVisitServicesData
    {

        public static DataTable GetVisitServices(int VisitID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT VS.ServiceID, S.ServiceName, VS.SavedServicePrice, VS.Quantity, VS.Discount 
                     FROM VisitServices VS
                     INNER JOIN Services S ON VS.ServiceID = S.ServiceID
                     WHERE VS.VisitID = @VisitID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
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


        public static bool SaveVisitServices(int VisitID, DataTable dtServices, SqlTransaction transaction)
        {

            //DeleteVisitServices(VisitID, transaction);
            if(!dtServices.Columns.Contains("VisitID"))
            dtServices.Columns.Add("VisitID", typeof(int), VisitID.ToString());

            // الإدراج الجماعي
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(transaction.Connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "VisitServices";
                bulkCopy.ColumnMappings.Add("ServiceID", "ServiceID");
                bulkCopy.ColumnMappings.Add("VisitID", "VisitID");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("SavedServicePrice", "SavedServicePrice");
                bulkCopy.ColumnMappings.Add("Discount", "Discount");

                bulkCopy.WriteToServer(dtServices);
            }
            clsBillingServiceData.UpdatePaymentStatus(VisitID,1, transaction);
            return true;


        }

        public static bool DeleteVisitServices(int VisitID, SqlTransaction transaction)
        {
            using (SqlCommand cmdDelete = new SqlCommand("DELETE FROM VisitServices WHERE VisitID = @VisitID", transaction.Connection, transaction))
            {
                cmdDelete.Parameters.AddWithValue("@VisitID", VisitID);
                cmdDelete.ExecuteNonQuery();
            }


            return true;




        }

    }
}
