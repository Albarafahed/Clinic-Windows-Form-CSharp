using Clinic_Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.global_classes
{
    public static class MyExtensions
    {
        // دالة واحدة تقوم إما بتحديث الصف إذا وجدته، أو إضافته إذا لم يكن موجوداً
        public static void UpsertRow(this DataTable dt, DataRow newRow, int primaryKeyValue)
        {
            DataRow existingRow = dt.Rows.Find(primaryKeyValue);

            if (existingRow != null)
            {
                // تحديث (Update)
                foreach (DataColumn col in dt.Columns)
                {
                    // شرط إضافي: تحقق هل العمود قابل للكتابة؟
                    if (col.ReadOnly) continue;

                    if (newRow.Table.Columns.Contains(col.ColumnName))
                    {
                        existingRow[col.ColumnName] = newRow[col.ColumnName];
                    }
                }
            }
            else
            {
                // إضافة (Add)
                dt.ImportRow(newRow);
            }

            dt.AcceptChanges();
        }

        public static List<int> GetCheckedIDs(this CheckedListBox clb, string idColumnName)
        {
            return clb.CheckedItems.OfType<DataRowView>()
                      .Select(item => (int)item[idColumnName])
                      .ToList();
        }

        // داخل مشروع الـ BusinessLayer أو أي مشروع يرى الـ BusinessLayer


    }
}
