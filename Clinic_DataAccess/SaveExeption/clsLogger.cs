using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess.SaveExeption
{
    public class clsLogger
    {
        public delegate void LogEventHandler(SqlException ex,string Level,int UserID);
        private LogEventHandler _logEvent;

        public clsLogger(LogEventHandler logEvent)
        {
            _logEvent = logEvent;
        }

        public void Log(SqlException ex, string Level, int UserID)
        {
            _logEvent(ex, Level, UserID);
        }
    }
}
