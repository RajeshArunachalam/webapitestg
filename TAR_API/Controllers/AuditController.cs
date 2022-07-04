using TAR_API.App_Code;
using TAR_API.Models;
using TAR_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class AuditController : ControllerBase
    {
        AuditRepository repAudit = null;

        public AuditController()
        {
            repAudit = new AuditRepository();
        }

        [Route("getAssignedAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetAssignedAccounts(Audit obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> acc = await repAudit.GetAssignedAccounts(obj);

                if (acc == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    StatusCount = acc.Item1,
                    AccountDetails = acc.Item2,
                    AccountUserDetails = acc.Item3,
                    AuditRoleBasedCount = acc.Item4

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

        public class PreErrorLoadData
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
        }

        public class ClarificationResponse
        {

            public int UserID { get; set; }

            public int SubCategoryID { get; set; }
            public string AccountIDs { get; set; }
            public string ResponseComment { get; set; }

            public string ResponseDescription { get; set; }


        }
        /// <summary>
        /// This is to get the agent pre load data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getAuditErrorPreLoadData")]
        [HttpPost]
        public async Task<IActionResult > GetAuditErrorPreLoadData(PreErrorLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repAudit.GetAuditErrorPreLoadData(obj.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    AuditErrorDetails = objResult.Item1,

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
        [Route("getAuditPreLoadData")]
        [HttpPost]
        public async Task<IActionResult > GetAuditPreLoadData(PreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repAudit.GetAuditPreLoadData(obj.PHMID, obj.UserID);

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
                    Configuration = objResult.Item6
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

        [Route("submitAuditTransaction")]
        [HttpPost]
        public async Task<IActionResult > SubmitQCTransaction(SubmitAuditTransaction obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repAudit.SubmitAuditTransaction(obj);

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
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing Inbox/SubmitAuditTransaction");
            }
        }

        [Route("editAuditTransaction")]
        [HttpPost]
        public async Task<IActionResult > editAuditTransaction(EditAuditTransaction obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repAudit.editAuditTransaction(obj);

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




        public class Clarification
        {

            public int UserID { get; set; }
            public String AccountID { get; set; }
            public int ClarificationID { get; set; }
            public int SubCategoryID { get; set; }
            public string ResponseComment { get; set; }
            public string ResponseDescription { get; set; }


        }

        /// <summary>
        /// This is to submit the clarification details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitClarificationResponse")]
        [HttpPost]
        public IActionResult  SubmitClarificationResponse(ClarificationResponse obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                //#region "AccountID"

                //DataTable dtAccountID = new DataTable();
                //dtAccountID.Columns.Add("AccountID", typeof(int));
                //DataRow drRow = dtAccountID.NewRow();
                //drRow["AccountID"] = obj.AccountID;
                //dtAccountID.Rows.Add(drRow);

                //#endregion

                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repAudit.SubmitClarificationResponse(obj.UserID, obj.ResponseComment, obj.ResponseDescription, obj.SubCategoryID, obj.AccountIDs);

                if (objResult == null)
                {
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

        [Route("GetFile")]
        [HttpPost]

        //download file api  
        public IActionResult  GetFile(DocumentUpload obj)
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
                Ok(response);
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
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return Ok(response);
        }



        [Route("GetClientDocuments")]
        [HttpPost]
        public IActionResult  GetClientDocuments(DocumentUpload obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repAudit.GetClientDocuments(obj.PHMID, obj.AccountID.ToString());

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
