using TAR_API.App_Code;
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
using TAR_API.Models;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class SMEController : ControllerBase
    {
        SMERepository repSME = null;

        public SMEController()
        {
            repSME = new SMERepository();
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
        [Route("getSMEPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetSMEPreLoadData(PreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repSME.GetSMEPreLoadData(obj.PHMID, obj.UserID);

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
                    Users = objResult.Item7
                };

                // Requested data are transfered as json data.
                return  Ok(myResult);
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
        public class SMEinboxdetails
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
        [Route("GetSMEAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetSMEAccounts(SMEinboxdetails obj)
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
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repSME.GetSMEAccounts(obj.PHMID, obj.UserID, obj.ImportFileID, obj.RoleCode, obj.StatusCode, dtSearch);

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
                var myResult = repSME.GetAccountRuleInfo(obj.PHMID, obj.AccountID);

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
                int iAfftedRows = await repSME.updateAccountStartedTime(obj.AccountID);

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

        /// <summary>
        /// This is to submit agent transaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitSMETransaction")]
        [HttpPost]
        public async Task<IActionResult> SubmitAssociateTransaction(SubmitSMETransaction obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repSME.SubmitSMETransaction(obj);

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
            public int PHMID { get; set; }
            public int UserID { get; set; }
            public string Query { get; set; }
            public string SubCategoryID { get; set; }
            public String AccountID { get; set; }
            public string ClarificationRoleCode { get; set; }
            public string ClarificationTo { get; set; }

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
                IEnumerable<dynamic> objResult = repSME.SubmitNonWorkable(obj.PHMID, obj.UserID, obj.AccountIDs, obj.Notes);

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
        /// This is to submit clarification details
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


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repSME.SubmitClarification(obj.PHMID, obj.UserID, obj.Query, obj.SubCategoryID, obj.AccountID, obj.ClarificationRoleCode, obj.ClarificationTo);

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
        public IActionResult GetClarification(Clarification obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repSME.GetClarification(obj.PHMID, obj.UserID, obj.AccountID);

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
