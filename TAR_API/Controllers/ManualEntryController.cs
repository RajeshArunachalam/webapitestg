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
using Newtonsoft.Json;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class ManualEntryController : ControllerBase
    {
        ManualEntryRepository repManual = null;

        public ManualEntryController()
        {
            repManual = new ManualEntryRepository();
        }

        #region "manualAccountLoadData"

        public class PreLoadData
        {
            public int PHMID { get; set; }

        }
        /// <summary>
        /// To load data for manual account
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("manualAccountLoadData")]
        [HttpPost]
        public IActionResult  manualAccountLoadData(PreLoadData obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = repManual.ManualAccountLoadData(obj.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    SupplyFields = objResult.Item1,
                    EPICCategoryDetails = objResult.Item2,
                    ConfigurationDetails = objResult.Item3,

                    SupplyFieldsItems = objResult.Item4


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

        #region "submitAgentTransaction"
        public class ManualAgentTransaction
        {
            public int PHMID { get; set; }
            public String AccountIDs { get; set; }
            public string Note { get; set; }
            public int UserID { get; set; }
            public string DistinctID { get; set; }
            public string UserName { get; set; }


        }

        /// <summary>
        /// This is to submit agent transaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("submitmanualTransaction")]
        [HttpPost]
        public IActionResult  submitmanualTransaction(ManualAgentTransaction obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                #region "AccountIDs"

                DataTable dtManualAccountDetailsValues = new DataTable();
                dtManualAccountDetailsValues.Columns.Add("FieldColumnID", typeof(int));
                dtManualAccountDetailsValues.Columns.Add("FieldColumnName", typeof(string));
                dtManualAccountDetailsValues.Columns.Add("Value", typeof(string));

                dynamic dynJson = JsonConvert.DeserializeObject(obj.AccountIDs);
                foreach (var item in dynJson)
                {
                    DataRow drRow = dtManualAccountDetailsValues.NewRow();
                    drRow["FieldColumnID"] = item.FieldID;
                    drRow["FieldColumnName"] = item.FieldName;
                    drRow["Value"] = item.Value;
                    dtManualAccountDetailsValues.Rows.Add(drRow);
                }


                #endregion



                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repManual.SubmitMaualAgentTransaction(obj.PHMID, obj.UserID, obj.DistinctID, obj.Note, obj.UserName, dtManualAccountDetailsValues);

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

        #endregion


    }

}
