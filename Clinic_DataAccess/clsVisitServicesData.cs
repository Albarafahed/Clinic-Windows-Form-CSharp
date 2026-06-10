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

        public static bool SaveVisitServices(int VisitID, DataTable dtServices)
        {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // الحذف
                        using (SqlCommand cmdDelete = new SqlCommand("DELETE FROM VisitServices WHERE VisitID = @VisitID", connection, transaction))
                        {
                            cmdDelete.Parameters.AddWithValue("@VisitID", VisitID);
                            cmdDelete.ExecuteNonQuery();
                        }

                        dtServices.Columns.Add("VisitID", typeof(int), VisitID.ToString());

                        // الإدراج الجماعي
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "VisitServices";
                        bulkCopy.ColumnMappings.Add("ServiceID", "ServiceID");
                        bulkCopy.ColumnMappings.Add("VisitID", "VisitID");
                        bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                        bulkCopy.ColumnMappings.Add("SavedServicePrice", "SavedServicePrice");
                        bulkCopy.ColumnMappings.Add("Discount", "Discount");

                        bulkCopy.WriteToServer(dtServices);
                    }
                        clsVisitData.UpdateVisitTotalAmount(VisitID,transaction);
                    transaction.Commit();
                    return true;
                }
                    catch (Exception)
                    {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
}
