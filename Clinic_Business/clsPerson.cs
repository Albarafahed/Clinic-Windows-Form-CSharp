using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Clinic_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew = 1, Update = 2 }

        public enMode Mode = enMode.AddNew;
        public int PersonID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { set; get; }
        public string Address { set; get; }
        public string PhoneNumber { set; get; }
        public string Email { set; get; }

        public int NationalityCountryID { set; get; }

        public clsCountry CountryInfo { set; get; }
        public string ImagePath { set; get; }

        private clsPerson(int personID, string name, DateTime dateOfBirth, short gendor, string address, string phoneNumber, string email, int nationalityCountryID, string imagePath)
        {
            this.PersonID = personID;
            this.Name = name;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            CountryInfo = clsCountry.Find(nationalityCountryID);
            Mode = enMode.Update;
            ImagePath = imagePath;
        }

        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalityCountryID = -1;
            this.Name = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 0;
            this.Address = "";
            this.PhoneNumber = "";
            this.Email = "";
            this.ImagePath = "";
            Mode = enMode.AddNew;

        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(Name, DateOfBirth, Gendor, PhoneNumber, Email, Address, ImagePath, NationalityCountryID);
            return this.PersonID > 0;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PersonID, Name, DateOfBirth, Gendor, PhoneNumber, Email, Address, ImagePath, NationalityCountryID);
        }

        public bool Delete()
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static clsPerson Find(int PersonID)
        {
            string name = "";
            DateTime dateOfBirth = DateTime.Now;
            short gendor = 0;
            string address = "";
            string phoneNumber = "";
            string email = "";
            int nationalcontry = -1;
            string imagePath = "";

          if(clsPersonData.GetPersonByPersonID(PersonID, ref name, ref dateOfBirth, ref gendor, ref phoneNumber, ref email, ref address, ref nationalcontry, ref imagePath))
            {
                return new clsPerson(PersonID, name, dateOfBirth, gendor, address, phoneNumber, email, nationalcontry, imagePath);
            }
            else
            {
                return null;
            }



        }

        public static DataTable GetAllPersons()
        {
            return clsPersonData.GetAllPersons();
        }

      
        public static bool SoftDelete(int PersonID)
        {
            return clsPersonData.SoftDeletePerson(PersonID);
        }

        public static bool Restore(int PersonID)
        {
            return clsPersonData.RestorePerson(PersonID);
        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }
    }
}
