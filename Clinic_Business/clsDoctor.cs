using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsDoctor : clsPerson
    {
        public enum enMode { AddNew=1,Update=2}
        private enMode _Mode = enMode.AddNew;
        public int DoctorID { get; set; }

        public float ConsultationFees { get; set; }

        public string DaysOfWork { get; set; }

        public string SpecializationsName { get; set; }

        public List<int> SelectedSpecialtyIDs { get; set; }

        public List<int> SelectedDayIDs { get; set; }

        public clsDoctor()
        {
            this.DoctorID = -1;
    
            DaysOfWork= string.Empty;
            _Mode = enMode.AddNew;

        }

        public clsDoctor(int DoctorID, int SpecializationID, float ConsultationFees, string DaysOfWork)
        {
            this.DoctorID = DoctorID;
            this.ConsultationFees = ConsultationFees;
            this.DaysOfWork = DaysOfWork;
            _Mode = enMode.Update;
        }

        public static clsDoctor Find(int DoctorID)
        {
            return new clsDoctor();
        }

        public bool DeleteDoctor()
        {
            return true;
        }
       


    }
}
