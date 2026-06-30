using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsBillingServiceData
    {
        public static bool CheckInPatient(int AppointmentID, int DoctorID, int PatientID, decimal Fees, int UserID, string PaymentMethod)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. إنشاء سجل الزيارة (Visit)
                        string queryVisit = @"INSERT INTO Visits (PatientID, AppointmentID, DoctorID, VisitDate, VisitStatus, CreatedByUserID)
                                         VALUES (@PatientID, @AppointmentID, @DoctorID, GETDATE(), 1, @UserID);
                                         SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdVisit = new SqlCommand(queryVisit, connection, transaction);
                        cmdVisit.Parameters.AddWithValue("@PatientID", PatientID);
                        cmdVisit.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        cmdVisit.Parameters.AddWithValue("@UserID", UserID);
                        cmdVisit.Parameters.AddWithValue("@DoctorID", DoctorID);

                        int VisitID = Convert.ToInt32(cmdVisit.ExecuteScalar());

                        string queryBill = @"INSERT INTO Bills (VisitID, PaymentStatus, CreatedByUserID, BillDate) 
                             VALUES (@VisitID, 2, @UserID, GETDATE()); 
                             SELECT SCOPE_IDENTITY();";
                        int BillID = -1;
                        using (SqlCommand cmdBill = new SqlCommand(queryBill, connection, transaction))
                        {
                            cmdBill.Parameters.AddWithValue("@VisitID", VisitID);
                            cmdBill.Parameters.AddWithValue("@UserID", UserID);
                            BillID = Convert.ToInt32(cmdBill.ExecuteScalar());

                            UpdateBillNumber(BillID, transaction);
                        }

                        // 4. تسجيل الدفعة (هنا يتم استخدام PaymentMethod)
                        string queryPay = @"INSERT INTO Payments (BillID, PaymentAmount, PaymentDate, PaymentMethod, CreatedByUserID) 
                            VALUES (@BillID, @Amount, GETDATE(), @Method, @UserID)";

                        using (SqlCommand cmdPay = new SqlCommand(queryPay, connection, transaction))
                        {
                            cmdPay.Parameters.AddWithValue("@BillID", BillID);
                            cmdPay.Parameters.AddWithValue("@Amount", Fees);
                            cmdPay.Parameters.AddWithValue("@Method", PaymentMethod);
                            cmdPay.Parameters.AddWithValue("@UserID", UserID);
                            cmdPay.ExecuteNonQuery();
                        }

                        // 5. تحديث حالة الموعد
                        clsAppointmentData.UpdateAppointmentStatus(AppointmentID, 2, UserID, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                        return false;
                    }
                }
            }
        }

        public static bool UpdateBillNumber(int BillID, SqlTransaction transaction)
        {
            string queryUpdateBillNumber = @"UPDATE Bills SET BillNumber = 'INV-' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-' + RIGHT('0000' + CAST(@BillID AS VARCHAR), 4) 
                                                 WHERE BillID = @BillID";
            SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, transaction.Connection, transaction);
            cmdUpdate.Parameters.AddWithValue("@BillID", BillID);
            return cmdUpdate.ExecuteNonQuery() > 0;
        }

        public static bool UpdatePaymentStatus(int VisitID, byte PaymentStatus, SqlTransaction transaction)
        {
            string queryUpdateBillNumber = @"UPDATE Bills SET PaymentStatus=@PaymentStatus 
                                                 WHERE VisitID = @VisitID";
            SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, transaction.Connection, transaction);
            cmdUpdate.Parameters.AddWithValue("@VisitID", VisitID);
            cmdUpdate.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);

            return cmdUpdate.ExecuteNonQuery() > 0;
        }

        public static bool UpdatePaymentStatus(int BillID, byte PaymentStatus)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

                {
                    string queryUpdateBillNumber = @"UPDATE Bills SET PaymentStatus=@PaymentStatus 
                                                 WHERE BillID = @BillID";
                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdateBillNumber, connection ))
                    {
                        cmdUpdate.Parameters.AddWithValue("@BillID", BillID);
                        cmdUpdate.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);
                        connection.Open();
                        return cmdUpdate.ExecuteNonQuery() > 0;
                    }

                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                return false;
            }

        }

        public static DataTable GetAllBills()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SElECT * FROM View_Bills
                                     ORDER BY BillNumber DESC;";
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

        public static DataRow GetBillByID(int BillID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM View_Bills WHERE BillID = @BillID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);

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

            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        public static bool CancelBillAndRestoreStock(int billID, int userID)
        {
            bool isCancelled = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // 1. إرجاع المخزون
                        string query1 = @"
                    UPDATE M
                    SET M.CurrentStock = M.CurrentStock + PD.DispensedQuantity
                    FROM Medicines M
                    INNER JOIN PrescriptionDetails PD ON M.MedicineID = PD.MedicineID
                    INNER JOIN Prescriptions P ON P.PrescriptionID = PD.PrescriptionID
                    WHERE P.BillID = @BillID
                    AND PD.DispensedQuantity > 0;
                ";

                        using (SqlCommand cmd1 = new SqlCommand(query1, connection, transaction))
                        {
                            cmd1.Parameters.AddWithValue("@BillID", billID);
                            cmd1.ExecuteNonQuery();
                        }

                        // 2. تسجيل حركة الإرجاع في InventoryTransactions
                        string query2 = @"
                    INSERT INTO InventoryTransactions
                    (
                        MedicineID,
                        QuantityChange,
                        ReferenceID,
                        TransactionDate,
                        UserID
                    )
                    SELECT
                        PD.MedicineID,
                        PD.DispensedQuantity,
                        @BillID,
                        GETDATE(),
                        @UserID
                    FROM PrescriptionDetails PD
                    INNER JOIN Prescriptions P ON P.PrescriptionID = PD.PrescriptionID
                    WHERE P.BillID = @BillID
                    AND PD.DispensedQuantity > 0;
                ";

                        using (SqlCommand cmd2 = new SqlCommand(query2, connection, transaction))
                        {
                            cmd2.Parameters.AddWithValue("@BillID", billID);
                            cmd2.Parameters.AddWithValue("@UserID", userID);
                            cmd2.ExecuteNonQuery();
                        }

                        // 3. تصفير الصرف
                        string query3 = @"
                    UPDATE PrescriptionDetails
                    SET DispensedQuantity = 0,
                        IsDispensed = 0
                    WHERE PrescriptionID IN
                    (SELECT PrescriptionID FROM Prescriptions WHERE BillID = @BillID);
                ";

                        using (SqlCommand cmd3 = new SqlCommand(query3, connection, transaction))
                        {
                            cmd3.Parameters.AddWithValue("@BillID", billID);
                            cmd3.ExecuteNonQuery();
                        }

                        // 4. إلغاء الوصفات
                        string query4 = @"
                    UPDATE Prescriptions
                    SET PrescriptionStatus = 6
                    WHERE BillID = @BillID;
                ";

                        using (SqlCommand cmd4 = new SqlCommand(query4, connection, transaction))
                        {
                            cmd4.Parameters.AddWithValue("@BillID", billID);
                            cmd4.ExecuteNonQuery();
                        }

                        // 5. إلغاء الفاتورة (آخر خطوة للتحقق)
                        string query5 = @"
                    UPDATE Bills
                    SET PaymentStatus = 4
                    WHERE BillID = @BillID
                    AND PaymentStatus = 0;
                ";

                        using (SqlCommand cmd5 = new SqlCommand(query5, connection, transaction))
                        {
                            cmd5.Parameters.AddWithValue("@BillID", billID);

                            int affected = cmd5.ExecuteNonQuery();

                            if (affected == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        isCancelled = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error, billID);
                isCancelled = false;
            }

            return isCancelled;
        }
        public static bool GetBillSummaryData(int BillID,
         ref string billNumber, ref string patientName,
         ref decimal appointmentFees, ref decimal totalMedicines, ref decimal totalServices,
         ref decimal totalDiscount, ref decimal totalTax, ref decimal paymentAmount, ref decimal balanceDue,
         ref decimal totalBeforeDiscountAndTax, ref decimal finalTotalIncludingAll, DataTable dtBillDetails)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT 
                                    BillNumber, PatientName, AppointmentFees, MedicinesTotal, 
                                    ServicesTotal, TotalDiscount, TotalTax, PaymentAmount, BalanceDue,
                                    (AppointmentFees + MedicinesTotal + ServicesTotal) AS TotalBeforeDiscountAndTax,
                                    ((AppointmentFees + MedicinesTotal + ServicesTotal - TotalDiscount) + TotalTax) AS FinalTotalIncludingAll
                                 FROM View_BillSummaries 
                                 WHERE BillID = @BillID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BillID", BillID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                billNumber = reader["BillNumber"].ToString();
                                patientName = reader["PatientName"].ToString();
                                appointmentFees = reader["AppointmentFees"] is DBNull ? 0 : Convert.ToDecimal(reader["AppointmentFees"]);
                                totalMedicines = reader["MedicinesTotal"] is DBNull ? 0 : Convert.ToDecimal(reader["MedicinesTotal"]);
                                totalServices = reader["ServicesTotal"] is DBNull ? 0 : Convert.ToDecimal(reader["ServicesTotal"]);
                                totalDiscount = reader["TotalDiscount"] is DBNull ? 0 : Convert.ToDecimal(reader["TotalDiscount"]);
                                totalTax = reader["TotalTax"] is DBNull ? 0 : Convert.ToDecimal(reader["TotalTax"]);
                                paymentAmount = reader["PaymentAmount"] is DBNull ? 0 : Convert.ToDecimal(reader["PaymentAmount"]);
                                balanceDue = reader["BalanceDue"] is DBNull ? 0 : Convert.ToDecimal(reader["BalanceDue"]);
                                totalBeforeDiscountAndTax = reader["TotalBeforeDiscountAndTax"] is DBNull ? 0 : Convert.ToDecimal(reader["TotalBeforeDiscountAndTax"]);
                                finalTotalIncludingAll = reader["FinalTotalIncludingAll"] is DBNull ? 0 : Convert.ToDecimal(reader["FinalTotalIncludingAll"]);

                                // تعبئة الـ DataTable الممرر
                                dtBillDetails.Clear();
                                if (dtBillDetails.Columns.Count == 0)
                                {
                                    dtBillDetails.Columns.Add("BillNumber", typeof(string));
                                    dtBillDetails.Columns.Add("PatientName", typeof(string));
                                    dtBillDetails.Columns.Add("TotalBeforeDiscountAndTax", typeof(decimal));
                                    dtBillDetails.Columns.Add("FinalTotalIncludingAll", typeof(decimal));
                                    dtBillDetails.Columns.Add("BalanceDue", typeof(decimal));
                                }
                                DataRow row = dtBillDetails.NewRow();
                                row["BillNumber"] = billNumber;
                                row["PatientName"] = patientName;
                                row["TotalBeforeDiscountAndTax"] = totalBeforeDiscountAndTax;
                                row["FinalTotalIncludingAll"] = finalTotalIncludingAll;
                                row["BalanceDue"] = balanceDue;
                                dtBillDetails.Rows.Add(row);

                                return true;
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



        #region EDitPending Prescription ITems
        public static DataTable GetPrescriptionsDetails(int BillID, ref int PrescriptionID, ref DateTime PrescriptionDate, ref bool isFound)
        {
            DataTable dtDetails = new DataTable();
            isFound = false;

            string query = @"
        -- الاستعلام الأول: جلب معلومات الفاتورة والمريض العامة
          Select PrescriptionID,Format(PrescriptionDate,'yyyy/MM/dd')As PrescriptionDate
                 From Prescriptions
                Where BillID=@BillID

        -- الاستعلام الثاني: جلب تفاصيل الأدوية والضرائب والكميات المرتجعة سابقاً
         Select PrescriptionDetailsID,SavedMedicineName,SavedMedicinePrice,DispensedQuantity
                  From PrescriptionDetails PD
                  INNER JOIN Prescriptions P ON PD.PrescriptionID=P.PrescriptionID
                  Where P.BillID=@BillID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BillID", BillID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // 1. قراءة النتيجة الأولى (المعلومات العامة للفاتورة)
                            if (reader.Read())
                            {
                                PrescriptionID = (int)reader["PrescriptionID"];
                                PrescriptionDate = Convert.ToDateTime(reader["PrescriptionDate"]); // تم تصحيح الكاستنج هنا
                                isFound = true;
                            }

                            // 2. الانتقال إلى النتيجة الثانية (جدول تفاصيل الأدوية)
                            if (reader.NextResult())
                            {
                                if (reader.HasRows)
                                {
                                    dtDetails.Load(reader);
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
                        isFound = false;
                    }
                }
            }
            return dtDetails;
        }

        public static bool UpdateAfterEditPendingPrescription(DataTable dtDetails,int BillID,int CreatedByUserID)
        {
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. إنشاء الجدول المؤقت
                    conn.ExecuteNonQuery("CREATE TABLE #TempDispensed (DetailsID INT, DispensedQty INT)", transaction);

                    // 2. نقل البيانات عبر SqlBulkCopy
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "#TempDispensed";
                        bulkCopy.ColumnMappings.Add("PrescriptionDetailsID", "DetailsID");
                        bulkCopy.ColumnMappings.Add("QtyReturned", "DispensedQty");
                        bulkCopy.WriteToServer(dtDetails);
                    }

                    // 3. تحديث الجدول الأصلي
                    string updateQuery = @"UPDATE PD 
                                   SET PD.DispensedQuantity =PD.DispensedQuantity - T.DispensedQty, 
                                       PD.IsDispensed = CASE WHEN (PD.DispensedQuantity - T.DispensedQty) > 0 THEN 1 ELSE 0 END
                                   FROM PrescriptionDetails PD
                                   INNER JOIN #TempDispensed T ON PD.PrescriptionDetailsID = T.DetailsID";
                    conn.ExecuteNonQuery(updateQuery, transaction);

                  
                    

                    string updateStockQuery = @"
                                UPDATE M
                                SET M.CurrentStock = M.CurrentStock + T.DispensedQty
                                FROM Medicines M
                                INNER JOIN PrescriptionDetails PD ON M.MedicineID = PD.MedicineID
                                INNER JOIN #TempDispensed T ON PD.PrescriptionDetailsID = T.DetailsID
                                WHERE T.DispensedQty > 0;";

                    using (SqlCommand cmdUpdateStock = new SqlCommand(updateStockQuery, conn, transaction))
                    {
                        cmdUpdateStock.ExecuteNonQuery();
                    }

                    // ب. تسجيل حركة الصرف بالسالب في جدول InventoryTransactions للرقابة والتدقيق
                    string insertTransactionQuery = @"
                                    INSERT INTO InventoryTransactions (MedicineID, QuantityChange, ReferenceID, TransactionDate, UserID)
                                    SELECT 
                                        PD.MedicineID,
                                        +T.DispensedQty,  
                                        @BillID,          -- المرجع هنا هو رقم الفاتورة التي تم ربطها
                                        GETDATE(),
                                        @UserID
                                    FROM #TempDispensed T
                                    INNER JOIN PrescriptionDetails PD ON T.DetailsID = PD.PrescriptionDetailsID
                                    WHERE T.DispensedQty > 0;";

                    using (SqlCommand cmdInsertTrans = new SqlCommand(insertTransactionQuery, conn, transaction))
                    {
                        cmdInsertTrans.Parameters.AddWithValue("@BillID", BillID);
                        cmdInsertTrans.Parameters.AddWithValue("@UserID", CreatedByUserID);
                        cmdInsertTrans.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                    clsGlobalLogger.LogSqlException(ex, clsGlobalLogger.LogLevel.Error);
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

        #endregion


    }
}
