using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clinic_Business
{
    public class clsAppointmentType
    {
        public enum enMode { AddNew=1, Update = 2 }
        private enMode _Mode= enMode.AddNew;
        public int AppointmentTypeID { get; set; }
        public string TypeName { get; set; }

        public float DefaultFees { get; set; }

        public clsAppointmentType()
        { 
            this.AppointmentTypeID = -1;
            this.TypeName = string.Empty;
            this.DefaultFees = 0.0f;
            _Mode = enMode.AddNew;
        }

        public clsAppointmentType(int appointmentTypeID, string typeName, float defaultFees)
        {
            this.AppointmentTypeID = appointmentTypeID;
            this.TypeName = typeName;
            this.DefaultFees = defaultFees;
            _Mode = enMode.Update;
        }

        private bool _AddAppointment()
        {
            this.AppointmentTypeID=clsAppointmentTypeData.AddAppointmentType(this.TypeName, this.DefaultFees);
            return this.AppointmentTypeID >0;
        }

        private bool _UpdateAppointment()
        {
            return clsAppointmentTypeData.UpdateAppointmentType(this.AppointmentTypeID, this.TypeName, this.DefaultFees);
        }

        public static bool Delete(int AppointmentTypeID)
        {
            return clsAppointmentTypeData.DeleteAppointmentType(AppointmentTypeID);
        }

        public static clsAppointmentType Find(int AppointmentTypeID)
        {
            string typeName = string.Empty;
            float defaultFees = 0.0f;

            if (Clinic_DataAccess.clsAppointmentTypeData.GetAppointmentTypeByID(AppointmentTypeID, ref typeName, ref defaultFees))
            {
                return new clsAppointmentType(AppointmentTypeID, typeName, defaultFees);
            }

            return null;
        }

        public static DataTable GetAllAppointmentType()
        {
            return clsAppointmentTypeData.GetAllAppintmentType();
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if(_AddAppointment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else 
                        return false;
                case enMode.Update:
                    return _UpdateAppointment();
            }
            return false;
        }
    }
}
