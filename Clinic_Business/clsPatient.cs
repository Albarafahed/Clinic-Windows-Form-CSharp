using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsPatient : clsPerson
    {
        public enum enMode { AddNew=1,Update=2}

        private enMode _Mode= enMode.AddNew;
        public int PatientID { get; set; }
        public string EmergencyContact { get; set; }
        public int BloodTypeID { get; set; }
        public clsBloodType BloodTypeInfo { get; set; }
        public string MedicalHistory { get; set; }

        public clsPatient()
        {
            this.PatientID = -1;
            this.EmergencyContact = "";
            this.BloodTypeID = -1;
            this.MedicalHistory = "";
            _Mode = enMode.AddNew;
        }

        public clsPatient(int PatientID,string emergencyContact,int bloodTypeID,string medicalHistory,int personID, string name, DateTime dateOfBirth,
                short gendor, string address, string phoneNumber, string email,int nationalityCountryID,string imagePath) 
                   
        {
            this.PatientID = PatientID;
            this.EmergencyContact = emergencyContact;
            this.BloodTypeID = bloodTypeID;
            this.BloodTypeInfo=clsBloodType.Find(bloodTypeID);
            this.MedicalHistory = medicalHistory;
            this.PersonID = personID;
            this.Name = name;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.ImagePath = imagePath;

            _Mode = enMode.Update;
        }

        private bool _AddNewPatient()
        {
            this.PatientID = clsPatinetData.AddNewPatient(this.PersonID, this.EmergencyContact, this.BloodTypeID, this.MedicalHistory);

            return this.PatientID > 0;
        }

        private bool _UpdatePatient()
        {
            return clsPatinetData.UpdatePatient(this.PatientID, this.PersonID, this.EmergencyContact, this.BloodTypeID, this.MedicalHistory);
        }

        public static bool Delete(int PatientID)
        {
            return clsPatinetData.DeletePatient(PatientID);
        }

        public static clsPatient Find(int PatientID)
        {
            string EmergencyContact = "";
            int BloodTypeID = -1;
            string MedicalHistory = "";
            int PersonID = -1;
            string Name = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = 0;
            string Address = "";
            string PhoneNumber = "";
            string Email = "";
            int NationalityCountryID = -1;
            string imagePath = "";

            if(clsPatinetData.GetPatientByPatientID(PatientID,ref PersonID,ref EmergencyContact, ref BloodTypeID,ref MedicalHistory))
            {
                if (clsPersonData.GetPersonByPersonID(PersonID, ref Name, ref DateOfBirth, ref Gendor, ref PhoneNumber, ref Email, ref Address, ref NationalityCountryID, ref imagePath))

                    return new clsPatient(PatientID, EmergencyContact, BloodTypeID, MedicalHistory, PersonID, Name, DateOfBirth, Gendor, Address, PhoneNumber, Email, NationalityCountryID, imagePath);

                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllPationts()
        {
           return clsPatinetData.GetAllPationtes();
        }

        public static DataRow GetPatientData(int PersonID)
        {
            return clsPatinetData.GetPationte(PersonID);
        }

        public bool Save()
        {
            base.Mode=(clsPerson.enMode)_Mode;
            if (!base.Save())
                return false;

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPatient())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePatient();
            }

            return false;
        }


    }
}
