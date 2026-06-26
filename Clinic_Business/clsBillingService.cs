using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsBillingService
    {
        public static bool CheckInPatient(clsAppointment Appointment, int UserID, string PaymentMethod)
        {
            return clsBillingServiceData.CheckInPatient(Appointment.AppointmentID, Appointment.DoctorID, Appointment.PatientID, Appointment.AppointmentFees, UserID, PaymentMethod);
        }

        public enum enPaymetnStatus:byte
        {
            Pending = 0, 
            Partial = 1, 
            Paid = 2,
            Refunded = 3,
            Cancelled = 4
        }

        public static bool UpdatePaymentStatus(int BillID,enPaymetnStatus Status)
        {
            return clsBillingServiceData.UpdatePaymentStatus(BillID, (byte)Status);
        }

        public static DataTable GetAllBills()
        {
            return clsBillingServiceData.GetAllBills();
        }
    }
}
