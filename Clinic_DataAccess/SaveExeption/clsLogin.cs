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
        private static string GetCleanStackTrace(Exception ex)
        {
            if (ex.StackTrace == null) return "No Stack Trace";

            // تقسيم الـ StackTrace إلى أسطر
            string[] lines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // استخراج فقط الأسطر التي تحتوي على اسم مشروعك (Clinic_)
            var cleanLines = lines.Where(line => line.Contains("Clinic_"));

            // إعادة تجميعها في نص واحد
            return string.Join(Environment.NewLine, cleanLines);
        }

     
        public static void LogToScreen(Exception ex, string Level, int UserID)
        {
            Console.WriteLine($"Exception: {ex.Message}, Level: {Level}, User ID: {UserID}");
        }

        // Method to log to a file
        public static void LogToFile1(Exception ex, string Level, int UserID)
        {
            string fileName = "SystemLogTable.csv"; // تغيير الامتداد لـ csv ليسهل فتحه بالإكسيل
            bool fileExists = File.Exists(fileName);

            using (StreamWriter writer = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                // إذا كان الملف جديداً، نكتب رأس الجدول أولاً
                if (!fileExists)
                {
                    writer.WriteLine("Timestamp,LogLevel,UserID,Message,StackTrace,Source,MachineName");
                }

                // كتابة البيانات كصف في الجدول
                // نستخدم التنسيق: قيمة,قيمة,قيمة
                string line = string.Format("{0},{1},{2},\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // {0}
     Level,                                       // {1}
     UserID == -1 ? "N/A" : UserID.ToString(),    // {2}
     ex.Message.Replace(",", " "),                // {3}
     GetCleanStackTrace(ex).Replace(",", " "),    // {4}
     ex.Source ?? "Unknown",                      // {5}
     Environment.MachineName                      // {6}
 );
                

                writer.WriteLine(line);
            }
        }

        public static void LogToFile(Exception ex, string Level, int UserID)
        {
            string fileName = "SystemLogTable.csv";
            string backupFileName = "SystemLogTable_Backup.csv"; // الملف البديل

            try
            {
                // محاولة الكتابة في الملف الأساسي
                WriteToFile(fileName, ex, Level, UserID);
            }
            catch (IOException)
            {
                // إذا كان الملف مشغولاً، ننتقل فوراً للملف البديل
                try
                {
                    WriteToFile(backupFileName, ex, Level, UserID);
                }
                catch
                {
                    // هنا نكون قد استنفدنا كل الحلول، ممكن نكتب في الـ Trace كما اقترحنا سابقاً
                    System.Diagnostics.Trace.WriteLine("Critical Failure: Could not write to main or backup log.");
                }
            }
        }

        private static void WriteToFile(string path, Exception ex, string Level, int UserID)
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
        public static void LogToDatabase(Exception ex, string level, int UserID)
        {
            //write the code to log the message in the database .

            string query = @"INSERT INTO SystemLogs (LogLevel, Message, StackTrace, Source, UserID, MachineName, Timestamp)
                             VALUES (@Level, @Message, @StackTrace, @Source, @UserID, @MachineName, @Timestamp)";

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Level", level);
                cmd.Parameters.AddWithValue("@Message", ex.Message);
                cmd.Parameters.AddWithValue("@StackTrace", (object)ex.StackTrace ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Source", (object)ex.Source ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UserID", UserID != -1 ? (object)UserID : DBNull.Value);
                cmd.Parameters.AddWithValue("@MachineName", Environment.MachineName);
                cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException exLog)
                {
                    System.Diagnostics.Trace.WriteLine("Database Log Failed, switching to File Log: " + exLog.Message);
                    LogToFile(ex, level, UserID);
                } 
                  
            }


        }
    }
}
