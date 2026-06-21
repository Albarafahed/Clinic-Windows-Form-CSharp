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
    public class clsPrescriptionData
    {
        public static int SavePrescription(int? VisitID, string Notes, DateTime Date, DataTable dtMedicines, byte PrescriptionStatus, byte Prescriptiontype)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. حفظ رأس الوصفة واسترجاع الـ ID
                        string queryMaster = @"INSERT INTO Prescriptions (VisitID, PrescriptionDate, PrescriptionNotes,PrescriptionStatus,Prescriptiontype) 
                                      VALUES (@VisitID, @Date, @Notes,@PrescriptionStatus,@Prescriptiontype);
                                      SELECT SCOPE_IDENTITY();";

                        int prescriptionID = -1;
                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, connection, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@VisitID", (object)VisitID ?? DBNull.Value);
                            cmdMaster.Parameters.AddWithValue("@Date", Date);
                            cmdMaster.Parameters.AddWithValue("@Notes", Notes.ToDBValue());
                            cmdMaster.Parameters.AddWithValue("@PrescriptionStatus", PrescriptionStatus);
                            cmdMaster.Parameters.AddWithValue("@Prescriptiontype", Prescriptiontype);

                            prescriptionID = Convert.ToInt32(cmdMaster.ExecuteScalar());
                        }

                        dtMedicines.Columns.Add("PrescriptionID", typeof(int), prescriptionID.ToString());

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "PrescriptionDetails";

                            // ربط الأعمدة (يجب أن تتطابق مع أسماء الأعمدة في DataTable و SQL)
                            bulkCopy.ColumnMappings.Add("PrescriptionID", "PrescriptionID");
                            bulkCopy.ColumnMappings.Add("MedicineID", "MedicineID");
                            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                            bulkCopy.ColumnMappings.Add("Dosage", "Dosage");
                            bulkCopy.ColumnMappings.Add("Instructions", "Instructions");
                            bulkCopy.ColumnMappings.Add("SavedMedicineName", "SavedMedicineName");
                            bulkCopy.ColumnMappings.Add("SavedMedicinePrice", "SavedMedicinePrice");
                            bulkCopy.ColumnMappings.Add("Frequency", "Frequency");
                            bulkCopy.ColumnMappings.Add("DiscountAmount", "DiscountAmount");
                            bulkCopy.WriteToServer(dtMedicines);
                        }
                        if (VisitID.HasValue)
                            clsVisitData.UpdateVisitTotalAmount(VisitID.Value, transaction);
                        transaction.Commit();
                        return prescriptionID;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // يُفضل تسجيل الخطأ هنا باستخدام Logger الخاص بك
                        return -1;
                    }
                }
            }
        }

        public static bool UpdatePrescription(int PrescriptionID, int VisitID, string Notes, DateTime Date, byte PrescriptionStatus, DataTable dtMedicines)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryMaster = @"UPDATE Prescriptions 
                                       SET PrescriptionDate = @Date, 
                                           PrescriptionNotes = @Notes ,
                                            PrescriptionStatus=@PrescriptionStatus
                                       WHERE PrescriptionID = @PrescriptionID";

                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, connection, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
                            cmdMaster.Parameters.AddWithValue("@Date", Date);
                            cmdMaster.Parameters.AddWithValue("@Notes", Notes.ToDBValue());
                            cmdMaster.Parameters.AddWithValue("@PrescriptionStatus", PrescriptionStatus);

                            cmdMaster.ExecuteNonQuery();
                        }

                        // 2. حذف التفاصيل القديمة (عن طريق الـ PrescriptionID)
                        string queryDeleteDetails = "DELETE FROM PrescriptionDetails WHERE PrescriptionID = @PrescriptionID";
                        using (SqlCommand cmdDelete = new SqlCommand(queryDeleteDetails, connection, transaction))
                        {
                            cmdDelete.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
                            cmdDelete.ExecuteNonQuery();
                        }


                        if (!dtMedicines.Columns.Contains("PrescriptionID"))
                            dtMedicines.Columns.Add("PrescriptionID", typeof(int), PrescriptionID.ToString());

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "PrescriptionDetails";

                            bulkCopy.ColumnMappings.Add("PrescriptionID", "PrescriptionID");
                            bulkCopy.ColumnMappings.Add("MedicineID", "MedicineID");
                            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                            bulkCopy.ColumnMappings.Add("Dosage", "Dosage");
                            bulkCopy.ColumnMappings.Add("Instructions", "Instructions");
                            bulkCopy.ColumnMappings.Add("SavedMedicineName", "SavedMedicineName");
                            bulkCopy.ColumnMappings.Add("SavedMedicinePrice", "SavedMedicinePrice");
                            bulkCopy.ColumnMappings.Add("Frequency", "Frequency");
                            bulkCopy.ColumnMappings.Add("DiscountAmount", "DiscountAmount");
                            bulkCopy.WriteToServer(dtMedicines);
                        }
                        clsVisitData.UpdateVisitTotalAmount(VisitID, transaction);

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
        public static DataTable GetVisitMedicines(int VisitID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"
                                      SELECT
                                    PPD.MedicineID, 
                                    PPD.SavedMedicineName, 
                                    PPD.Dosage, 
                                    PPD.Instructions,
                                    PPD.SavedMedicinePrice, 
                                    PPD.Quantity,
                                    PPD.Frequency,
                                    PPD.DiscountAmount
                                FROM PrescriptionS PP
                                INNER JOIN PrescriptionDetails PPD ON 
                                    PP.PrescriptionID = PPD.PrescriptionID
                                WHERE PP.VisitID = @VisitID;";


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

        public static DataTable GetAllActivePrescriptions()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @" SELECT 
                                        P.PrescriptionID,
                                         P.VisitID,
                                         V.AppointmentID,   
                                        PP.FullName AS PatientName,
                                        PD.FullName AS DoctorName,
                                        P.PrescriptionDate,
                                        CASE P.PrescriptionStatus
                                        WHEN 1 THEN 'Pending'
                                        WHEN 2 THEN 'Waiting For Payment'
                                        WHEN 3 THEN 'Ready For Dispensing'
                                        WHEN 4 THEN 'Dispensed'
                                        WHEN 5 THEN 'Partially Dispensed'
                                        WHEN 6 THEN 'Cancelled'
                                        ELSE 'Unknown'
                                    END AS Status,
                                        P.VisitID,
                                         V.AppointmentID
                                    FROM Prescriptions AS P
                                    INNER JOIN Visits AS V ON P.VisitID = V.VisitID
                                    INNER JOIN Patients AS Pat ON V.PatientID = Pat.PatientID
                                    INNER JOIN Persons AS PP ON Pat.PersonID = PP.PersonID
                                    INNER JOIN Doctors AS D ON V.DoctorID = D.DoctorID
                                    INNER JOIN Persons AS PD ON D.PersonID = PD.PersonID
                                    WHERE P.PrescriptionStatus NOT IN (4, 6) 
                                      AND P.Prescriptiontype = 1;";


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

        public static DataTable GetAllPrescriptionDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 
                                            P.PrescriptionStatus,
                                            PD.PrescriptionID,
                                            PD.PrescriptionDetailsID,
                                            PD.SavedMedicineName AS MedicineName,
                                            PD.Dosage,
                                            M.TaxRate,
                                            PD.SavedMedicinePrice,
                                            PD.Frequency,
                                            PD.Quantity AS RequiredQuantity,
                                            PD.Instructions,
                                            PD.DiscountAmount,

                                            -- منطق ذكي للكمية المصروفة
                                            CASE 
                                                WHEN P.PrescriptionStatus = 1 THEN 
                                                    (CASE 
                                                        WHEN M.CurrentStock IS NULL OR M.CurrentStock <= 0 THEN 0
                                                        WHEN M.CurrentStock >= PD.Quantity THEN PD.Quantity
                                                        ELSE M.CurrentStock 
                                                    END)
                                                ELSE PD.DispensedQuantity 
                                            END AS DispensedQuantity,

                                            -- منطق ذكي للـ CheckBox
                                            CASE 
                                                WHEN P.PrescriptionStatus = 1 THEN 
                                                    (CASE 
                                                        WHEN M.CurrentStock IS NULL OR M.CurrentStock <= 0 THEN 0
                                                        ELSE 1 
                                                    END)
                                                ELSE PD.IsDispensed 
                                            END AS IsDispensed,

                                            -- منطق ذكي لحالة التوفر
                                            CASE 
                                                WHEN P.PrescriptionStatus = 1 THEN
                                                    (CASE 
                                                        WHEN M.CurrentStock IS NULL OR M.CurrentStock <= 0 THEN 'Out of Stock'
                                                        WHEN M.CurrentStock >= PD.Quantity THEN 'Fully Available'
                                                        ELSE 'Partially Available (' + CAST(M.CurrentStock AS VARCHAR) + ')'
                                                    END)
                                                ELSE 'Finalized'
                                            END AS AvailableStatus

                                        FROM PrescriptionDetails PD
                                        LEFT JOIN Prescriptions P ON PD.PrescriptionID = P.PrescriptionID
                                        LEFT JOIN Medicines M ON PD.SavedMedicineName = M.MedicineName;";
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
                                     
    
                                     
                                     
                                     
                                     

                                     
                                     
                                     
                                     
                                   


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

        public static DataTable GetAllPrescriptionRecords()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM View_PrescriptionDetails;";


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

        public static DataTable GetPrescriptionItemsRaw(int PrescriptionID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 
                                      --  PrescriptionID,
                                        SavedMedicineName AS MedicineName,
                                        Dosage,
                                        Frequency,
                                        Quantity,
                                        Instructions,
                                        SavedMedicinePrice As Price,
                                        DiscountAmount
                                    FROM PrescriptionDetails
                                        Where PrescriptionID=@PrescriptionID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);
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

        public static bool Find(int VisitID, ref int PrescriptionID, ref string PrescriptionNotes, ref DateTime PrescriptionDate, ref byte PrescriptionStatus)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT PrescriptionID,
                                            PrescriptionDate,
                                          PrescriptionNotes,
                                            PrescriptionStatus
                                      FROM  Prescriptions
                                      WHERE VisitID=@VisitID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VisitID", VisitID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PrescriptionID = (int)reader["PrescriptionID"];
                                PrescriptionDate = (DateTime)reader["PrescriptionDate"];
                                PrescriptionNotes = reader["PrescriptionNotes"].ToStringOrEmpty();
                                PrescriptionStatus = (byte)reader["PrescriptionStatus"];

                                return true;

                            }
                            else
                                return false;
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
        }


        public static bool UpdatePrescriptionStatus(int prescriptionId, int newStatus, SqlTransaction transaction)
        {
            string query = @"UPDATE Prescriptions 
                     SET PrescriptionStatus = @Status 
                     WHERE PrescriptionID = @PrescriptionID";

            using (SqlCommand command = new SqlCommand(query, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                command.Parameters.AddWithValue("@Status", newStatus);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool UpdatePrescriptionStatus(int prescriptionId, byte newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Prescriptions 
                         SET PrescriptionStatus = @Status 
                         WHERE PrescriptionID = @PrescriptionID";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                        command.Parameters.AddWithValue("@Status", newStatus);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }
        }

        public static decimal GetPrescriptionTotalForCashier(int prescriptionId)
        {
            decimal total = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT SUM((Quantity * SavedMedicinePrice) - DiscountAmount) 
                         FROM PrescriptionDetails 
                         WHERE PrescriptionID = @PrescriptionID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PrescriptionID", prescriptionId);


                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            total = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return -1;
            }

            return total;
        }

        public static bool SendToCashier(int prescriptionId, DataTable dtDispensedItems,int ? VisitID, int ? AppointmentID, decimal TotalMedicinesAmount,decimal TaxRate, int userId)
        {
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. إنشاء الجدول المؤقت
                    conn.ExecuteNonQuery("CREATE TABLE #TempDispensed (DetailsID INT, DispensedQty INT, IsDispensed BIT)", transaction);

                    // 2. نقل البيانات عبر SqlBulkCopy
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "#TempDispensed";
                        bulkCopy.ColumnMappings.Add("PrescriptionDetailsID", "DetailsID");
                        bulkCopy.ColumnMappings.Add("DispensedQuantity", "DispensedQty");
                        bulkCopy.ColumnMappings.Add("IsDispensed", "IsDispensed");
                        bulkCopy.WriteToServer(dtDispensedItems);
                    }

                    // 3. تحديث الجدول الأصلي
                    string updateQuery = @"UPDATE PD 
                                   SET PD.DispensedQuantity = T.DispensedQty, 
                                       PD.IsDispensed = T.IsDispensed
                                   FROM PrescriptionDetails PD
                                   INNER JOIN #TempDispensed T ON PD.PrescriptionDetailsID = T.DetailsID";
                    conn.ExecuteNonQuery(updateQuery, transaction);

                    // 4. استدعاء دالتك الجاهزة لتحديث الحالة (مع تمرير الـ Transaction لضمان الأمان)
                    // ملاحظة: تأكد أن دالتك الموجودة مسبقاً تقبل (SqlConnection, SqlTransaction)
                    if (!UpdatePrescriptionStatus(prescriptionId, 2, transaction))
                    {
                        throw new Exception("Failed to update prescription status.");
                    }
                  
                    // 2. حساب التكلفة الإجمالية بناءً على القيمة النقدية
                    decimal TotalCost = TotalMedicinesAmount + TaxRate;

                    // 5. إدراج الفاتورة
                    string insertBill = @"INSERT INTO Bills (PrescriptionID, TotalMedicinesAmount,TaxAmount,TotalCost, PaymentStatus, BillDate, CreatedByUserID,VisitID,AppointmentID, IsVoid) 
                                  VALUES (@PID, @TotalMedicinesAmount,@TaxAmount,@TotalCost, 0, GETDATE(), @UID,@VisitID,@AppointmentID, 0);
                                         SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmdBill = new SqlCommand(insertBill, conn, transaction))
                    {
                        cmdBill.Parameters.AddWithValue("@PID", prescriptionId);
                        cmdBill.Parameters.AddWithValue("@TotalMedicinesAmount", TotalMedicinesAmount);
                        cmdBill.Parameters.AddWithValue("@UID", userId);
                        cmdBill.Parameters.AddWithValue("@VisitID", VisitID);
                        cmdBill.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        cmdBill.Parameters.AddWithValue("@TaxAmount", TaxRate);
                        cmdBill.Parameters.AddWithValue("@TotalCost", TotalCost);

                        int BillID = Convert.ToInt32(cmdBill.ExecuteScalar());

                        // 3. توليد رقم الفاتورة

                      clsBillingServiceData.UpdateBillNumber(BillID, transaction);

                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    try { conn.ExecuteNonQuery("IF OBJECT_ID('tempdb..#TempDispensed') IS NOT NULL DROP TABLE #TempDispensed", transaction); }
                    catch { }
                }
            }
        }
    }
}
