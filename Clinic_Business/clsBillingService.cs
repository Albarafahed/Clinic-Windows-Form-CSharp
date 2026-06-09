using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsBillingService
    {
        public static bool CheckInPatient(int AppointmentID, decimal Fees, int UserID, string PaymentMethod)
        {
          return  clsAppointmentData.CheckInPatient(AppointmentID, Fees, UserID, PaymentMethod);
        }
    }
}
