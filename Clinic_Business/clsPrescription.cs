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

        public byte PrescriptionStatu { get; set; }
        public enum PrescriptionStatus : byte
        {
            Pending = 1,
            WaitingForPayment = 2,
            ReadyForDispensing = 3,
            Dispensed = 4,
            PartiallyDispensed = 5,
            Cancelled = 6
        }

        public DataTable dtMedicines { get; set; }

        public clsPrescription()
        {
            this.PrescriptionID = -1;
            this.PrescriptionNotes = string.Empty;
            this.PrescriptionDate = DateTime.Now;
            this.VisitID = -1;
            this.dtMedicines = new DataTable();
            this.PrescriptionStatu =(byte) PrescriptionStatus.Pending;
            _Mode = enMode.AddNew;

        }

        public clsPrescription(int PrescriptionID, int VisitID, string PrescriptionNotes, DateTime PrescriptionDate,byte PrescriptionStatus)
        {
            this.PrescriptionID = PrescriptionID;
            this.PrescriptionNotes = PrescriptionNotes;
            this.VisitID = VisitID;
            this.PrescriptionDate = PrescriptionDate;
            this.PrescriptionStatu = PrescriptionStatus;
            this.dtMedicines = GetVisitMedicines();
            _Mode = enMode.Update;
        }

        private bool _AddNewPrescription()
        {
            this.PrescriptionID = clsPrescriptionData.SavePrescription(VisitID, PrescriptionNotes, PrescriptionDate, dtMedicines,(byte)PrescriptionStatus.Pending);
            return this.PrescriptionID > 0;
        }
        private bool _UpdatePrescription()
        {
            return clsPrescriptionData.UpdatePrescription(PrescriptionID, VisitID, PrescriptionNotes, PrescriptionDate,PrescriptionStatu, dtMedicines);
        }

        public static clsPrescription Find(int VisitID)
        {
            int PrescriptionID = -1;

            DateTime PrescriptionDate = DateTime.Now;
            string PrescriptionNotes = string.Empty;
            byte PrescriptionStatus = 1;
            if (clsPrescriptionData.Find(VisitID,ref PrescriptionID,ref PrescriptionNotes,ref PrescriptionDate,ref PrescriptionStatus))
                return new clsPrescription(PrescriptionID, VisitID, PrescriptionNotes,PrescriptionDate,PrescriptionStatus);
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

        private DataTable GetVisitMedicines()
        {

            return clsPrescriptionData.GetVisitMedicines(this.VisitID);
        }

        
    }
}
