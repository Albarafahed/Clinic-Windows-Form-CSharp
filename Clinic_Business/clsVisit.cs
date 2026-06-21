using Clinic_DataAccess;
using System;
using System.Data;

namespace Clinic_Business
{
    public class clsVisit
    {
        public enum enMode { AddNew = 1, Update = 2 }

        public enMode _Mode = enMode.AddNew;
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string Diagnosis { get; set; }
        public DateTime VisitDate { get; set; }
        public int AppointmentID { get; set; }
        public string VisitNotes { get; set; }
        public int DoctorID { get; set; }
        public byte VisitStatus { get; set; }
        public int CreatedByUserID { get; set; }
        public decimal TotalAmount { get; set; }
        public enum enVisitStatus
        {
            Active = 1,
            Pending_Billing = 2,
            Completed = 3
        }
        public clsVisit()
        {
            this.VisitID = -1;
            this.PatientID = -1;
            this.DoctorID = -1;
            this.AppointmentID = -1;
            this.VisitNotes = "";
            this.VisitStatus = 0;
            this.Diagnosis = "";
            this.VisitDate = DateTime.Now;
            _Mode = enMode.AddNew;
        }

        private clsVisit(int VisitID, int PatientID, int DoctorID, int AppointmentID,
                         string Diagnosis, string VisitNotes, DateTime VisitDate,
                         byte VisitStatus, decimal TotalAmount, int CreatedByUserID)
        {
            this.VisitID = VisitID;
            this.PatientID = PatientID;
            this.DoctorID = DoctorID;
            this.AppointmentID = AppointmentID;
            this.VisitNotes = VisitNotes;
            this.VisitStatus = VisitStatus;
            this.Diagnosis = Diagnosis;
            this.VisitDate = VisitDate;
            this.TotalAmount = TotalAmount;
            this.CreatedByUserID = CreatedByUserID;
            _Mode = enMode.Update;
        }

        private bool _AddNewVisit()
        {
            this.VisitID = clsVisitData.SaveVisitAndLinkVitals(AppointmentID, PatientID, Diagnosis, VisitNotes, DoctorID, VisitDate, CreatedByUserID, TotalAmount);
            return (this.VisitID != -1);
        }

        private bool _UpdateVisit()
        {
            return clsVisitData.UpdateVisit(VisitID, Diagnosis, VisitNotes, VisitStatus, CreatedByUserID);
        }

        public static bool DeleteVisit(int VisitID,int AppointmentID)
        {
            return clsVisitData.DeleteVisit(VisitID, AppointmentID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewVisit())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateVisit();
            }
            return false;
        }

        public static clsVisit Find(int VisitID)
        {
            int PatientID = -1, AppointmentID = -1, DoctorID = -1;
            string Diagnosis = "", VisitNotes = "";
            DateTime VisitDate = DateTime.Now;
            byte VisitStatus = 0;
            decimal TotalAmount = 0.0m;
            int CreatedByUserID = -1;

            if (clsVisitData.GetVisitByID(VisitID,ref PatientID,ref Diagnosis,ref VisitDate,ref AppointmentID,ref VisitNotes,ref DoctorID,ref VisitStatus,ref TotalAmount,ref CreatedByUserID))
            {
                return new clsVisit(VisitID, PatientID, DoctorID, AppointmentID, Diagnosis, VisitNotes, VisitDate, VisitStatus,TotalAmount,CreatedByUserID);
            }
            return null;
        }

         public static bool IsVisitClosed(int VisitID)
        {
            return clsVisitData.IsVisitClosed(VisitID);
        }

        public static DataTable GetPatientsWaitingForDoctors(int DoctorID)
        {
            return clsVisitData.GetPatientsWaitingForDoctors(DoctorID);
        }

        public static DataTable GetAllVisits()
        {
            return clsVisitData.GetAllVisits();
        }
    }
}