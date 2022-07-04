
using TAR_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TAR_API.App_Code;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class AllotmentController : ControllerBase
    {
        AllotmentRepository repAllot = null;
        public AllotmentController()
        {
            repAllot = new AllotmentRepository();
        }

        public class PreLoadData
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getworkallotmentPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetworkallotmentPreLoadData(PreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repAllot.GetworkallotmentPreLoadData(obj.PHMID, obj.UserID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    SupplyFields = objResult.Item1,
                    Users = objResult.Item2,
                    Configuration = objResult.Item3,
                    AgingDetails = objResult.Item4

                };

                // Requested data are transfered as json data.
                return Ok( myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

                return Ok(HttpStatusCode.InternalServerError);
            }
        }

        #region "work allotment Accounts"


        /// <summary>
        /// GetAccounts
        /// </summary>
        public class workallotmentdetails
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
        public async Task<ActionResult> GetAccounts(workallotmentdetails obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                   return BadRequest(HttpStatusCode.BadRequest);
                }

                DataTable dtSearch = ClsCommon.ParseSearchData(obj.SearchData);

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repAllot.GetAccounts(obj.PHMID, obj.UserID, obj.ImportFileID, obj.RoleCode, obj.StatusCode, dtSearch);

                if (objResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    AccountDetails = objResult.Item1,
                    AccountSummary = objResult.Item2,
                    UserSummary = objResult.Item3,
                    RoleBasedCount = objResult.Item4
                };

                // Requested data are transfered as json data.
                return Ok(myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                // ExceptionLogging.SendErrorToText(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
                //return Http(HttpStatusCode.InternalServerError, "Error occured while executing GetAccounts");
            }
        }

        #endregion

        public class AssignAccount
        {
            public String SupplyData { get; set; }
            public int LoginUserID { get; set; }
            public string AssignType { get; set; }
            public string NeedCallRemarks { get; set; }
            public int PHMID { get; set; }

        }
        /// <summary>
        /// This to assign accounts to user
        /// </summary>
        /// <param name="listObject"></param>
        /// <returns></returns>

        [Route("AssignAccounts")]
        [HttpPost]
        public async Task<IActionResult> AssignAndUnAssignAccounts(AssignAccount listObject)
        {

            try
            {

                if (listObject == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                var myResult = await repAllot.AssignSupply(listObject.SupplyData, listObject.LoginUserID, listObject.AssignType, listObject.NeedCallRemarks, listObject.PHMID);

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
                //This is to show the error occurance place.
                //ExceptionLogging.SendErrorToText(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        public class ImportFileDetail
        {
            public int PHMID { get; set; }

            public string ActiveMenu { get; set; }

        }

        /// <summary>
        /// This is to get the imported file list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getImportFileList")]
        [HttpPost]
        public async Task<IActionResult> GetImportFileList(ImportFileDetail obj)
        {

            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repAllot.GetImportFileList(obj.PHMID, obj.ActiveMenu);

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

        public class workallotmentSummerydetails
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }

            public int ImportFileID { get; set; }
            public string StatusCode { get; set; }
            public string RoleCode { get; set; }

        }
        /// <summary>
        /// This To Get the get the accounts
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetAccountStatusSummary")]
        [HttpPost]
        public async Task<IActionResult> GetAccountStatusSummary(workallotmentSummerydetails obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repAllot.GetAccountStatusSummary(obj.PHMID, obj.UserID, obj.ImportFileID, obj.RoleCode, obj.StatusCode);

                if (objResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    AgingDetails = objResult.Item1,
                    AccountCountSummary = objResult.Item2,
                    UserSummary = objResult.Item3

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

        public class workFilterallotmentdetails
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }

            public int ImportFileID { get; set; }
            public string StatusCode { get; set; }
            public string RoleCode { get; set; }

        }
        /// <summary>
        /// This To Get the get the accounts
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetFilterdataAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetFilterdataAccounts(workFilterallotmentdetails obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repAllot.GetFilterdataAccounts(obj.PHMID, obj.UserID, obj.ImportFileID, obj.RoleCode, obj.StatusCode);

                if (objResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    AccountFilterStatusDetails = objResult.Item1,
                    AgingFilterStatuDetails = objResult.Item2,
                    AccountFilterDetails = objResult.Item3,
                    AccountFilterUserSummary = objResult.Item4,

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

        public class ClarificationResponse
        {
            public String JSONData { get; set; }
        }

        /// <summary>
        /// This is to submit clarification response details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submiClarificationResponse")]
        [HttpPost]
        public IActionResult SubmiClarificationResponse(ClarificationResponse obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                int iAffectedRows = repAllot.SubmiClarificationResponse(obj.JSONData);

                if (iAffectedRows == 0)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                return Ok(iAffectedRows);

            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
               return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public class Rebuttal
        {

            public String Request { get; set; }


        }

        /// <summary>
        /// This is for INSERTING Rebuttal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("SubmitRebuttal")]
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
                IEnumerable<dynamic> objResult = repAllot.SubmitRebuttal(obj.Request);

                if (objResult == null)
                {
                    //When expected crud operation is not done ,error message is given as NotModified.
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                return  Ok(objResult);

            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
