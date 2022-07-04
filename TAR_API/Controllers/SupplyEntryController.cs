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
    public class SupplyEntryController : ControllerBase
    {
        SupplyEntryRepository repSupplyEntry = null;

        public SupplyEntryController()
        {
            repSupplyEntry = new SupplyEntryRepository();
        }

        #region "SupplyEntryAccountLoadData"

        public class PreLoadData
        {
            public int PHMID { get; set; }

        }
        /// <summary>
        /// To load data for manual account
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("supplyEntryAccountLoadData")]
        [HttpPost]
        public IActionResult SupplyEntryAccountLoadData(PreLoadData obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = repSupplyEntry.SupplyEntryAccountLoadData(obj.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    SupplyFields = objResult.Item1,
                    ScenarioDetails = objResult.Item2,
                    CalltypeDetails = objResult.Item3,
                    AdditinalCaptures = objResult.Item4,
                    Configuration = objResult.Item5,
                    SupplyFieldsItems = objResult.Item6


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

        #endregion



        /// <summary>
        /// This is to submit agent transaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitSupplyEntryTransaction")]
        [HttpPost]
        public async Task<IActionResult> SubmitSupplyEntryTransaction(SubmitSUpplyEntryTransaction objS)
        {
            try
            {
                if (objS == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                IEnumerable<dynamic> objResult = await repSupplyEntry.SubmitSupplyEntryTransaction(objS);

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


        #region "Rule Outcome info data"

        public class RuleoutcomeData
        {
            public int PHMID { get; set; }
            public string AccountIDs { get; set; }
            public int UserID { get; set; }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getManualAccountRuleOutcomeInfo")]
        [HttpPost]
        public IActionResult GetManualAccountRuleOutcomeInfo(RuleoutcomeData obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = repSupplyEntry.GetManualAccountRuleOutcomeInfo(obj.PHMID, obj.AccountIDs, obj.UserID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    RuleID = objResult.Item1,
                    RuleOutComeInfo = objResult.Item2

                };

                // Requested data are transfered as json data.
                return Ok( myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
              return StatusCode(StatusCodes.Status500InternalServerError); 
            }
        }

        #endregion


    }

}
