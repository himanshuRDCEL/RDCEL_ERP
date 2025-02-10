using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Helper
{
    public static class ExcelExportHelper
    {
        #region Convert List into Excel Memory Stream Added by VK
        public static MemoryStream ListExportToExcel<T>(List<T> items)
        {
            bool flag = false;
            using (XLWorkbook wb = new XLWorkbook())
            {
                MemoryStream stream = new MemoryStream();
                wb.Worksheets.Add(ListToDatatable.ToDataTable(items));
                using (stream)
                {
                    wb.SaveAs(stream);
                    return stream;
                }
            }
        }
        #endregion

        #region 
        public static MemoryStream MultiListExportToExcel<T1,T2,T3>(List<T1>? list1 = null, List<T2>? list2 = null, List<T3>? list3 = null)
        {
            bool flag = false;
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (list1 != null && list1.Count > 0)
                {
                    wb.Worksheets.Add(ListToDatatable.ToDataTable(list1),"PendingForQC");
                    //object res = list1?.GetType().GetMembers(BindingFlags.Default);
                }
                if (list2 != null && list2.Count > 0)
                {
                    wb.Worksheets.Add(ListToDatatable.ToDataTable(list2), "PendingForPriceAcceptance");
                }
                if (list3 != null && list3.Count > 0)
                {
                    wb.Worksheets.Add(ListToDatatable.ToDataTable(list3), "PendingForPickup");
                }
                MemoryStream stream = new MemoryStream();
                using (stream)
                {
                    wb.SaveAs(stream);
                    return stream;
                }
            }
        }
        #endregion


    }
}
