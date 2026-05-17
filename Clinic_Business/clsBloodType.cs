using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsBloodType
    {
        public enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode = enMode.AddNew;
        public int BloodTypeID { get; set; }
        public string BloodTypeName { get; set; }

        public clsBloodType()
        {
            this.BloodTypeID = -1;
            this.BloodTypeName = "";
            _Mode = enMode.AddNew;
        }

        public clsBloodType(int bloodTypeID, string bloodTypeName)
        {
            this.BloodTypeID = bloodTypeID;
            this.BloodTypeName = bloodTypeName;
            _Mode = enMode.Update;
        }

        public static clsBloodType Find(int BloodtTypeID)
        {
            string BloodTypeName = "";

            if (clsBloodTypeData.FindBloodTypeByID(BloodtTypeID, ref BloodTypeName))
            {
                return new clsBloodType(BloodtTypeID, BloodTypeName);
            }

            return null;
        }

        public static clsBloodType Find(string BloodTypeName)
        {
            int BloodTypeID = -1;

            if (clsBloodTypeData.FindBloodTypeByName(BloodTypeName, ref BloodTypeID))
            {
                return new clsBloodType(BloodTypeID, BloodTypeName);
            }

            return null;
        }

        public static DataTable GetAllBloodTypes()
        {
            return clsBloodTypeData.GetAllBloodTypes();
        }
    }
}
