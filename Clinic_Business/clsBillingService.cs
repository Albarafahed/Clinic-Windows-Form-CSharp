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
        public static bool CheckInPatient(clsAppointment Appointment, int UserID, string PaymentMethod)
        {
            return clsBillingServiceData.CheckInPatient(Appointment.AppointmentID, Appointment.DoctorID, Appointment.PatientID, Appointment.AppointmentFees, UserID, PaymentMethod);
        }
    }
}
