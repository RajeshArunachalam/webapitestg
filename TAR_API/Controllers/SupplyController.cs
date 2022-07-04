
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TAR_API;
using TAR_API.App_Code;
using TAR_API.Common;
using TAR_API.Models;
using TAR_API.Repository;
using Color = System.Drawing.Color;

namespace ARCAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class SupplyController : ControllerBase
    {
       
        private readonly IHttpContextAccessor _httpContextAccessor;
        private SupplyRepository repsupply = null;
        // MessageDetails objMessage = null;

        public SupplyController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            repsupply = new SupplyRepository();
        }
        #region "Supply Configuratin"

        public class FieldMapping
        {
            public int PHMID { get; set; }

        }

        /// <summary>
        /// This method is used to get the column mapping details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getColumnMappingDetails")]
        [HttpPost]
        public async Task<IActionResult> GetColumnMappingDetails(FieldMapping obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repsupply.GetFieldMappingDetails(obj.PHMID);

                if (objResult == null)
                {
                    //When expected crud operation is not done ,error message is given as NotModified.
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                return  Ok( objResult);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        public class SaveFieldMapping
        {
            public int PHMID { get; set; }
            public String UserName { get; set; }
            public string FieldMapping { get; set; }

        }
        public class FieldMappingDetails
        {
            public int FieldID { get; set; }
            public string FieldName { get; set; }
            public string ProjectFieldName { get; set; }

            public bool IsMandatory { get; set; }
            public bool IsUnique { get; set; }
            public bool IsWorkLevel { get; set; }
            public bool IsGroupBy { get; set; }
            public bool IsCSVBOT { get; set; }
            public bool IsIVRBOT { get; set; }
            public int FieldOrder { get; set; }
            public bool IsActive { get; set; }
        }


        /// <summary>
        /// This is the method to save supply field mapping
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("saveSupplyFieldMapping")]
        [HttpPost]
        public async Task<IActionResult> SaveSupplyFieldMapping(SaveFieldMapping obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("FieldID", typeof(int));
                dt.Columns.Add("ProjectFieldName", typeof(String));
                dt.Columns.Add("IsMandatory", typeof(bool));
                dt.Columns.Add("IsUnique", typeof(bool));
                dt.Columns.Add("IsWorkLevel", typeof(bool));
                dt.Columns.Add("IsGroupBy", typeof(bool));
                dt.Columns.Add("IsIVRBOT", typeof(bool));
                dt.Columns.Add("IsCSVBOT", typeof(bool));
                dt.Columns.Add("FieldOrder", typeof(int));
                dt.Columns.Add("IsActive", typeof(bool));


                dynamic dynJson = JsonConvert.DeserializeObject<IList<FieldMappingDetails>>(obj.FieldMapping);
                foreach (var objVar in dynJson)
                {
                    DataRow drRow = dt.NewRow();
                    drRow = dt.NewRow();
                    drRow["FieldID"] = objVar.FieldID;
                    drRow["ProjectFieldName"] = objVar.ProjectFieldName;
                    drRow["IsMandatory"] = objVar.IsMandatory;
                    drRow["IsUnique"] = objVar.IsUnique;
                    drRow["IsWorkLevel"] = objVar.IsWorkLevel;
                    drRow["IsGroupBy"] = objVar.IsGroupBy;
                    drRow["IsIVRBOT"] = objVar.IsIVRBOT;
                    drRow["IsCSVBOT"] = objVar.IsCSVBOT;
                    drRow["FieldOrder"] = 1;
                    drRow["IsActive"] = objVar.IsActive;
                    dt.Rows.Add(drRow);
                }
                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repsupply.saveSupplyFieldMapping(obj.PHMID, obj.UserName, dt);

                if (objResult == null)
                {
                    //When expected crud operation is not done ,error message is given as NotModified.
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                return Ok(objResult);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        #endregion  "Supply Configuratin"



        #region "Supply Upload"
        MessageDetails objMessage = null;
        DataTable dtExcelData = new DataTable();
        List<FieldMappings> listProjectColumns = new List<FieldMappings>();
        public class MessageDetails
        {
            public string MessageType { get; set; }
            public string MessageDescription { get; set; }
        }
        public class UploadMode
        {
            public const string Manual = "MANL";
            public const string Auto = "AUTO";
        }
        public class SupplyColumn
        {
            public int FieldColumnID { get; set; }
            public string FieldName { get; set; }
            public int PHMID { get; set; }
            public int SupplyTypeID { get; set; }
            public int ClientID { get; set; }
            public int LocationID { get; set; }
            public int ProjectID { get; set; }
            public string FileName { get; set; }

            public int UserID { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string RecordType { get; set; }
            public string UniqueID { get; set; }
            public string UserName { get; set; }

        }

        #region ReadDataFromExcel
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="isHDR"></param>
        /// <returns></returns>
        public Tuple<System.Data.DataTable, MessageDetails> ReadDataFromExcel(string FilePath, string isHDR)
        {
            System.Data.DataTable dtData = new System.Data.DataTable();
            System.Data.DataTable dtExcelSchema = new System.Data.DataTable();
            MessageDetails objMessage = new MessageDetails();
            IExcelDataReader excelReader = null;
            string FirstSheetName = "Sheet1";
            string SheetName = string.Empty;

            try
            {
                FileStream stream = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read);
                string Extension = System.IO.Path.GetExtension(FilePath);

                switch (Extension.ToLower())
                {
                    case ".xls": //Reading from a binary Excel file ('97-2003 format; *.xls)
                        try
                        {
                            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        catch (Exception)
                        {
                            objMessage.MessageType = "ERROR";
                            objMessage.MessageDescription = "Problem in opening excel file..!";
                            return Tuple.Create(dtData, objMessage);
                        }
                        break;
                    case ".xlsx": //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        try
                        {
                            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        catch (Exception)
                        {
                            objMessage.MessageType = "ERROR";
                            objMessage.MessageDescription = "Problem in opening excel file..!";
                            return Tuple.Create(dtData, objMessage);
                        }
                        break;
                    default:
                        return null;


                }

                try
                {
                    //excelReader.IsFirstRowAsColumnNames = true;
                    DataSet dsResult = new DataSet();

                    try
                    {
                        dsResult = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        }); ;
                    }
                    catch (Exception ex)
                    {

                        string conn = string.Empty;

                        if (Extension.CompareTo(".xls") == 0)
                            conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties='Excel 8.0;HRD=YES;IMEX=1';"; //for below excel 2007  
                        else
                            conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties='Excel 12.0;HRD=YES;IMEX=1';"; //for above excel 2007  
                        using (OleDbConnection con = new OleDbConnection(conn))
                        {
                            try
                            {
                                OleDbDataAdapter oleAdpt = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", con); //here we read data from sheet1  
                                oleAdpt.Fill(dsResult); //fill excel data into dataTable  

                                if (dsResult != null && dsResult.Tables.Count > 0)
                                {
                                    SheetName = FirstSheetName;
                                    dtData = dsResult.Tables[0];
                                }
                            }
                            catch { }
                        }

                    }


                    if (dsResult != null && dsResult.Tables.Count > 0)
                    {
                        foreach (DataTable dtName in dsResult.Tables)
                        {
                            if (dtName.TableName.Equals(FirstSheetName))
                            {
                                SheetName = dtName.TableName;
                                dtData = dsResult.Tables[0];
                                break;
                            }
                        }
                    }
                    else
                    {
                        objMessage.MessageType = "ERROR";
                        objMessage.MessageDescription = "No values found in uploaded excel..!";
                        return Tuple.Create(dtData, objMessage);
                    }

                    if (string.IsNullOrEmpty(SheetName))
                    {
                        objMessage.MessageType = "ERROR";
                        objMessage.MessageDescription = "Expected sheet name mismatching (First sheet name should be Sheet1 )..!";
                        return Tuple.Create(dtData, objMessage);
                    }

                }
                catch (Exception ex)
                {
                    objMessage.MessageType = "ERROR";
                    objMessage.MessageDescription = "Problem in reading excel file..!";
                    return Tuple.Create(dtData, objMessage);
                }

            }
            catch (Exception ex)
            {
                objMessage.MessageType = "ERROR";
                objMessage.MessageDescription = "Problem in opening excel file..!";
                return Tuple.Create(dtData, objMessage);
            }
            finally
            {
                excelReader.Close();
            }

            objMessage.MessageType = "SUCCESS";
            objMessage.MessageDescription = "Read data from excel..!";
            return Tuple.Create(dtData, objMessage);
        }
        #endregion

        #region ConvertCSVtoExcel

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// Killing the process by main window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                Process.GetProcessById((int)processID).Kill();
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Convert the CSV file to excel
        /// </summary>
        /// <param name="CSVFilePath"></param>
        /// <returns></returns>
        public Tuple<bool, string> ConvertCSVtoExcel(string CSVFilePath)
        {
            bool IsConverted = false;

            int hWnd = 0;
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.DisplayAlerts = false;
            xlApp.Visible = false;
            hWnd = xlApp.Application.Hwnd;

            object missing = Type.Missing;
            bool ReadOnly = false;

            string sTempExcelFolderPath = string.Empty;
            string sTempExcelFilePath = string.Empty;
            string csvfiletoXLsx = string.Empty;

            try
            {
                xlWorkBook = xlApp.Workbooks.Open(CSVFilePath,
                  missing, //updatelinks
                  ReadOnly, //readonly
                  missing, //format
                  missing, //Password
                  missing, //writeResPass
                  true, //ignoreReadOnly
                  missing, //origin
                  missing, //delimiter
                  true, //editable
                  missing, //Notify
                  missing, //converter
                  missing, //AddToMru
                  missing, //Local
                  missing); //corruptLoad

                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;

                xlWorkSheet.Name = "Sheet1";

                //Get the used Range
                Microsoft.Office.Interop.Excel.Range usedRange = xlWorkSheet.UsedRange;

                //If directory not exist create it.
                 object oFileName = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "CsvUploads\\" + Path.GetFileNameWithoutExtension(CSVFilePath) + ".xlsx");
                //object oFileName = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "CsvUploads\\" + Path.GetFileNameWithoutExtension(CSVFilePath) + ".xlsx");
                csvfiletoXLsx = oFileName.ToString();

                if (!Directory.Exists(Path.GetDirectoryName(csvfiletoXLsx)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(csvfiletoXLsx));
                }

                //If file exist already delete it.
                if (System.IO.File.Exists(csvfiletoXLsx))
                {
                    System.IO.File.Delete(csvfiletoXLsx);
                }

                // usedRange.AutoFit();

                xlWorkBook.SaveAs(oFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, AccessMode: Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive);

                xlWorkBook.Close(true, missing, missing);
                xlApp.Quit();
                IsConverted = true;

            }
            catch (Exception ex)
            {
                // ExceptionHandler.HandleException(ex.ToString());
            }
            finally
            {
                TryKillProcessByMainWindowHwnd(hWnd);
            }
            return Tuple.Create(IsConverted, csvfiletoXLsx);
        }

        #endregion
        /// <summary>
        /// Convert Text To Column
        /// </summary>
        /// <param name="ExcelFilePath"></param>
        /// <param name="ProjectColumns"></param>
        /// <returns></returns>
        public bool ConvertTextToColumn(string ExcelFilePath, List<FieldMappings> ProjectColumns)
        {
            bool bSavedSuccessfully = false;
            bool IsExcelOpened = false;
            List<String> lstFieldColumns = new List<string>();
            List<String> lstUsedFieldColumns = new List<string>();

            try
            {
                //List<FieldColumn> objInvColumns = repField.GetFieldColumns(PHMID);

                foreach (FieldMappings oInvCol in ProjectColumns)
                {
                    lstFieldColumns.Add(oInvCol.ProjectFieldName);
                }

                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook = null;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

                xlApp = new Microsoft.Office.Interop.Excel.Application();

                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                int hWnd = xlApp.Application.Hwnd;

                object missing = Type.Missing;
                bool ReadOnly = false;
                try
                {
                    xlWorkBook = xlApp.Workbooks.Open(ExcelFilePath,
                      missing, //updatelinks
                      ReadOnly, //readonly
                      missing, //format
                      missing, //Password
                      missing, //writeResPass
                      true, //ignoreReadOnly
                      missing, //origin
                      missing, //delimiter
                      true, //editable
                      missing, //Notify
                      missing, //converter
                      missing, //AddToMru
                      missing, //Local
                      missing); //corruptLoad
                    IsExcelOpened = true;
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;

                    try
                    {
                        int colIndex = 1;

                        while (colIndex <= xlWorkSheet.UsedRange.Columns.Count)
                        {
                            if (xlApp.WorksheetFunction.CountA(xlWorkSheet.Cells[1, colIndex]) == 0)
                            {
                                xlWorkSheet.Range[1, colIndex].EntireColumn.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftToLeft);
                            }
                            else
                            {
                                colIndex++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogging.SendErrorToText(ex);
                    }



                    #region "Remove Empty Rows"

                    try
                    {
                        int rowIndex = 1;

                        while (rowIndex <= xlWorkSheet.UsedRange.Rows.Count)
                        {
                            //if (xlApp.WorksheetFunction.CountA(xlWorkSheet.Cells[rowIndex, 1].EntireRow) == 0)
                            if (xlApp.WorksheetFunction.CountA(xlWorkSheet.Range[rowIndex, 1].EntireRow) == 0)
                            {
                                //Delete all empty rows after the last used row
                                xlWorkSheet.Range[rowIndex, 1].EntireRow.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
                                //xlWorkSheet.Cells[rowIndex, 1].EntireRow.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionLogging.SendErrorToText(ex);
                    }

                    #endregion

                    //Get the used Range
                    Microsoft.Office.Interop.Excel.Range usedRange = xlWorkSheet.UsedRange;

                    Microsoft.Office.Interop.Excel.Range xlRng = null;

                    string sColumnName = string.Empty;

                    object[,] currentColumn = null;

                    object[,] valueArray = null;

                    //get an object array of all of the cells in the worksheet (their values)
                    try
                    {
                        valueArray = (object[,])usedRange.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogging.SendErrorToText(ex);
                    }

                    if (valueArray != null)
                    {
                        #region "If header column values are able to read"

                        for (int col = 1; col <= valueArray.GetLength(1); col++)
                        {
                            try
                            {
                                sColumnName = (string)valueArray[1, col];

                                if (string.IsNullOrEmpty(sColumnName))
                                    continue;

                                xlRng = xlWorkSheet.get_Range(string.Format("{0}1", GetExcelColumnName(col)), string.Format("{0}1", GetExcelColumnName(col)));

                                List<FieldMappings> filteredList = ProjectColumns.Where(x => x.ProjectFieldName == sColumnName).ToList();

                                foreach (FieldMappings obj in filteredList)
                                {
                                    //xlRng.Value = obj.ProjectFieldName;

                                    #region "ConvertToText"

                                    if (obj.DataType != null && !string.IsNullOrEmpty(obj.DataType) && obj.DataType.ToUpper() == "STRING")
                                    {
                                        try
                                        {

                                            int[,] _MultiDimentionArray = new int[1, 2] { { 1, 2 } };

                                            Microsoft.Office.Interop.Excel.Range xlColumnRng = xlWorkSheet.Range[string.Format("{0}:{0}", GetExcelColumnName(col))];

                                            xlColumnRng.TextToColumns(Destination: xlWorkSheet.Range[string.Format("{0}1", GetExcelColumnName(col))], DataType: Microsoft.Office.Interop.Excel.XlTextParsingType.xlDelimited, TextQualifier: Microsoft.Office.Interop.Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
                                                ConsecutiveDelimiter: false, Tab: true, Semicolon: false, Comma: false, Space: false, Other: false, FieldInfo: _MultiDimentionArray, TrailingMinusNumbers: true);
                                        }
                                        catch (Exception ex)
                                        {
                                            ExceptionLogging.SendErrorToText(ex);
                                        }

                                    }

                                    #endregion
                                    lstUsedFieldColumns.Add(obj.ProjectFieldName);

                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogging.SendErrorToText(ex);
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region "If header column values are not able to read"

                        for (int col = 1; col <= usedRange.Columns.Count; col++)
                        {
                            // this line does only one COM interop call for the whole column
                            try
                            {
                                currentColumn = (object[,])usedRange.Columns[col, Type.Missing];
                                //currentColumn = (object[,])usedRange.Columns[col, Type.Missing].Value;
                            }
                            catch (Exception ex)
                            {
                                currentColumn = (object[,])usedRange.Columns[col, Type.Missing];
                                //currentColumn = (object[,])usedRange.Columns[col, Type.Missing].Value2;
                                ExceptionLogging.SendErrorToText(ex);
                            }


                            try
                            {
                                sColumnName = ((object[,])currentColumn)[1, 1].ToString();

                                xlRng = xlWorkSheet.get_Range(string.Format("{0}1", GetExcelColumnName(col)), string.Format("{0}1", GetExcelColumnName(col)));

                                List<FieldMappings> filteredList = ProjectColumns.Where(x => x.ProjectFieldName == sColumnName).ToList();

                                foreach (FieldMappings obj in filteredList)
                                {
                                    //xlRng.Value = obj.FieldColumnName;

                                    #region "ConvertToText"

                                    if (obj.DataType != null && !string.IsNullOrEmpty(obj.DataType) && obj.DataType.ToUpper() == "STRING")
                                    {
                                        int[,] _MultiDimentionArray = new int[1, 2] { { 1, 2 } };

                                        Microsoft.Office.Interop.Excel.Range xlColumnRng = xlWorkSheet.Range[string.Format("{0}:{0}", GetExcelColumnName(col))];

                                        xlColumnRng.TextToColumns(Destination: xlWorkSheet.Range[string.Format("{0}1", GetExcelColumnName(col))], DataType: Microsoft.Office.Interop.Excel.XlTextParsingType.xlDelimited, TextQualifier: Microsoft.Office.Interop.Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
                                            ConsecutiveDelimiter: false, Tab: true, Semicolon: false, Comma: false, Space: false, Other: false, FieldInfo: _MultiDimentionArray, TrailingMinusNumbers: true);

                                    }

                                    #endregion

                                    lstUsedFieldColumns.Add(obj.ProjectFieldName);

                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogging.SendErrorToText(ex);
                                return bSavedSuccessfully;

                            }
                        }

                        #endregion
                    }


                    xlWorkBook.Save();

                    xlWorkBook.Close(true, missing, missing);
                    xlApp.Quit();
                    IsExcelOpened = false;
                    bSavedSuccessfully = true;
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);
                    return bSavedSuccessfully;
                }
                finally
                {
                    if (IsExcelOpened)
                    {
                        xlWorkBook.Close(true, missing, missing);
                        xlApp.Quit();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return bSavedSuccessfully;
            }
            return bSavedSuccessfully;
        }

        /// <summary>
        /// Get Excel Column Name
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        /// <summary>
        /// Saving the excel template
        /// </summary>
        /// <param name="MandatoryProjectColumnn"></param>
        private void ExcelTemplateSave(List<string> MandatoryProjectColumnn)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = null;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook = null;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                Microsoft.Office.Interop.Excel.Range range = null;
                object misValue = System.Reflection.Missing.Value;
                try
                {


                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlApp.Visible = false;
                    xlApp.DisplayAlerts = false;
                    xlWorkBook = xlApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    int iCount = xlApp.Workbooks.Count;
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    xlApp.StandardFont = "Calibri";
                    xlApp.StandardFontSize = 11;

                    int iRow = 1;
                    int iHeadingRowStart = iRow;
                    int sColumnEnd = MandatoryProjectColumnn.Count();
                    xlWorkSheet.Cells[iRow, 1] = "sdfs";
                    int iColumnCount = 1;
                    foreach (var val in MandatoryProjectColumnn)
                    {
                        xlWorkSheet.Cells[iRow, iColumnCount] = val;
                        iColumnCount++;
                    }
                    string sFileName = "SupplyTemplate.xlsx";

                    string downloadsPath = "";
                    //new KnownFolder(KnownFolderType.Downloads).Path;

                    downloadsPath = Path.Combine(downloadsPath, sFileName);

                    range = xlWorkSheet.Rows.get_Range("A1", "A1");
                    //System.Drawing.Color colorHeading = System.Drawing.ColorTranslator.FromHtml("#94DAE3");
                    //range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);

                    if (System.IO.File.Exists(downloadsPath))
                    {
                        System.IO.File.Delete(downloadsPath);
                    }


                    xlWorkBook.SaveAs(downloadsPath);
                    xlWorkBook.Close(true, misValue, misValue);

                }
                catch (Exception ex)
                {

                }
                finally
                {

                }

            }
            catch (Exception ex)
            {

            }
        }

        #region InventoryUploadfile
        /// <summary>
        /// Uploading the inventory file
        /// </summary>
        /// <returns></returns>
        [Route("supplyUploadfile")]
        [HttpPost()]
        public async Task<IActionResult> SupplyUploadfile()
        {
            try
            {

                string sSourcePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "FileUploads");
                // string sSourcePath = _hostingEnvironment.WebRootPath("~/FileUploads/");
                string DestinationPath = ClsCommon._DestinationPath;
                string sDestinationPath = @DestinationPath;
                bool IsErrorUpload = false;
                SupplyColumn obj = new SupplyColumn();
                var httpRequest = _httpContextAccessor.HttpContext.Request;
                IFormFileCollection files = _httpContextAccessor.HttpContext.Request.Form.Files;
                Microsoft.AspNetCore.Http.IFormCollection hparam = _httpContextAccessor.HttpContext.Request.Form;


                if (files.Count > 0)
                {
                    // HttpPostedFile file = files[0];
                    obj.FileName = hparam["FileName"];
                    obj.PHMID = Convert.ToInt32(hparam["PHMID"]);
                    obj.SupplyTypeID = Convert.ToInt32(hparam["SupplyTypeID"]);
                    obj.ClientID = Convert.ToInt32(hparam["ClientID"]);
                    obj.LocationID = Convert.ToInt32(hparam["LocationID"]);
                    obj.ProjectID = Convert.ToInt32(hparam["PracticeID"]);
                    obj.UniqueID = Convert.ToString(hparam["UniqueID"]);
                    obj.UserName = Convert.ToString(hparam["UserName"]);

                }
                if (obj.PHMID == 0)
                {
                    // When expected parameters are not passed, error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                // CHECK THE FILE COUNT.
                for (int fileCount = 0; fileCount <= files.Count - 1; fileCount++)
                {
                    IFormFile file = files[fileCount];

                    if (file.Length > 0)
                    {
                        sSourcePath = Path.Combine(sSourcePath, Path.GetFileName(file.FileName));
                        var x = Path.GetFileName(sSourcePath);

                        #region "File Name Checking"

                        string sFileNameWithExtension = Path.GetFileName(sSourcePath);
                        string sFileNameWithoutExtension = Path.GetFileNameWithoutExtension(sSourcePath);
                        string sFileNameWithoutUniqueID = string.Empty;

                        // Getting all the practice column names
                        if (obj.PHMID > 0)
                        {
                            listProjectColumns = await repsupply.GetTemplate(obj.SupplyTypeID, obj.PHMID);
                        }

                        if (listProjectColumns.Count == 0)
                        {
                            objMessage.MessageType = "ERROR";
                            objMessage.MessageDescription = "File format error, Column Header's mismatch";
                            return Ok(objMessage);
                        }


                        if (obj.UniqueID != sFileNameWithoutExtension.Split('_').GetValue(0).ToString())
                        {
                            objMessage.MessageType = "ERROR";
                            objMessage.MessageDescription = string.Format("please provide valid UniqueID for selected proctice : UniqueID_FileName", sFileNameWithExtension);
                            return Ok(objMessage);
                        }

                        if (!IsErrorUpload)
                        {
                            string sCheckUniqueID = sFileNameWithoutExtension.Split('_').GetValue(0).ToString();

                            Regex regex = new Regex(@"^[0-9]*$");

                            if (!regex.IsMatch(sCheckUniqueID))
                            {

                                objMessage.MessageType = "ERROR";
                                objMessage.MessageDescription = string.Format("uploaded file name is in correct format.Please follow : UniqueID_FileName", sFileNameWithExtension);
                                return Ok(objMessage);

                            }
                        }

                        string[] sSplitFileNamechk = Path.GetFileName(sSourcePath).Split('_');

                        if (sSplitFileNamechk.Length > 1)
                        {
                            var result = sSplitFileNamechk.Skip(1);
                            sFileNameWithoutUniqueID = string.Join("_", result.ToArray());
                        }
                        #endregion

                        #region "ConvertCSVtoExcel"
                        if (Path.GetExtension(sSourcePath).ToUpper() == ".PDF" || Path.GetExtension(sSourcePath).ToUpper() == ".TXT")
                        {
                            objMessage.MessageType = "ERROR";
                            objMessage.MessageDescription = string.Format("[{0}] please upload file formates .csv/.ods/.xlsx/.xls only. Please try with different file name.", Path.GetFileName(sSourcePath));
                            return Ok (objMessage);
                        }

                        #endregion

                        #region "ConvertCSVtoExcel"

                        if (System.IO.File.Exists(sSourcePath))
                        {
                            if (Path.GetExtension(sSourcePath).ToUpper() == ".CSV" || Path.GetExtension(sSourcePath).ToUpper() == ".ODS" || Path.GetExtension(sSourcePath).ToUpper() == ".TXT")
                            {
                                Tuple<bool, string> ConversionDetails = ConvertCSVtoExcel(sSourcePath);

                                if (!ConversionDetails.Item1)
                                {
                                    objMessage.MessageType = "ERROR";
                                    objMessage.MessageDescription = string.Format("CSV/ODS/TXT file Conversion failed");

                                }

                                sSourcePath = ConversionDetails.Item2;
                            }

                        }

                        #endregion

                        #region "Checking - File uploaded already"

                        //var iExistCount = await repInventory.GetFileNameCheck(Path.GetFileName(sSourcePath), obj.CHMapID);

                        //if (iExistCount >= 1)
                        //{
                        //    objMessage.MessageType = "ERROR";
                        //    objMessage.MessageDescription = string.Format("[{0}] File uploaded already. Please try with different file name.", Path.GetFileName(sSourcePath));
                        //    return Ok( objMessage);
                        //}

                        #endregion

                        // CHECK THE FILE TYPE (YOU CAN CHECK WITH .xls ALSO).
                        if (sSourcePath.EndsWith(".xlsx"))
                        {
                            if (!Directory.Exists(Path.GetDirectoryName(sSourcePath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(sSourcePath));
                            }

                            // SAVE THE FILES IN THE FOLDER.
                            using (Stream fileStream = new FileStream(sSourcePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            //file.SaveAs(sSourcePath);
                            if (System.IO.File.Exists(sSourcePath))
                            {

                                #region "Read data from excel"

                                //bool istexttocolumnconverted = ConvertTextToColumn(sSourcePath, listPracticeColumns);
                                //if (!istexttocolumnconverted)
                                //{
                                //    objMessage.MessageType = "error"; ;
                                //    objMessage.MessageDescription = "converting text to column";
                                //    return Ok( objMessage);
                                //}

                                Tuple<System.Data.DataTable, MessageDetails> objExcelData = ReadDataFromExcel(sSourcePath, "YES");
                                dtExcelData = objExcelData.Item1;
                                objMessage = objExcelData.Item2;

                                if (objMessage.MessageType.Equals("ERROR"))
                                {
                                    return Ok( objMessage);
                                }

                                #endregion

                                #region "Check for Columns & Rows Count in Uploaded Excel"

                                if (dtExcelData != null)
                                {
                                    //Check for the excel columns count , if no columns found then return error message.
                                    if (dtExcelData.Columns.Count == 0)
                                    {
                                        objMessage.MessageType = "ERROR";
                                        objMessage.MessageDescription = "No columns are found..!";
                                        return Ok(objMessage);
                                    }

                                    //Check for the excel rows count , if no rows found then return error message.
                                    if (dtExcelData.Rows.Count == 0)
                                    {
                                        objMessage.MessageType = "ERROR";
                                        objMessage.MessageDescription = "No rows are found..!";
                                        return Ok( objMessage);
                                    }
                                }

                                #endregion

                                #region "Practice column checking"

                                // Getting all the practice column names
                                // listPracticeColumns = await repInventory.GetTemplate(obj.InventoryTypeID, obj.CHMapID);//commented by suresh

                                // This is to convert practice columns into list to compare with excel columns.
                                List<string> lstPracticeColumns = listProjectColumns.AsEnumerable().Select(r => r.ProjectFieldName).ToList<string>();

                                // This is to convert excel column values into list to compare with inventory fields.
                                List<string> lstExcelColumns = (from dc in dtExcelData.Columns.Cast<DataColumn>() select dc.ColumnName).ToList<string>();

                                // Get the list of not matched columns.
                                var NotMatchingColumns = from i in lstPracticeColumns
                                                         where !lstExcelColumns.Contains(i)
                                                         select i;

                                if (NotMatchingColumns.Count() > 0)
                                {
                                    string sMessageDescription = string.Empty;
                                    string sMissingColumns = string.Empty;

                                    foreach (string column in NotMatchingColumns)
                                    {
                                        sMissingColumns = string.IsNullOrEmpty(sMissingColumns) ? column : sMissingColumns + Environment.NewLine + column;
                                    }

                                    sMessageDescription = string.Format("({0}) Column{1} {2} missing in excel..! ", NotMatchingColumns.Count(), (NotMatchingColumns.Count() > 1 ? 's' : ' '), (NotMatchingColumns.Count() > 1 ? "are" : "is"));

                                    sMessageDescription = string.Format(sMessageDescription + Environment.NewLine + "[ " + sMissingColumns + " ]");

                                    objMessage.MessageType = "ERROR";
                                    objMessage.MessageDescription = sMessageDescription;

                                    return Ok( objMessage);
                                }

                                #endregion
                            }


                            #region "Moving file to Share Folder"


                            try
                            {


                                DateTime dtTempCurrentTime = DateTime.Now;
                                string sUploadShareFolderPath = ClsCommon._DestinationPath;
                                string sYearMonthDay = string.Format("{0}\\{1}\\{2}\\{3}\\", sUploadShareFolderPath, dtTempCurrentTime.ToString("yyyy"), dtTempCurrentTime.ToString("MMM"), dtTempCurrentTime.ToString("dd"));
                                string sServerDirectoryPath = Path.Combine(sUploadShareFolderPath, sYearMonthDay);
                                string sServerFilePath = Path.Combine(sServerDirectoryPath, Path.GetFileNameWithoutExtension(sSourcePath) + "(" + dtTempCurrentTime.ToString("ddMMyyyyhhmmsstt") + ")" + Path.GetExtension(sSourcePath));

                                if (!Directory.Exists(Path.GetDirectoryName(sServerFilePath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(sServerFilePath));
                                }

                                if (System.IO.File.Exists(sServerFilePath))
                                {
                                    System.IO.File.Delete(sServerFilePath);
                                }


                                System.IO.File.Move(sSourcePath, sServerFilePath);
                                ClsFormatExcelFiles formatEcxcel = new ClsFormatExcelFiles();//commented by suresh
                                formatEcxcel.RunMacro(sServerFilePath);//commented by suresh
                                #region "Execute SSIS Package"

                                int objResult = 0;

                                objResult = await repsupply.SSISPackageExecution(sServerFilePath, Path.GetFileName(sSourcePath), Path.GetFileName(sServerFilePath), obj.PHMID, obj.SupplyTypeID, UploadMode.Manual, obj.UserName);
                                if (objResult > 0)
                                {

                                }
                                #endregion

                                objMessage.MessageType = "SUCCESS";
                                objMessage.MessageDescription = "Completed!";
                                return Ok( objMessage);


                            }
                            catch (Exception ex)
                            {
                                objMessage.MessageType = "ERROR";
                                objMessage.MessageDescription = "There is a problem in moving file to server!";
                                ExceptionLogging.SendErrorToText(ex);
                                return Ok( objMessage);
                            }

                            #endregion



                        }
                    }


                }
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
            }
            return null;




        }
        #endregion

        public class SupTemplate
        {
            public int PHMID { get; set; }

        }
        /// <summary>
        /// Get Inventory template for the Column list.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getSupplyTemplate")]
        [HttpPost]
        public IActionResult getSupplyTemplate(SupTemplate obj)
        {
            try
            {
                if (obj.PHMID == 0)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = repsupply.GetSupplyTemplate(obj.PHMID);

                if (myResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }

                // Requested data are transfered as json data.
                return Ok( myResult);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        public class ImportFileDetails
        {
            public int UserID { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public int ClientID { get; set; }
            public int LocationID { get; set; }
            public int ProjectID { get; set; }
            public string RecordType { get; set; }

        }



        /// <summary>
        /// This is to import the file details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getImportFileDetails")]
        [HttpPost]
        public IActionResult GetImportFileDetails(ImportFileDetails obj)
        {

            try
            {
                if (obj.UserID == 0)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }



                //This is to call the repository method.
                var myResult = repsupply.GetImportFileDetails(obj.UserID, obj.FromDate, obj.ToDate, obj.ClientID, obj.LocationID, obj.ProjectID, obj.RecordType);

                if (myResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }

                // Requested data are transfered as json data.
                return Ok(myResult);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        public class ErrorRecords
        {
            public int ImportFileID { get; set; }

        }

        /// <summary>
        /// This method is used to get the error records after uploading the inventory file.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getSupplyErrorRecords")]
        [HttpPost]
        public IActionResult GetSupplyErrorRecords(ErrorRecords obj)
        {

            try
            {
                if (obj.ImportFileID == 0)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }



                //This is to call the repository method.
                var myResult = repsupply.GetSupplyErrorRecords(obj.ImportFileID);

                if (myResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }

                // Requested data are transfered as json data.
                return Ok(myResult);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        #endregion  "Supply Upload"

    }

}
