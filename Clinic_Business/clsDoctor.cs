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
        public class clsDoctorShift
        {
            public int DayID { get; set; }
            public string DayName { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
          
        }   

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
        public List<clsDoctorShift> DoctorShifts { get; set; }
      
        public bool IsActive { get; set; }

        public string LicenseNumber { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public clsDoctor()
        {
            this.DoctorID = -1;
            this.ConsultationFees = 0;
            this.SelectedSpecialtyIDs = new List<int>();
            this.DoctorShifts = new List<clsDoctorShift>();
            this.IsActive = true;
            this.LicenseNumber = string.Empty;
            this.CreatedByUserID = -1;
            _Mode = enMode.AddNew;

        }

        public clsDoctor(int DoctorID, float ConsultationFees, string LicenseNumber,
                          int CreatedByUserID, bool IsActive, DateTime CreatedDate,

                          int PersonID, string Name, DateTime DateOfBirth,
                          short Gender, string Address, string PhoneNumber,
                          string Email, int NationalityCountryID, string ImagePath)
        {
            this.DoctorID = DoctorID;
            this.ConsultationFees = ConsultationFees;
            this.LicenseNumber = LicenseNumber;
            this.CreatedByUserID = CreatedByUserID;
            this.IsActive = IsActive;
            this.CreatedDate = CreatedDate;


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
            // فك الكائنات إلى مصفوفات أولية قبل الإرسال لطبقة البيانات
            List<int> dayIDs = this.DoctorShifts.Select(s => s.DayID).ToList();
            List<TimeSpan> starts = this.DoctorShifts.Select(s => s.StartTime).ToList();
            List<TimeSpan> ends = this.DoctorShifts.Select(s => s.EndTime).ToList();

            this.DoctorID = clsDoctorData.AddNewDoctor(
                this.PersonID, this.ConsultationFees, this.LicenseNumber,
                this.CreatedByUserID, this.IsActive, this.SelectedSpecialtyIDs,
                dayIDs, starts, ends); // تمرير المصفوفات

            return this.DoctorID > 0;
        }

        private bool _UpdateDoctor()
        {
            List<int> dayIDs = this.DoctorShifts.Select(s => s.DayID).ToList();
            List<TimeSpan> starts = this.DoctorShifts.Select(s => s.StartTime).ToList();
            List<TimeSpan> ends = this.DoctorShifts.Select(s => s.EndTime).ToList();

            return clsDoctorData.UpdateDoctor(
                this.DoctorID, this.PersonID, this.ConsultationFees,
                this.LicenseNumber, this.CreatedByUserID, this.IsActive,
                this.SelectedSpecialtyIDs, dayIDs, starts, ends);
        }

        public string GetSpecializations()
        {
            return clsDoctorData.GetSpecializations(this.DoctorID);
        }

        public static string WorkingDays(int DoctorID)
        {
            return clsDoctorData.GetWorkingDays(DoctorID);

        }
        public  string GetWorkingDays()
        {
            return clsDoctorData.GetWorkingDays(this.DoctorID);
        }

        public static float GetConsultationFees(int DoctID)
        {
            return clsDoctorData.GetConsultationFees(DoctID);
        }
        public  List<int> GetDoctorSpecializations()
        {
            return clsDoctorData.GetDoctorSpecialtyIDs(this.DoctorID);
        }
        public static DataTable GetDoctorShifts(int DoctorID)
        {
            return clsDoctorData.GetDoctorShifts(DoctorID);
        }
      

        public static bool DeleteDoctor(int DoctorID)
        {
            return clsDoctorData.DeleteDoctor(DoctorID);
        }

        public static clsDoctor FindDoctorByID(int DoctorID)
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
            return clsDoctorData.GetAllDoctors();
        }


        public static DataRow GetDoctorByID(int DoctorID)
        {
            return clsDoctorData.GetDoctorByID(DoctorID);
        }

        public static bool IsDoctorExistForPersonID(int PersonID)
        {
            return clsDoctorData.IsDoctorExistForPersonID(PersonID);
        }

        public bool SaveDoctor()
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

        public static bool IsDoctorWorkingOnThisDay(int DoctorID, DateTime time)
        {
            return clsDoctorData.IsDoctorWorkingOnThisDay(DoctorID, time);
        }

        public static bool IsDoctorWorkingAtThisTime(int DoctorID, DateTime AppointmentDateTime)
        {
            return clsDoctorData.IsDoctorWorkingAtThisTime(DoctorID, AppointmentDateTime);
        }

        public static DataTable GetAllDays()
        {
            return clsDoctorData.GetAllDays();
        }

        public static DataTable LoadQueueData()
        {
            return clsDoctorData.LoadQueueData();
        }


    }
}
