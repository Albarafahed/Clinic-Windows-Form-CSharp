using Clinic_DataAccess;
using System;
using System.Data;

namespace Clinic_Business
{
    public class clsAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int CreatedByUserID { get; set; }
        public short AppointmentStatus { get; set; }
        public int AppointmentTypeID { get; set; }
        public decimal AppointmentFees { get; set; }
        public DateTime AppointmentDate { get; set; }

        public clsAppointment()
        {
            this.AppointmentID = -1;
            this.PatientID = -1;
            this.DoctorID = -1;
            this.CreatedByUserID = -1;
            this.AppointmentStatus = 1; // Default: Scheduled
            this.AppointmentTypeID = -1;
            this.AppointmentFees = 0;
            this.AppointmentDate = DateTime.Now;
            Mode = enMode.AddNew;
        }

        private clsAppointment(int AppointmentID, int PatientID, int DoctorID, int CreatedByUserID,
            short AppointmentStatus, int AppointmentTypeID, decimal AppointmentFees, DateTime AppointmentDate)
        {
            this.AppointmentID = AppointmentID;
            this.PatientID = PatientID;
            this.DoctorID = DoctorID;
            this.CreatedByUserID = CreatedByUserID;
            this.AppointmentStatus = AppointmentStatus;
            this.AppointmentTypeID = AppointmentTypeID;
            this.AppointmentFees = AppointmentFees;
            this.AppointmentDate = AppointmentDate;
            Mode = enMode.Update;
        }

        public static clsAppointment Find(int AppointmentID)
        {
            int PatientID = -1, DoctorID = -1, CreatedByUserID = -1, AppointmentTypeID = -1;
            short AppointmentStatus = 1;
            decimal AppointmentFees = 0;
            DateTime AppointmentDate = DateTime.Now;

            if (clsAppointmentData.GetAppointmentByID(AppointmentID, ref PatientID, ref DoctorID, ref CreatedByUserID,
                ref AppointmentStatus, ref AppointmentTypeID, ref AppointmentFees, ref AppointmentDate))
            {
                return new clsAppointment(AppointmentID, PatientID, DoctorID, CreatedByUserID,
                    AppointmentStatus, AppointmentTypeID, AppointmentFees, AppointmentDate);
            }
            return null;
        }

        private bool _AddNewAppointment()
        {
            this.AppointmentID = clsAppointmentData.AddNewAppointment(this.PatientID, this.DoctorID, this.CreatedByUserID,
                this.AppointmentStatus, this.AppointmentTypeID, this.AppointmentFees, this.AppointmentDate);
            return (this.AppointmentID != -1);
        }

        private bool _UpdateAppointment()
        {
            return clsAppointmentData.UpdateAppointment(this.AppointmentID, this.PatientID, this.DoctorID, this.CreatedByUserID,
                this.AppointmentStatus, this.AppointmentTypeID, this.AppointmentFees, this.AppointmentDate);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateAppointment();
            }
            return false;
        }

        public static bool DeleteAppointment(int AppointmentID)
        {
            return clsAppointmentData.DeleteAppointment(AppointmentID);
        }

        public static DataTable GetAllAppointments()
        {
            return clsAppointmentData.GetAllAppointments();
        }

        public static DataRow GetAppointmentInfo(int AppointmentID)
        {
            return clsAppointmentData.GetAppointmentInfoByID(AppointmentID);
        }
        public static bool IsExists(int AppointmentID)
        {
            return clsAppointmentData.DoesAppointmentExist(AppointmentID);
        }
    }
}