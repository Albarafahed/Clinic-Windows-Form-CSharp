using Clinic_DataAccess; // تأكد من وجود المراجع الصحيحة
using Clinic_DataAccess.SaveSqlException;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsMedicineData
    {
        // 1. جلب بيانات دواء واحد عبر الـ ID
        public static bool GetMedicineInfoByID(int MedicineID, ref string MedicineName, ref decimal MedicinePrice,
                                               ref int CurrentStock, ref int ReorderLevel, ref decimal TaxRate)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM Medicines WHERE MedicineID = @MedicineID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MedicineID", MedicineID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                MedicineName = (string)reader["MedicineName"];
                                MedicinePrice = (decimal)reader["MedicinePrice"];
                                CurrentStock = (int)reader["CurrentStock"];
                                ReorderLevel = (int)reader["ReorderLevel"];
                                TaxRate = (decimal)reader["TaxRate"];
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

        // 3. (إضافي) للتحقق من وجود الدواء بالاسم قبل إضافته
        public static bool IsMedicineExists(string MedicineName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT Found = 1 FROM Medicines WHERE MedicineName = @MedicineName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MedicineName", MedicineName);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null) isFound = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
            }
            return isFound;
        }

        public static DataTable GetAllMedicines()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT *
                                FROM Medicines;";



                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public static bool IsMedicineAvailable(int medicineId, int requestedQuantity)
        {
            string query = "SELECT CurrentStock FROM Medicines WHERE MedicineID = @MedicineID";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MedicineID", medicineId);

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int c))
                        {
                            int currentStock = c;

                            // التأكد من أن الكمية في المخزن تغطي الكمية المطلوبة
                            if (currentStock >= requestedQuantity)
                            {
                                return true; // الدواء متوفر والكمية كافية
                            }

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);

            }
            return false;

        }

        public static int AddNewMedicine(string MedicineName, decimal MedicinePrice, int CurrentStock, int ReorderLevel, decimal TaxRate)
        {
            int medicineID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Medicines (MedicineName, MedicinePrice, CurrentStock, ReorderLevel, TaxRate)
                             VALUES (@MedicineName, @MedicinePrice, @CurrentStock, @ReorderLevel, @TaxRate);
                             SELECT SCOPE_IDENTITY();"; // لاسترجاع ID الدواء الجديد

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MedicineName", MedicineName);
                        command.Parameters.AddWithValue("@MedicinePrice", MedicinePrice);
                        command.Parameters.AddWithValue("@CurrentStock", CurrentStock);
                        command.Parameters.AddWithValue("@ReorderLevel", ReorderLevel);
                        command.Parameters.AddWithValue("@TaxRate", TaxRate);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            medicineID = insertedID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, -1);
            }
            return medicineID;
        }

        public static bool UpdateMedicine(int MedicineID, string MedicineName, decimal MedicinePrice, int CurrentStock, int ReorderLevel, decimal TaxRate)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Medicines 
                             SET MedicineName = @MedicineName,
                                 MedicinePrice = @MedicinePrice,
                                 CurrentStock = @CurrentStock,
                                 ReorderLevel = @ReorderLevel,
                                 TaxRate = @TaxRate
                             WHERE MedicineID = @MedicineID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MedicineID", MedicineID);
                        command.Parameters.AddWithValue("@MedicineName", MedicineName);
                        command.Parameters.AddWithValue("@MedicinePrice", MedicinePrice);
                        command.Parameters.AddWithValue("@CurrentStock", CurrentStock);
                        command.Parameters.AddWithValue("@ReorderLevel", ReorderLevel);
                        command.Parameters.AddWithValue("@TaxRate", TaxRate);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, MedicineID);
                return false;
            }
            return (rowsAffected > 0);
        }

        public static bool DeleteMedicine(int MedicineID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"Delete From Medicines 
                             WHERE MedicineID = @MedicineID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MedicineID", MedicineID);
                      

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, MedicineID);
                return false;
            }
            return (rowsAffected > 0);
        }
    }
}