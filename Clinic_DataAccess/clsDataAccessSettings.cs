using System.Configuration;

namespace Clinic_DataAccess
{
    public class clsDataAccessSettings
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
    }
}
