using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

using TAR_API.App_Code;

namespace TAR_API.Common
{
    public class ClsFormatExcelFiles
    {
        public void RunMacro(string strInputexcelpath)
        {
            try
            {
                //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                //string path = System.Web.HttpContext.Current.Server.MapPath(@"\App_Data\Macro\");
                ////~~> Start Excel and open the workbook.
                //xlWorkBook = xlApp.Workbooks.Open(path + @"ExcelFormat.xlsm");

                ////~~> Run the macros by supplying the necessary arguments
                //xlApp.Run("formatiing", strInputexcelpath);

                ////~~> Clean-up: Close the workbook
                //xlWorkBook.Close(false);

                ////~~> Quit the Excel Application
                //xlApp.Quit();

                ////~~> Clean Up
                //releaseObject(xlApp);
                //releaseObject(xlWorkBook);

                var workbook = new XLWorkbook(strInputexcelpath);
                var rows = workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1);
                var columns = workbook.Worksheet(1).RangeUsed().ColumnsUsed();

                foreach (var row in rows)
                {
                    var rowNumber = row.RowNumber();
                    foreach (var column in columns)
                    {
                        var columnNumber = column.ColumnNumber();
                        if (row.Cell(columnNumber).DataType.ToString() == "Number")
                        {
                            row.Cell(columnNumber).SetValue(row.Cell(columnNumber).Value.ToString());
                        }
                    }
                }
                workbook.Save();
                workbook.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}