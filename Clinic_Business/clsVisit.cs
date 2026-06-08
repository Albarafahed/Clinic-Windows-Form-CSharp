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
        public bool VisitStatus { get; set; }

        public clsVisit()
        {
            this.VisitID = -1;
            this.PatientID = -1;
            this.DoctorID = -1;
            this.AppointmentID = -1;
            this.VisitNotes = "";
            this.VisitStatus = false;
            this.Diagnosis = "";
            this.VisitDate = DateTime.Now;
            _Mode = enMode.AddNew;
        }

        private clsVisit(int VisitID, int PatientID, int DoctorID, int AppointmentID,
                         string Diagnosis, string VisitNotes, DateTime VisitDate, bool VisitStatus)
        {
            this.VisitID = VisitID;
            this.PatientID = PatientID;
            this.DoctorID = DoctorID;
            this.AppointmentID = AppointmentID;
            this.VisitNotes = VisitNotes;
            this.VisitStatus = VisitStatus;
            this.Diagnosis = Diagnosis;
            this.VisitDate = VisitDate;
            _Mode = enMode.Update;
        }

        private bool _AddNewVisit()
        {
            this.VisitID = clsVisitData.AddNewVisit(this.PatientID, this.Diagnosis, this.VisitDate,
                                                    this.AppointmentID, this.VisitNotes, this.DoctorID, this.VisitStatus);
            return (this.VisitID != -1);
        }

        private bool _UpdateVisit()
        {
            return clsVisitData.UpdateVisit(this.VisitID, this.Diagnosis, this.VisitNotes, this.VisitStatus);
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
            bool VisitStatus = false;

            if (clsVisitData.GetVisitByID(VisitID, ref PatientID, ref Diagnosis, ref VisitDate,
                                          ref AppointmentID, ref VisitNotes, ref DoctorID, ref VisitStatus))
            {
                return new clsVisit(VisitID, PatientID, DoctorID, AppointmentID, Diagnosis, VisitNotes, VisitDate, VisitStatus);
            }
            return null;
        }

        public static DataTable GetPatientsWaitingForDoctors(int DoctorID)
        {
            return clsVisitData.GetPatientsWaitingForDoctors(DoctorID);
        }
    }
}