using Clinic_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Business
{
    public class clsMedicine
    {
        public static bool IsMedicineAvailable(int medicineId, int requestedQuantity)
        {
            return clsMedicineData.IsMedicineAvailable(medicineId, requestedQuantity);
        }

        public static DataTable GetAllMedicines()
        {
            return clsMedicineData.GetAllMedicines();
        }
    }
}
