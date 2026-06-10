using Clinic_DataAccess;
using Clinic_DataAccess.SaveSqlException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Clinic_Business
{
    public class clsPrescription
    {
        public enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode = enMode.AddNew;

        public int PrescriptionID { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public int VisitID { get; set; }
        public string PrescriptionNotes { get; set; }

        public DataTable dtMedicines { get; set; }

        public clsPrescription()
        {
            this.PrescriptionID = -1;
            this.PrescriptionNotes = string.Empty;
            this.PrescriptionDate = DateTime.Now;
            this.VisitID = -1;
            this.dtMedicines = new DataTable();

            _Mode = enMode.AddNew;

        }

        public clsPrescription(int PrescriptionID, int VisitID, string PrescriptionNotes, DateTime PrescriptionDate)
        {
            this.PrescriptionID = PrescriptionID;
            this.PrescriptionNotes = PrescriptionNotes;
            this.VisitID = VisitID;
            this.PrescriptionDate = PrescriptionDate;
            this.dtMedicines = GetVisitMedicines();
            _Mode = enMode.Update;
        }


        private DataTable GetVisitMedicines()
        {

            return clsPrescriptionData.GetVisitMedicines(this.VisitID);
        }

        private bool _AddNewPrescription()
        {
            this.PrescriptionID = clsPrescriptionData.SavePrescription(VisitID, PrescriptionNotes, PrescriptionDate, dtMedicines);
            return this.PrescriptionID > 0;
        }
        private bool _UpdatePrescription()
        {
            return clsPrescriptionData.UpdatePrescription(PrescriptionID, VisitID, PrescriptionNotes, PrescriptionDate, dtMedicines);
        }

        public static clsPrescription Find(int VisitID)
        {
            int PrescriptionID = -1;

            DateTime PrescriptionDate = DateTime.Now;
            string PrescriptionNotes = string.Empty;

            if(clsPrescriptionData.Find(VisitID,ref PrescriptionID,ref PrescriptionNotes,ref PrescriptionDate))
                return new clsPrescription(PrescriptionID, VisitID, PrescriptionNotes,PrescriptionDate);
            else
                return null;
               

        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPrescription())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePrescription();
                default:
                    return false;
            }
        }
    }
}
