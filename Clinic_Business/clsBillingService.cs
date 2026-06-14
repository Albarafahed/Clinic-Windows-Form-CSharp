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
        public static bool CheckInPatient(int AppointmentID, decimal Fees,decimal Discount,  int UserID, string PaymentMethod)
        {
            return clsBillingServiceData.CheckInPatient(AppointmentID, Fees, Discount, UserID, PaymentMethod);
        }
    }
}
