using TAR_API.App_Code;
using TAR_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TAR_API.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Specialized;
using Microsoft.AspNetCore.StaticFiles;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class ProductionController : ControllerBase
    {
        ProductionRepository repProduction = null;

        
        private readonly IHttpContextAccessor _httpContextAccessor;
      
        //public ProductionController(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        //{
        //    repProduction = new ProductionRepository();
        //     _env = env;
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public ProductionController(IHttpContextAccessor httpContextAccessor)
        {
           
            _httpContextAccessor = httpContextAccessor;
            repProduction = new ProductionRepository();
           
        }

        public class PreLoadData
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
        }
        /// <summary>
        /// This is to get the agent pre load data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getProductionPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetProductionPreLoadData(PreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, Tuple<IEnumerable<dynamic>>> objResult = await repProduction.GetProductionPreLoadData(obj.PHMID, obj.UserID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    SupplyFields = objResult.Item1,
                    AdditionalCaptures = objResult.Item2,
                    Scenario = objResult.Item3,
                    CallType = objResult.Item4,
                    AgingDetails = objResult.Item5,
                    Configuration = objResult.Item6,
                    Users = objResult.Item7,
                    BotServiceType = objResult.Rest.Item1
                };

                // Requested data are transfered as json data.
                return Ok(myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

             return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }


        /// <summary>
        /// GetAccounts
        /// </summary>
        public class inboxdetails
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
            public string ImportFileID { get; set; }
            public string StatusCode { get; set; }
            public string RoleCode { get; set; }
            public String SearchData { get; set; }

        }
        /// <summary>
        /// This To Get the get the accounts
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetAccounts(inboxdetails obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                //This is to parse JsonData
                DataTable dtSearch = ClsCommon.ParseSearchData(obj.SearchData);
                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repProduction.GetAccounts(obj.PHMID, obj.UserID, obj.ImportFileID, obj.RoleCode, obj.StatusCode, dtSearch);

                if (objResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    StatusCount = objResult.Item1,
                    AccountDetails = objResult.Item2,
                    RoleBasedCount = objResult.Item3

                };

                // Requested data are transfered as json data.
                return Ok(myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                // ExceptionLogging.SendErrorToText(ex);
             return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }

        public struct RuleAccountInfo
        {
            public int PHMID { get; set; }
            public String AccountID { get; set; }
        }
        /// <summary>
        /// GetAccountRuleInfo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetAccountRuleInfo")]
        [HttpPost]
        public IActionResult GetAccountRuleInfo(RuleAccountInfo obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repProduction.GetAccountRuleInfo(obj.PHMID, obj.AccountID);

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
                //This is to show the error occurance place.
             return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }

        public class AccountStartedTime
        {
            public int AccountID { get; set; }
        }

        public class PreAudit
        {
            public string SupplyIDsJSONDATA { get; set; }
            public int UserID { get; set; }
            public int PHMID { get; set; }
            public string PreAuditAcknowledgement { get; set; }
        }
        /// <summary>
        /// This is to get the history details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("updateAccountStartedTime")]
        [HttpPost]
        public async Task<IActionResult> updateAccountStartedTime(AccountStartedTime obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                int iAfftedRows = await repProduction.updateAccountStartedTime(obj.AccountID);

                if (iAfftedRows > 0)
                {
                    // Requested data are transfered as json data.
                   return Ok(iAfftedRows);
                }
                else
                {
                    //When expected crud operation is not done ,error message is given as NotModified.
                    return NotFound(HttpStatusCode.NotModified);
                }
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

            return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }






        [Route("SubmitPreAuditAcknowledgement")]
        [HttpPost]
        public async Task<IActionResult> SubmitPreAuditAcknowledgement(PreAudit obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repProduction.PreAuditAcknowledgement(obj);

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
                //This is to show the error occurance place.

              return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }


        /// <summary>
        /// This is to submit agent transaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitAssociateTransaction")]
        [HttpPost]
        public async Task<IActionResult> SubmitAssociateTransaction(SubmitProductionTransaction obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repProduction.SubmitAssociateTransaction(obj);

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
                //This is to show the error occurance place.
            return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }

        public class ClarificationDetails
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }

            public String AccountID { get; set; }
        }
            public class Clarification
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
            public string Query { get; set; }
            public int SubCategoryID { get; set; }
            public String AccountID { get; set; }
            public string ClarificationRoleCode { get; set; }
            public string ClarificationTo { get; set; }

        }

        /// <summary>
        /// This is to get the history details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitClarification")]
        [HttpPost]
        public IActionResult SubmitClarification(Clarification obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                #region "InventoryID"

                //DataTable dtAccountID = new DataTable();
                //dtAccountID.Columns.Add("AccountID", typeof(int));
                //dynamic dynJson = JsonConvert.DeserializeObject(obj.AccountID);
                //foreach (var item in dynJson)
                //{
                //    DataRow drRow = dtAccountID.NewRow();
                //    drRow["AccountID"] = item.AccountID;
                //    dtAccountID.Rows.Add(drRow);
                //}

                #endregion

                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repProduction.SubmitClarification(obj.PHMID, obj.UserID, obj.Query, obj.SubCategoryID.ToString(), obj.AccountID, obj.ClarificationRoleCode, obj.ClarificationTo);

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
                //This is to show the error occurance place.
             return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }

        public class NonWorkable
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
            public String AccountIDs { get; set; }
            public String Notes { get; set; }

        }

        /// <summary>
        /// This is to get the history details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitNonWorkable")]
        [HttpPost]
        public IActionResult SubmitNonWorkable(NonWorkable obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repProduction.SubmitNonWorkable(obj.PHMID, obj.UserID, obj.AccountIDs, obj.Notes);

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
                //This is to show the error occurance place.
           return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }




        public class Rebuttal
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
            public String AccountIDs { get; set; }
            public String Notes { get; set; }

        }

        /// <summary>
        /// This is to get the history details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitRebuttal")]
        [HttpPost]
        public IActionResult SubmitRebuttal(Rebuttal obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repProduction.SubmitRebuttal(obj.PHMID, obj.UserID, obj.AccountIDs, obj.Notes);

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
                //This is to show the error occurance place.
              return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }












        [Route("GetClarification")]
        [HttpPost]
        public IActionResult GetClarification(ClarificationDetails obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repProduction.GetClarification(obj.PHMID, obj.UserID, obj.AccountID);

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
                //This is to show the error occurance place.
            return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }

        public class DocumentDeleteClass
        {
            public int FileID { get; set; }
        }

            public class DocumentUpload
        {
            public int FieldColumnID { get; set; }
            public string FieldName { get; set; }
            public string FileDescription { get; set; }
            public int PHMID { get; set; }
            public int SupplyTypeID { get; set; }
            public int AccountID { get; set; }
            public int LocationID { get; set; }
            public int ProjectID { get; set; }
            public string FileName { get; set; }
            public string FileID { get; set; }
            public int UserID { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string RecordType { get; set; }
            public string UniqueID { get; set; }
            public string UserName { get; set; }

        }
        #region DocumentUploadfile
        /// <summary>
        /// Uploading the inventory file
        /// </summary>
        /// <returns></returns>
        [Route("AccountUploadfile")]
        [HttpPost()]
        public async Task<IActionResult> AccountUploadfile()
        {
            try
            {
                //var webRoot = this._env.WebRootPath;
              // ProductionController p = new ProductionController(IHostingEnvironment,)
                string sSourcePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "FileUploads");
                string DestinationPath = ClsCommon._DestinationPath;
                string sDestinationPath = @DestinationPath;
                bool IsErrorUpload = false;
                DocumentUpload obj = new DocumentUpload();
                var httpRequest = _httpContextAccessor.HttpContext.Request;
                IFormFileCollection files = _httpContextAccessor.HttpContext.Request.Form.Files;
                Microsoft.AspNetCore.Http.IFormCollection hparam = _httpContextAccessor.HttpContext.Request.Form;

                if (files.Count > 0)
                {
                    // HttpPostedFile file = files[0];
                    obj.FileName = hparam["FileName"];
                    obj.FileDescription = hparam["FileDescription"];
                    obj.PHMID = Convert.ToInt32(hparam["PHMID"]);
                    obj.AccountID = Convert.ToInt32(hparam["AccountID"]);
                    obj.UserID = Convert.ToInt32(hparam["UserID"]);

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


                        string[] sSplitFileNamechk = Path.GetFileName(sSourcePath).Split('_');

                        if (sSplitFileNamechk.Length > 1)
                        {
                            var result = sSplitFileNamechk.Skip(1);
                            sFileNameWithoutUniqueID = string.Join("_", result.ToArray());
                        }
                        #endregion


                        // CHECK THE FILE TYPE (YOU CAN CHECK WITH .xls ALSO).
                        if (1 == 1)
                        // if (sSourcePath.EndsWith(".xlsx"))
                        {
                            if (!Directory.Exists(Path.GetDirectoryName(sSourcePath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(sSourcePath));
                            }

                            // SAVE THE FILES IN THE FOLDER.
                            //file.SaveAs(sSourcePath);
                            using (Stream fileStream = new FileStream(sSourcePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            if (System.IO.File.Exists(sSourcePath))
                            {
                            }


                            #region "Moving file to Share Folder"


                            try
                            {


                                DateTime dtTempCurrentTime = DateTime.Now;
                                string sUploadShareFolderPath = ClsCommon._DestinationPathAccounts;
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
                                string[] filetype = sServerFilePath.Split('.');
                                repProduction.DocumentUploadSave(obj.PHMID, obj.UserID, Convert.ToString(obj.AccountID), obj.FileName, obj.FileDescription, sServerFilePath, "." + filetype[filetype.Length - 1]);

                                var message = new
                                {
                                    MessageType = "SUCCESS",
                                    MessageDescription = "Completed!"
                                };

                                return Ok(message);


                            }
                            catch (Exception ex)
                            {
                                var message = new
                                {
                                    MessageType = "ERROR",
                                    MessageDescription = "There is a problem in moving file to server!"
                                };

                                ExceptionLogging.SendErrorToText(ex);
                               return Ok(message);
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



        [Route("GetClientDocuments")]
        [HttpPost]
        public IActionResult GetClientDocuments(DocumentUpload obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repProduction.GetClientDocuments(obj.PHMID, obj.AccountID.ToString());

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
                //This is to show the error occurance place.
             return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }


        [Route("GetFile")]
        [HttpPost]

        //download file api  
        public IActionResult GetFile(DocumentUpload obj)
        {
            //Create HTTP Response.  
            HttpResponseMessage response = new HttpResponseMessage();
            //Set the File Path.  
            string filePath = obj.FileName;
            //Check whether File exists.  
            if (!System.IO.File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.  
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", obj.FileName);
                Ok(response.ReasonPhrase);
            }
            //Read the File into a Byte Array.  
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            //Set the Response Content.  
            response.Content = new ByteArrayContent(bytes);
            //Set the Response Content Length.  
            response.Content.Headers.ContentLength = bytes.LongLength;
            //Set the Content Disposition Header Value and FileName.  
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //string contentType = string.Empty;

            //string [] mimeType = obj.FileName.Split('.');
            response.Content.Headers.ContentDisposition.FileName = obj.FileName;
            string contentType = "";
            new FileExtensionContentTypeProvider().TryGetContentType(obj.FileName, out contentType);
            //Set the File Content Type.  
            //Task<HttpResponseMessage> response = await httpClient.GetAsync(request["resource"]);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return Ok(response);
        }
       

        [Route("DocumentDelete")]
        [HttpPost]
        public IActionResult DocumentDelete(DocumentDeleteClass obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repProduction.DocumentDelete(Convert.ToInt32(obj.FileID));

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
                //This is to show the error occurance place.
            return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }



    }

}
