using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsUser:clsPerson
    {
        public enum enMode { AddNew=1, Update=2}
        private enMode _Mode=enMode.AddNew;

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int RoleID { get; set; }

       public clsRole RoleInfo { get; set; }

        public clsUser() 
        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
            this.RoleID = -1 ;
             _Mode = enMode.AddNew;
        }

        public clsUser (int UserID,string UserName, string Password,bool IsActive,int RoleID,
            int personID,string name, DateTime dateOfBirth, short Gender, string address,
            string phoneNumber, string email, int nationalityCountryID, string imagePath)
        {

              //Inisualized By Parent
            this.PersonID = personID;
            this.Name = name;
            this.DateOfBirth = dateOfBirth;
            this.Gender = Gender;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.ImagePath = imagePath;


            this.UserID=UserID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.RoleID = RoleID;
            this.RoleInfo=clsRole.FindByID(RoleID);
            _Mode = enMode.Update;
        }

        public static clsUser Find(int UserID)
        {
            int PersonID = -1; string FullName = ""; DateTime DateOfBirth = DateTime.Now;

            short Gender = 0; string Address = ""; string PhoneNumber = "";
            string Email = ""; int NationalityCountryID = -1; string ImagePath = "";

            string UserName = "";  string Password = ""; bool IsActive = false;
            int RoleID = -1;

            if(clsUserData.Find(UserID,ref PersonID,ref UserName ,ref Password,ref IsActive,ref RoleID))
            {
                if(clsPersonData.GetPersonByPersonID(PersonID,ref FullName,ref DateOfBirth,ref Gender,ref PhoneNumber,ref Email,ref Address,ref NationalityCountryID,ref ImagePath))
                
                    return new clsUser(UserID, UserName, Password, IsActive, RoleID, PersonID, FullName, DateOfBirth, Gender, Address, PhoneNumber, Email, NationalityCountryID, ImagePath);

                
            }

            return null;
        }

        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1; string FullName = ""; DateTime DateOfBirth = DateTime.Now;

            short Gender = 0; string Address = ""; string PhoneNumber = "";
            string Email = ""; int NationalityCountryID = -1; string ImagePath = "";

            string UserName = ""; string Password = ""; bool IsActive = false;
            int RoleID = -1;

            if (clsUserData.FindByPersonID(PersonID, ref UserID, ref UserName, ref Password, ref IsActive, ref RoleID))
            {
                if (clsPersonData.GetPersonByPersonID(PersonID, ref FullName, ref DateOfBirth, ref Gender, ref PhoneNumber, ref Email, ref Address, ref NationalityCountryID, ref ImagePath))

                    return new clsUser(UserID, UserName, Password, IsActive, RoleID, PersonID, FullName, DateOfBirth, Gender, Address, PhoneNumber, Email, NationalityCountryID, ImagePath);


            }

            return null;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive, this.RoleID);
            return this.UserID > 0;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID,this.PersonID, this.UserName, this.Password, this.IsActive, this.RoleID);
        }

        public  bool DeleteUser()
        {
            bool IsUserDeleted = false;
            bool IsBasePersonDeleted = false;

            IsUserDeleted=clsUserData.DeleteUser(this.UserID);

            if (!IsUserDeleted)
                return false;
            IsBasePersonDeleted= base.Delete();

            return IsBasePersonDeleted;

        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }

        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static DataRow GetUserByID(int UserID)
        {
            return clsUserData.GetUserByID(UserID);
        }

        public static string GetFullName(int UserID)
        {
            return clsUserData.GetFullName(UserID);
        }
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if(_AddNewUser())
                    {
                        _Mode = enMode.Update;
                        return true;

                    }
                    else
                        return false;   
                case enMode.Update:
                    return _UpdateUser();
                       
            }

            return false;
        }

        

    }

}
