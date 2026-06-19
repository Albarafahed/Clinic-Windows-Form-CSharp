using System.Data;
using System.Data.SqlClient;

public static class SqlExtension
{
    // دالة لتنفيذ الأوامر البسيطة (مثل CREATE TABLE أو UPDATE بسيط)
    public static int ExecuteNonQuery(this SqlConnection conn, string query, SqlTransaction transaction = null)
    {
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            if (transaction != null)
                cmd.Transaction = transaction;
            return cmd.ExecuteNonQuery();
        }
    }
}