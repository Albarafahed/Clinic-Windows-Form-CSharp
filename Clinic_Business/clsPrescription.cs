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

        public byte PrescriptionStatus { get; set; }
        public enum enPrescriptionStatus : byte
        {
            Pending = 1,
            WaitingForPayment = 2,
            ReadyForDispensing = 3,
            Dispensed = 4,
            PartiallyDispensed = 5,
            Cancelled = 6
        }

        public byte Prescriptiontype { get; set; }

        public enum enPrescriptionType : byte
        {
            DoctorPrescription = 1,  // وصفة قادمة من الطبيب
            PharmacyDirect = 2       // وصفة يتم إنشاؤها مباشرة في الصيدلية
        }

        public DataTable dtMedicines { get; set; }

        public clsPrescription()
        {
            this.PrescriptionID = -1;
            this.PrescriptionNotes = string.Empty;
            this.PrescriptionDate = DateTime.Now;
            this.VisitID = -1;
            this.dtMedicines = new DataTable();
            this.PrescriptionStatus = (byte)enPrescriptionStatus.Pending;
            _Mode = enMode.AddNew;

        }

        public clsPrescription(int PrescriptionID, int VisitID, string PrescriptionNotes, DateTime PrescriptionDate, byte PrescriptionStatus)
        {
            this.PrescriptionID = PrescriptionID;
            this.PrescriptionNotes = PrescriptionNotes;
            this.VisitID = VisitID;
            this.PrescriptionDate = PrescriptionDate;
            this.PrescriptionStatus = PrescriptionStatus;
            this.dtMedicines = GetVisitMedicines();
            _Mode = enMode.Update;
        }

        private bool _AddNewPrescription()
        {
            this.PrescriptionID = clsPrescriptionData.SavePrescription(VisitID, PrescriptionNotes, PrescriptionDate, dtMedicines, (byte)enPrescriptionStatus.Pending,this.Prescriptiontype);
            return this.PrescriptionID > 0;
        }
        private bool _UpdatePrescription()
        {
            return clsPrescriptionData.UpdatePrescription(PrescriptionID, VisitID, PrescriptionNotes, PrescriptionDate, PrescriptionStatus, dtMedicines);
        }

        public static clsPrescription Find(int VisitID)
        {
            int PrescriptionID = -1;

            DateTime PrescriptionDate = DateTime.Now;
            string PrescriptionNotes = string.Empty;
            byte PrescriptionStatus = 1;
            if (clsPrescriptionData.Find(VisitID, ref PrescriptionID, ref PrescriptionNotes, ref PrescriptionDate, ref PrescriptionStatus))
                return new clsPrescription(PrescriptionID, VisitID, PrescriptionNotes, PrescriptionDate, PrescriptionStatus);
            else
                return null;


        }

        public static DataTable GetAllActivePrescriptions()
        {
            return clsPrescriptionData.GetAllActivePrescriptions();
        }

        public static DataTable GetAllPrescriptionDetails()
        {
            return clsPrescriptionData.GetAllPrescriptionDetails();
        }

        public static DataTable GetAllPrescriptionRecords()
        {
            return clsPrescriptionData.GetAllPrescriptionRecords();
        }

        public static DataTable GetPrescriptionItemsRaw()
        {
            return clsPrescriptionData.GetPrescriptionItemsRaw();
        }

        public static bool UpdatePrescriptionStatus(int prescriptionId, enPrescriptionStatus newStatus)
        {
            return clsPrescriptionData.UpdatePrescriptionStatus(prescriptionId,(byte) newStatus);
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

        public static decimal GetPrescriptionTotalForCashier(int PrescriptionID)
        {
            return clsPrescriptionData.GetPrescriptionTotalForCashier(PrescriptionID);
        }

        public static bool SendToCashier(int PrescriptionID,DataTable dtDispensedItems,int ?VisitID,int ? AppointmentID,decimal TotalMedicinesAmount, decimal TaxRate, int userId)
        {
            return clsPrescriptionData.SendToCashier(PrescriptionID, dtDispensedItems,VisitID,AppointmentID, TotalMedicinesAmount,TaxRate, userId);
        }


    }
}
