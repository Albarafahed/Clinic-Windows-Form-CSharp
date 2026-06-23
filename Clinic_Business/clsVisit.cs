using Clinic_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Business
{
    public class clsVisit
    {
        public enum enMode { AddNew = 1, Update = 2 }

        public enMode _Mode = enMode.AddNew;
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string Diagnosis { get; set; }
        public DateTime VisitDate { get; set; }
        public int AppointmentID { get; set; }
        public string VisitNotes { get; set; }
        public int DoctorID { get; set; }
        public byte VisitStatus { get; set; }
        public int CreatedByUserID { get; set; }
        public decimal TotalAmount { get; set; }
        //public int PrescriptionID { get; set; }
        public clsPrescription PrescriptionInfo { get; set; }
        public DataTable dtServices { get; set; }
        public enum enVisitStatus
        {
            Active = 1,
            Pending_Billing = 2,
            Completed = 3
        }
        public clsVisit()
        {
            this.VisitID = -1;
            this.PatientID = -1;
            this.DoctorID = -1;
            this.AppointmentID = -1;
            this.VisitNotes = "";
            this.VisitStatus = 0;
            this.Diagnosis = "";
            this.VisitDate = DateTime.Now;
            this.PrescriptionInfo = new clsPrescription();
            //InitializeServicesTable();
            _Mode = enMode.AddNew;
        }

        private clsVisit(int VisitID, int PatientID, int DoctorID, int AppointmentID,
                         string Diagnosis, string VisitNotes, DateTime VisitDate,
                         byte VisitStatus, decimal TotalAmount, int CreatedByUserID)
        {
            this.VisitID = VisitID;
            this.PatientID = PatientID;
            this.DoctorID = DoctorID;
            this.AppointmentID = AppointmentID;
            this.VisitNotes = VisitNotes;
            this.VisitStatus = VisitStatus;
            this.Diagnosis = Diagnosis;
            this.VisitDate = VisitDate;
            this.TotalAmount = TotalAmount;
            this.CreatedByUserID = CreatedByUserID;
            PrescriptionInfo = clsPrescription.Find(VisitID);
            if (PrescriptionInfo == null)
                PrescriptionInfo = new clsPrescription();

            dtServices = clsVisitServices.GetVisitServices(VisitID);
            if (dtServices.Rows.Count > 0)
            {

                if (!dtServices.Columns.Contains("Total"))
                {

                    dtServices.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");
                }
            }
            else
            {
                InitializeServicesTable();
            }

            _Mode = enMode.Update;
        }

        //private bool _AddNewVisit()
        //{
        //    this.VisitID = clsVisitData.SaveVisitAndLinkVitals(AppointmentID, PatientID, Diagnosis, VisitNotes, DoctorID, VisitDate, CreatedByUserID, TotalAmount);
        //    return (this.VisitID != -1);
        //}

        //private bool _UpdateVisit()
        //{
        //    return clsVisitData.UpdateVisit(VisitID, Diagnosis, VisitNotes, VisitStatus, CreatedByUserID);
        //}


        public void InitializeServicesTable()
        {
            dtServices = new DataTable();
            dtServices.Columns.Clear(); // تنظيف الجدول أولاً

            dtServices.Columns.Add("ServiceID", typeof(int));
            dtServices.Columns.Add("ServiceName", typeof(string));
            dtServices.Columns.Add("Quantity", typeof(int));
            dtServices.Columns.Add("SavedServicePrice", typeof(decimal));
            dtServices.Columns.Add("Discount", typeof(decimal));
            dtServices.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");


        }


        private bool _AddNewVisit()
        {
            this.PrescriptionInfo.PrescriptionID = clsVisitData.AddNewVisitData(this.VisitID, this.AppointmentID, this.Diagnosis, this.VisitNotes, this.CreatedByUserID, this.TotalAmount, this.DoctorID, this.dtServices,
                        this.PrescriptionInfo.dtMedicines, this.PrescriptionInfo.PrescriptionNotes, this.PrescriptionInfo.PrescriptionDate);
            return (this.PrescriptionInfo.PrescriptionID != -1);
        }

        private bool _UpdateVisit()
        {
            int tempPrescriptionID = this.PrescriptionInfo.PrescriptionID;
            bool isSaved = clsVisitData.UpdateExistingVisitData(this.VisitID, this.AppointmentID, this.Diagnosis, this.VisitNotes, this.CreatedByUserID, this.TotalAmount, this.DoctorID, this.dtServices,
                        this.PrescriptionInfo.dtMedicines, ref tempPrescriptionID, this.PrescriptionInfo.PrescriptionNotes, this.PrescriptionInfo.PrescriptionDate);

            if (isSaved)
            {
                this.PrescriptionInfo.PrescriptionID = tempPrescriptionID;
            }

            return isSaved;
        }

        public static bool DeleteVisit(int VisitID, int AppointmentID)
        {
            return clsVisitData.DeleteVisit(VisitID, AppointmentID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewVisit())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateVisit();
            }
            return false;
        }

        public static clsVisit Find(int VisitID)
        {
            int PatientID = -1, AppointmentID = -1, DoctorID = -1;
            string Diagnosis = "", VisitNotes = "";
            DateTime VisitDate = DateTime.Now;
            byte VisitStatus = 0;
            decimal TotalAmount = 0.0m;
            int CreatedByUserID = -1;

            if (clsVisitData.GetVisitByID(VisitID, ref PatientID, ref Diagnosis, ref VisitDate, ref AppointmentID, ref VisitNotes, ref DoctorID, ref VisitStatus, ref TotalAmount, ref CreatedByUserID))
            {
                return new clsVisit(VisitID, PatientID, DoctorID, AppointmentID, Diagnosis, VisitNotes, VisitDate, VisitStatus, TotalAmount, CreatedByUserID);
            }
            return null;
        }

        public static bool IsVisitClosed(int VisitID)
        {
            return clsVisitData.IsVisitClosed(VisitID);
        }

        public static DataTable GetPatientsWaitingForDoctors(int DoctorID)
        {
            return clsVisitData.GetPatientsWaitingForDoctors(DoctorID);
        }

        public static DataTable GetAllVisits()
        {
            return clsVisitData.GetAllVisits();
        }


        ////public static bool SaveVisitDataWithDetails(string Diagnosis,string VisitNotes, DataTable services, clsPrescription prescription, DataTable medicines, SqlTransaction tran)
        //{
        //    // 1. تحديث بيانات الزيارة الأساسية
        //    if (!visit.Save()) return false;

        //    // 2. معالجة الخدمات (Services)
        //    // إذا كان الـ DataTable فارغاً، نحذف كل الخدمات المرتبطة بهذه الزيارة
        //    if (services == null || services.Rows.Count == 0)
        //    {
        //        clsVisitServices.DeleteVisitServices(visit.VisitID, tran);
        //    }
        //    else
        //    {
        //        // إذا كان هناك بيانات، نحذف القديم ثم نضيف الجديد (أو نستخدم منطق Update)
        //        clsVisitServices.DeleteVisitServices(visit.VisitID, tran);
        //        if (!clsVisitServices.SaveVisitServices(visit.VisitID, services, tran)) return false;
        //    }

        //    // 3. معالجة الأدوية (Prescription)
        //    if (medicines == null || medicines.Rows.Count == 0)
        //    {
        //        // حذف الوصفة بالكامل إذا تم إفراغها
        //        clsPrescription.DeletePrescription(prescription.PrescriptionID, tran);
        //    }
        //    else
        //    {
        //        prescription.VisitID = visit.VisitID;
        //        prescription.dtMedicines = medicines;
        //        if (!prescription.Save(tran)) return false;
        //    }

        //    return true;
        //}
    }
}