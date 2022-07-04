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

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class RuleBuilderController : ControllerBase
    {
        //Repository declaration
        RuleBuilderRepository repRule = null;

        //constructor
        public RuleBuilderController()
        {
            repRule = new RuleBuilderRepository();
        }


        public struct MasterRule
        {
            public int PHMID { get; set; }
            public int StatusID { get; set; }

        }
        /// <summary>
        /// THIS FUNCTION IS TO ACTIVATE OR DEACTIVATE CLIENT HEIRARCHY MAPPING
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getMasterRule")]
        [HttpPost]
        public async Task<IActionResult> getMasterRule(MasterRule Objrule)
        {
            try
            {

                Tuple<IEnumerable<dynamic>> objResult = await repRule.getMasterRule(Objrule.PHMID, Objrule.StatusID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    AllRules = objResult.Item1



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
        public struct RuleUsers
        {
            public int PHMID { get; set; }
            public string AssignStatusCode { get; set; }

        }
        [Route("GetUsers")]
        [HttpPost]
        public async Task<IActionResult> GetUsers(RuleUsers Objrule)
        {
            try
            {

                Tuple<IEnumerable<dynamic>> objResult = await repRule.GetUsers(Objrule.PHMID, Objrule.AssignStatusCode);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    AllRules = objResult.Item1



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

        public struct RuleforUser
        {
            public int RuleID { get; set; }

        }
        [Route("GetUsersPerRule")]
        [HttpPost]
        public IActionResult GetUsersPerRule(RuleforUser obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repRule.GetUsersPerRule(obj.RuleID);

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


        /// <summary>
        /// This is to get the additional captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getRuleBuilderProject")]
        [HttpPost]
        public IActionResult GetRuleBuilderProject(MasterRule obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repRule.GetRuleBuilderProject(obj.PHMID);

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

        /// <summary>
        /// This is to get the additional captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetImportFileByPHMID")]
        [HttpPost]
        public IActionResult GetImportFilesByPHMID(MasterRule obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repRule.GetImportFilesByPHMID(obj.PHMID);

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
        public struct MasterRuleImportFile
        {
            public int PHMID { get; set; }
            public int ImportFileID { get; set; }
        }
        /// <summary>
        /// This is to get the additional captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetAccountDetailsByImportFileID")]
        [HttpPost]
        public IActionResult GetAccountDetailsByImportFileID(MasterRuleImportFile obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repRule.GetAccountDetailsByImportFileID(obj.ImportFileID, obj.PHMID);

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
              return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }


        public struct AddRule
        {
            public int PHMID { get; set; }
            public string condition { get; set; }
            public string DisplayCondition { get; set; }
            public string ImportFileId { get; set; }
            public string RuleName { get; set; }
            public string ConclusiveOutcome { get; set; }
            public string AdjustmentCode { get; set; }
            public string TransferCode { get; set; }
            public string ResubmissionCode { get; set; }
            public string BillPatientCode { get; set; }
            public string Defer_TicklerDays { get; set; }
            public string ActionInstructionToSME { get; set; }
            public string AutoNotes { get; set; }
            public string AssignStatusCode { get; set; }
            public string StatusCode { get; set; }
            public string ActionCode { get; set; }
            public string RuleDesc { get; set; }
            public int RuleTypeID { get; set; }
            public string CreatedBy { get; set; }

        }
        /// <summary>
        /// THIS FUNCTION IS TO ADD THE ADDITIONAL CAPTURE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddNewRule")]
        [HttpPost]
        public IActionResult AddNewRule(AddRule obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repRule.AddNewRule(obj.PHMID, obj.condition, obj.DisplayCondition, obj.ImportFileId, obj.RuleName, obj.RuleDesc, obj.CreatedBy, obj.RuleTypeID, obj.ConclusiveOutcome, obj.AdjustmentCode, obj.TransferCode, obj.ResubmissionCode, obj.BillPatientCode, obj.Defer_TicklerDays, obj.ActionInstructionToSME, obj.AutoNotes, obj.AssignStatusCode, obj.StatusCode, obj.ActionCode);

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


        [Route("getRuletype")]
        [HttpPost]
        public async Task<IActionResult> getRuletype()
        {
            try
            {

                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repRule.getRuletype();

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    RuletypeDetails = objResult.Item1,

                    StatusDetails = objResult.Item2
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

        public struct ChangeStatusRule
        {
            public int RuleID { get; set; }
            public bool IsActive { get; set; }
            public string UserName { get; set; }
        }
        /// <summary>
        /// THIS FUNCTION IS TO ACTIVATE OR DEACTIVATE CLIENT HEIRARCHY MAPPING
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("ChangeRuleStatus")]
        [HttpPost]
        public async Task<IActionResult> ChangeRuleStatus(ChangeStatusRule obj)
        {

            try
            {
                if (obj.RuleID == null || obj.IsActive == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repRule.ChangeRuleStatus(obj.RuleID, obj.IsActive, obj.UserName);

                if (myResult == 0)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotModified);
                }

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

        public class UserRuleType
        {
            public int UserID { get; set; }
            public int RuleID { get; set; }
            public string CreatedBy { get; set; }


        }

        /// <summary>
        /// THIS FUNCTION IS TO SAVE THE AssignRuletoUser
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AssignRuletoUser")]
        [HttpPost]
        public async Task<IActionResult> AssignRuletoUser([FromBody] List<UserRuleType> objrule)
        {
            try
            {
                //if (obj == null)
                //{
                //    //When expected parameters are not passed,error message is given as BadRequest.
                //    return Request.CreateResponse(HttpStatusCode.BadRequest);
                //}
                DataTable dt = new DataTable();
                dt.Columns.Add("RuleID", typeof(int));
                dt.Columns.Add("UserID", typeof(int));
                dt.Columns.Add("CreatedBy", typeof(string));



                DataRow drRow = dt.NewRow();

                foreach (UserRuleType obj in objrule)
                {
                    drRow = dt.NewRow();
                    drRow["RuleID"] = obj.RuleID;
                    drRow["UserID"] = obj.UserID;
                    drRow["CreatedBy"] = obj.CreatedBy;


                    dt.Rows.Add(drRow);
                }


                //  dt.Rows.Add(drRow);

                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repRule.AssignRuletoUser(dt);

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
                ExceptionLogging.SendErrorToText(ex);
               return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }




        public struct ManualRule
        {
            public int PHMID { get; set; }
            public int RuleID { get; set; }

            public int ImportFileID { get; set; }

        }
        [Route("ManualRuleExecution")]
        [HttpPost]
        public async Task<IActionResult> ManualRuleExecution(ManualRule Objrule)
        {
            try
            {

                Tuple<IEnumerable<dynamic>> objResult = await repRule.ManualRuleExecution(Objrule.PHMID, Objrule.RuleID, Objrule.ImportFileID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    AllRules = objResult.Item1



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


    }

}
