using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess.SaveExeption
{
    public class clsLogger
    {
        public delegate void LogEventHandler(Exception ex,string Level,int UserID);
        private LogEventHandler _logEvent;

        public clsLogger(LogEventHandler logEvent)
        {
            _logEvent = logEvent;
        }

        public void Log(Exception ex, string Level, int UserID)
        {
            _logEvent(ex, Level, UserID);
        }
    }
}
