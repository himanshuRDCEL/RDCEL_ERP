using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace RDCELERP.Common.Helper
{
    public static class ExcelImportHelper
    {
        public static DataTable ExcelToDatatable(IFormFile ImportFile)
        {
            ISheet sheet = null;
            //datatable
            DataTable dt = new DataTable();
            if (ImportFile.Length > 0)
            {
                string sFileExtension = Path.GetExtension(ImportFile.FileName).ToLower();
                var stream = ImportFile.OpenReadStream();
                stream.Position = 0; /*var artistAlbums = null;*/
                /*HSSFWorkbook hssfwb = new HSSFWorkbook(stream);*/
                if (sFileExtension == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                }
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    dt.Columns.Add(cell.ToString().Replace("*", ""), typeof(object));
                }

                //Fill data in BusinessPartnerViewModel
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    //if (row.Cells.All(d => d.CellType == CellType.Blank));

                    var rowItem = row.ToArray();
                    dt.Rows.Add(rowItem);
                }
            }
            return dt;
        }
        public static List<T> ExcelToList<T>(IFormFile ImportFile, List<T> DynamicListModel)
        {
            ISheet sheet = null;
            //datatable
            DataTable dt = new DataTable();
            if (ImportFile.Length > 0)
            {
                string sFileExtension = Path.GetExtension(ImportFile.FileName).ToLower();
                var stream = ImportFile.OpenReadStream();
                stream.Position = 0; /*var artistAlbums = null;*/
                /*HSSFWorkbook hssfwb = new HSSFWorkbook(stream);*/
                if (sFileExtension == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                }
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    dt.Columns.Add(cell.ToString().Replace("*", ""), typeof(string));
                }

                //Fill data in BusinessPartnerViewModel

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    var rowItem = new object[row.LastCellNum];
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null || cell.CellType == CellType.Blank)
                        {
                            rowItem[j] = null; // Set null value for blank cell
                        }
                        else
                        {
                            rowItem[j] = cell.ToString();
                        }
                    }

                    dt.Rows.Add(rowItem);
                }
                DynamicListModel = ConvertDatatableToList<T>(dt);
            }
            return DynamicListModel;
        }


        public static List<T> ConvertDatatableToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties3 = typeof(T).GetProperties();
            /* var properties1 = typeof(T);*/
            var errorList = "";

            /*var properties4 = properties3.SingleOrDefault(s => s.Name == columnNames.SingleOrDefault());
            var propType = properties4.PropertyType;
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(propType);
            var wrongEntries = columnNames.Select((v,i)=>new { Data=v, LineNumber=i}).Where(d=>!converter.IsValid(d.Data));
            if (wrongEntries.Count() > 0)
            {
                foreach (var entry in wrongEntries)
                {
                    string error = "";
                    *//*errors.Add(entry);*//*
                }
            }*/

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties3)
                {
                    bool isMatched = false; // Flag to track match status
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            var columnName = pro.Name.ToLower();
                            if (!string.IsNullOrEmpty(row[columnName]?.ToString()))
                            {
                                var item = Convert.ChangeType(row[columnName], pro.PropertyType);
                                var converter = System.ComponentModel.TypeDescriptor.GetConverter(item);
                                pro.SetValue(objT, item);

                                var wrongEntries = columnNames
                                    .Select((v, i) => new { Data = v, LineNumber = i })
                                    .Where(d => !converter.IsValid(d.Data));

                                if (wrongEntries.Count() > 0)
                                {
                                    foreach (var entry in wrongEntries)
                                    {
                                        string error = "";
                                    }
                                }

                                isMatched = true; // Set the flag to true
                            }
                        }
                        catch (Exception ex)
                        {
                            errorList += ex.Message + ", ";
                        }
                    }

                    if (pro.Name == "Remarks")
                    {
                        pro.SetValue(objT, errorList);
                        errorList = string.Empty;
                    }

                    if (!isMatched)
                    {
                        // Skip setting values for fields with no match
                        continue;
                    }
                }

                return objT;
            }).ToList();
            /* public static List<string> GetModelErrros(ModelStateDictionary modelState)
             {
                 var errors = modelState.Values.Where(E => E.Errors.Count > 0)
                              .SelectMany(E => E.Errors)
                              .Select(E => E.ErrorMessage)
                              .ToList();

                 return errors;
             }*/

        }
    }
}
