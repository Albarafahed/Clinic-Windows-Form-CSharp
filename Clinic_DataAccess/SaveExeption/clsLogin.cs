using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess.SaveExeption
{
    public class clsLogin
    {
        private static string GetCleanStackTrace(SqlException ex)
        {
            if (ex.StackTrace == null) return "No Stack Trace";

            // تقسيم الـ StackTrace إلى أسطر
            string[] lines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // استخراج فقط الأسطر التي تحتوي على اسم مشروعك (Clinic_)
            var cleanLines = lines.Where(line => line.Contains("Clinic_"));

            // إعادة تجميعها في نص واحد
            return string.Join(Environment.NewLine, cleanLines);
        }
        public static void LogToScreen(SqlException ex, string Level, int UserID)
        {
            Console.WriteLine($"SqlException: {ex.Message}, Level: {Level}, User ID: {UserID}");
        }
        public static void LogToFile(SqlException ex, string Level, int UserID)
        {
            string fileName = "SystemLogTable.csv";
            string backupFileName = "SystemLogTable_Backup.csv"; // الملف البديل

            try
            {
                // محاولة الكتابة في الملف الأساسي
                _WriteToFile(fileName, ex, Level, UserID);
            }
            catch (IOException)
            {
                // إذا كان الملف مشغولاً، ننتقل فوراً للملف البديل
                try
                {
                    _WriteToFile(backupFileName, ex, Level, UserID);
                }
                catch
                {
                    // هنا نكون قد استنفدنا كل الحلول، ممكن نكتب في الـ Trace كما اقترحنا سابقاً
                    System.Diagnostics.Trace.WriteLine("Critical Failure: Could not write to main or backup log.");
                }
            }
        }
        private static void _WriteToFile(string path, SqlException ex, string Level, int UserID)
        {

            bool fileExists = File.Exists(path);
            using (StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                if (!fileExists)
                {
                    writer.WriteLine("Timestamp,LogLevel,UserID,Message,StackTrace,Source,MachineName");
                }

                string line = string.Format("{0},{1},{2},\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Level,
                    UserID == -1 ? "N/A" : UserID.ToString(),
                    ex.Message.Replace(",", " "),
                    GetCleanStackTrace(ex).Replace(",", " "),
                    ex.Source ?? "Unknown",
                    Environment.MachineName
                );
                writer.WriteLine(line);
            }
        }
        public static void LogToDatabase(SqlException ex, string level, int UserID)
        {
            //write the code to log the message in the database .


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO SystemLogs (LogLevel, Message, StackTrace, Source, UserID, MachineName, Timestamp)
                             VALUES (@Level, @Message, @StackTrace, @Source, @UserID, @MachineName, @Timestamp)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Level", level);
                        cmd.Parameters.AddWithValue("@Message", ex.Message);
                        cmd.Parameters.AddWithValue("@StackTrace", (object)ex.StackTrace ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Source", (object)ex.Source ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", UserID != -1 ? (object)UserID : DBNull.Value);
                        cmd.Parameters.AddWithValue("@MachineName", Environment.MachineName);
                        cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException exLog)
            {
                System.Diagnostics.Trace.WriteLine("Database Log Failed, switching to File Log: " + exLog.Message);
                LogToFile(ex, level, UserID);
            }

            
               
                  
        }


        
    }
}
