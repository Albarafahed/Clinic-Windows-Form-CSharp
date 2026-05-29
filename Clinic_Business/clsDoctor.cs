using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        public enum enDayOfWeek
        {
            Saturday = 1,
            Sunday = 2,
            Monday = 3,
            Tuesday = 4,
            Wednesday = 5,
            Thursday = 6,
            Friday = 7
        }
        public int DoctorID { get; set; }

        public float ConsultationFees { get; set; }

        public List<int> SelectedSpecialtyIDs { get; set; }
        public string Specializations { get; set; }
        public List<int> SelectedDayIDs { get; set; }
        public string WorkingDays { get; set; }

        public bool IsActive { get; set; }

        public string LicenseNumber { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public clsDoctor()
        {
            this.DoctorID = -1;
            this.ConsultationFees = 0;
            this.SelectedSpecialtyIDs = new List<int>();
            this.SelectedDayIDs = new List<int>();
            this.IsActive= true;
            this.LicenseNumber = string.Empty;
            this.CreatedByUserID = -1;
            _Mode = enMode.AddNew;

        }

        public clsDoctor(int DoctorID,float ConsultationFees, string LicenseNumber,
                          int CreatedByUserID,bool IsActive, DateTime CreatedDate,

                          int PersonID, string Name, DateTime DateOfBirth,
                          short Gender, string Address, string PhoneNumber, 
                          string Email,int NationalityCountryID, string ImagePath)
        {
            this.DoctorID = DoctorID;
            this.ConsultationFees = ConsultationFees;
            this.LicenseNumber = LicenseNumber;
            this.CreatedByUserID = CreatedByUserID;
            this.IsActive = IsActive;
            this.CreatedDate = CreatedDate;
            this.SelectedSpecialtyIDs = clsDoctorData.GetDoctorSpecialtyIDs(DoctorID);
            this.Specializations = clsDoctorData.GetSpecializations(DoctorID);
            this.SelectedDayIDs=clsDoctorData.GetDoctorWorkingDayIDs(DoctorID);
            this.WorkingDays = clsDoctorData.GetWorkingDays(DoctorID);


            this.PersonID = PersonID;
            this.Name = Name;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.PhoneNumber = PhoneNumber;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

            _Mode = enMode.Update;
        }

        private bool _AddNewDoctor()
        {
            this.DoctorID = clsDoctorData.AddNewDoctor
                (this.PersonID, this.ConsultationFees, 
                 this.LicenseNumber, this.CreatedByUserID,
                 this.IsActive, this.SelectedDayIDs, this.SelectedSpecialtyIDs);
            return this.DoctorID > 0;
        }

        private bool _UpdateDoctor()
        {
            return clsDoctorData.UpdateDoctor
                (this.DoctorID,this.PersonID, this.ConsultationFees,
                 this.LicenseNumber, this.CreatedByUserID,
                 this.IsActive, this.SelectedDayIDs, this.SelectedSpecialtyIDs);
        }


        public bool DeleteDoctor()
        {
            return clsDoctorData.DeleteDoctor(this.DoctorID);
        }

        public static clsDoctor Find(int DoctorID)
        {
           float ConsultationFees = 0;
            DateTime CreatedDate = DateTime.Now;
            int CreatedByUserID = -1;
            bool IsActive = true;
            string LicenseNumber = "";

            int PersonID = -1;
            string Name = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            string Address = "";
            string PhoneNumber = "";
            string Email = "";
            int NationalityCountryID = -1;
            string ImagePath = "";

            if (clsDoctorData.GetDoctorByID(DoctorID, ref PersonID, ref ConsultationFees, ref LicenseNumber, ref IsActive, ref CreatedDate, ref CreatedByUserID))
            {
                if (clsPersonData.GetPersonByPersonID(PersonID, ref Name, ref DateOfBirth, ref Gender, ref PhoneNumber, ref Email, ref Address, ref NationalityCountryID, ref ImagePath))
                {
                    return new clsDoctor(DoctorID, ConsultationFees, LicenseNumber, CreatedByUserID, IsActive, CreatedDate, PersonID, Name, DateOfBirth, Gender, Address, PhoneNumber, Email, NationalityCountryID, ImagePath);
                }
            }
            return null;
        }

        public static DataTable GetAllDoctors()
        {
            // تم تصحيح استدعاء الدالة لتطابق اسم GetAllPatients المصحح
            return clsDoctorData.GetAllDoctors();
        }


        public static DataRow GetDoctorByID(int DoctorID)
        {
            return clsDoctorData.GetDoctorByID(DoctorID);
        }

        public static bool IsDoctorExistForPersonID(int PersonID)
        {
            // استدعاء الدالة مباشرة من طبقة البيانات
            return clsDoctorData.IsDoctorExistForPersonID(PersonID);
        }

        public bool Save()
        {


            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDoctor())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateDoctor();
            }

            return false;
        }



    }
}
