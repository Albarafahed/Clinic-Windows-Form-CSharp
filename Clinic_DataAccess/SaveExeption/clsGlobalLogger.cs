using Clinic_DataAccess.SaveExeption;
using System;

namespace Clinic_DataAccess.SaveException
{
    public static class clsGlobalLogger
    {
        // نجعل الـ logger خاصاً ولا يمكن تغييره من الخارج
        private static clsLogger _logger = new clsLogger(clsLogin.LogToDatabase);

        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            Critical
        }
        public static void LogException(Exception ex, LogLevel level, int userID = -1)
        {
            _logger.Log(ex, level.ToString(), userID);
        }
    }
}