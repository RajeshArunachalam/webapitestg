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
    public class MasterController : ControllerBase
    {

        //Repository declaration
        MasterRepository repMaster = new MasterRepository();

        //constructor
        //public MasterController()
        //{
        //    repMaster = new MasterRepository();
        //}

        #region "MapClientMamming"
        public struct ClientMamming
        {
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string ClientCode { get; set; }
            public string ClientName { get; set; }
            public string VerticalCode { get; set; }
            public string VerticalName { get; set; }
            public string LocationCode { get; set; }
            public string LocationName { get; set; }
            public string ProjectCode { get; set; }
            public string ProjectName { get; set; }
            public string CreatedBy { get; set; }

        }

        /// <summary>
        /// THIS FUNCTION IS TO SAVE THE CLIENT HEIRARCHY MAPPING
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("clientHeirarchyMapping")]
        [HttpPost]
        public async Task<IActionResult> ClientProjectMapping(ClientMamming obj)
        {
            try
            {
                //if (obj == null)
                //{
                //    //When expected parameters are not passed,error message is given as BadRequest.
                //    return Request.CreateResponse(HttpStatusCode.BadRequest);
                //}
                DataTable dt = new DataTable();
                dt.Columns.Add("CompanyCode", typeof(string));
                dt.Columns.Add("CompanyName", typeof(string));
                dt.Columns.Add("ClientCode", typeof(string));
                dt.Columns.Add("ClientName", typeof(string));
                dt.Columns.Add("VerticalCode", typeof(string));
                dt.Columns.Add("VerticalName", typeof(string));
                dt.Columns.Add("LocationCode", typeof(string));
                dt.Columns.Add("LocationName", typeof(string));
                dt.Columns.Add("ProjectCode", typeof(string));
                dt.Columns.Add("ProjectName", typeof(string));
                dt.Columns.Add("CreatedBy", typeof(string));


                DataRow drRow = dt.NewRow();

                drRow["CompanyCode"] = obj.CompanyCode;
                drRow["CompanyName"] = obj.CompanyName;
                drRow["ClientCode"] = obj.ClientCode;
                drRow["ClientName"] = obj.ClientName;
                drRow["VerticalCode"] = obj.VerticalCode;
                drRow["VerticalName"] = obj.VerticalName;
                drRow["LocationCode"] = obj.LocationCode;
                drRow["LocationName"] = obj.LocationName;
                drRow["ProjectCode"] = obj.ProjectCode;
                drRow["ProjectName"] = obj.ProjectName;
                drRow["CreatedBy"] = obj.CreatedBy;

                dt.Rows.Add(drRow);

                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.ClientProjectMapping(dt);

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


        #endregion


        #region "User Master Mapping"

        /// <summary>
        /// THIS FUNCTION IS TO USER MAPPING DETAILS
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetMasterPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetMasterPreLoadData()
        {
            try
            {

                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repMaster.GetMasterPreLoadData();

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    ClientDetails = objResult.Item1,
                    LocationDetails = objResult.Item2,
                    ProjectDetails = objResult.Item3,
                    AllMappingDetails = objResult.Item4,
                    CompanyDetails = objResult.Item5,
                    VerticalDetails = objResult.Item6

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
        #endregion

        #region "Active/Deactivate PHM"
        public struct ChangeStatus
        {
            public int PHMID { get; set; }
            public bool IsActive { get; set; }
            public int UserID { get; set; }
        }
        /// <summary>
        /// THIS FUNCTION IS TO ACTIVATE OR DEACTIVATE CLIENT HEIRARCHY MAPPING
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("changeStatusOfPHM")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatusOfPHM(ChangeStatus obj)
        {

            try
            {
                if (obj.PHMID == null || obj.IsActive == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repMaster.ChangeStatusOfPHM(obj.PHMID, obj.IsActive);

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
        #endregion




        [Route("changeStatusOfCHM")]
        [HttpPost]
        public async Task<IActionResult> changeStatusOfCHM(ChangeStatus obj)
        {

            try
            {
                if (obj.PHMID == null || obj.IsActive == null || obj.UserID == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repMaster.changeStatusOfCHM(obj.PHMID, obj.IsActive, obj.UserID);

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
        #region "GetSupplyType"
        /// <summary>
        /// To get the Inventory type
        /// </summary>
        /// <returns></returns>

        [Route("getSupplyType")]
        [HttpGet]
        public ActionResult GetSupplyType()
        {
            try
            {
                //This is to call the repository method.
                var myRoleResult = repMaster.GetSupplyType();

                if (myRoleResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }

                // Requested data are transfered as json data.
                 return Ok(myRoleResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

            return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region "Additional Capture Master"

        public struct AdditionalCapture
        {
            public int PHMID { get; set; }
        }
        /// <summary>
        /// This is to get the additional captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetAdditionalCaptures")]
        [HttpPost]
        public IActionResult GetAdditionalCaptures(AdditionalCapture obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.GetAdditionalCaptures(obj.PHMID);

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

        public struct AdditionalCaptureRole
        {
            public int PHMID { get; set; }
            public int RoleID { get; set; }
        }
        /// <summary>
        /// Get additional captures based on role
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [Route("GetAdditionalCapturesByRole")]
        [HttpPost]
        public IActionResult GetAdditionalCapturesByRole(AdditionalCaptureRole obj)
        {
            try
            {
                var myResult = repMaster.GetAdditionalCapturesByRole(obj.PHMID, obj.RoleID);

                if (myResult == null)
                {
                    return NotFound(HttpStatusCode.NotFound);
                }
                return Ok(myResult);
            }
            catch (Exception ex)
            {
             return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This is to get the roles for the user
        /// </summary>
        /// <returns></returns>

        [Route("GetRoles")]
        [HttpPost]
        public IActionResult GetRoles()
        {
            try
            {
                var myResult = repMaster.GetRoles();
                if (myResult == null)
                {
                    return NotFound(HttpStatusCode.NotFound);
                }
               return Ok(myResult);
            }
            catch (Exception ex)
            {

              return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        public struct AddAdditionalCapture
        {
            public int RoleID { get; set; }
            public int AdditionalCaptureID { get; set; }
            public int PHMID { get; set; }
            public string FieldName { get; set; }
            public string ControlTypeID { get; set; }
            public string DataTypeID { get; set; }
            public string CommaValues { get; set; }
            public bool Required { get; set; }
            public bool IsQCEdit { get; set; }
            public int AdditionalCaptureOrder { get; set; }
        }
        /// <summary>
        /// THIS FUNCTION IS TO ADD THE ADDITIONAL CAPTURE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddAdditionalCapture")]
        [HttpPost]
        public IActionResult AddAdditionalCaptures(AddAdditionalCapture obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.SaveAdditionalCaptures(obj.RoleID, obj.PHMID, obj.FieldName, obj.ControlTypeID, obj.DataTypeID, obj.CommaValues, obj.Required, obj.IsQCEdit, obj.AdditionalCaptureOrder);

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
        /// THIS FUNCTION IS TO DISABLE OR ANABLEE ADDITIONAL CAPTURE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AdditionalEnableOrDisable")]
        [HttpPost]
        public IActionResult AdditionalEnableOrDisable(AddAdditionalCapture obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.EnableOrDisableAdditionalCapture(obj.AdditionalCaptureID);

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

        public class AdditionalCaptureRowOrder
        {
            public string AdditionalCaptureOrder { get; set; }
            public string AdditionalCaptureID { get; set; }
        }

        /// <summary>
        /// To save additional capture order
        /// </summary>
        /// <param name="AdditionalcaptureOrder"></param>
        /// <returns></returns>
        [Route("SaveAdditionalCaptureOrder")]
        [HttpPost]
        public IActionResult SaveAdditionalCaptureOrder([FromBody] List<AdditionalCaptureRowOrder> AdditionalcaptureOrder)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                DataTable dt = new DataTable();
                dt.Columns.Add("AdditionalCaptureID", typeof(int));
                dt.Columns.Add("AdditionalCaptureOrder", typeof(int));
                DataRow drRow;
                drRow = dt.NewRow();
                foreach (AdditionalCaptureRowOrder obj in AdditionalcaptureOrder)
                {
                    drRow = dt.NewRow();
                    drRow["AdditionalCaptureID"] = obj.AdditionalCaptureID;
                    drRow["AdditionalCaptureOrder"] = obj.AdditionalCaptureOrder;


                    dt.Rows.Add(drRow);
                }
                myResult = repMaster.SaveAdditionalCaptureOrder(dt);

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



        #region "Manual Entry Capture Master"

        public struct ManualCaptureDetails
        {
            public int PHMID { get; set; }
        }
        /// <summary>
        /// This is to get the additional captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetManualCaptureDetails")]
        [HttpPost]
        public IActionResult GetManualCaptureDetails(ManualCaptureDetails obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.GetManualCaptureDetails(obj.PHMID);

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

        public struct ManualCapture
        {
            public int PHMID { get; set; }

        }
        /// <summary>
        /// Get Manual Capture based on role
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [Route(" GetManualCaptureDetailsbyRole")]
        [HttpPost]
        public IActionResult GetManualCaptureDetailsbyRole(ManualCapture obj)
        {
            try
            {
                var myResult = repMaster.GetManualCaptureDetailsbyRole(obj.PHMID);

                if (myResult == null)
                {
                    return NotFound(HttpStatusCode.NotFound);
                }
                return Ok(myResult);
            }
            catch (Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        public struct AddManualCapture
        {


            public int PHMID { get; set; }
            public int FieldID { get; set; }
            public string ControlTypeID { get; set; }
            public string DataTypeID { get; set; }
            public string CommaValues { get; set; }
            public bool Required { get; set; }

        }
        /// <summary>
        /// THIS FUNCTION IS TO ADD THE Manual CAPTURE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddManualCapture")]
        [HttpPost]
        public IActionResult AddManualCaptures(AddManualCapture obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.SaveManualCaptureOrder(obj.PHMID, obj.FieldID, obj.ControlTypeID, obj.DataTypeID, obj.CommaValues, obj.Required);

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


        public struct ManualCaptures
        {

            public int ManualCaptureID { get; set; }

        }

        [Route("ManualEnableOrDisable")]
        [HttpPost]
        public IActionResult ManualEnableOrDisable(ManualCaptures obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.EnableOrDisableManualEntry(obj.ManualCaptureID);

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

        #endregion "Manual Capture Master


        #region "BotInput Details"

        public struct BotInputDetails
        {
            public int PHMID { get; set; }
        }
        /// <summary>
        /// This is to get the BotInputDetails
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetBotInputDetails")]
        [HttpPost]
        public ActionResult GetBotInputDetails(BotInputDetails obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.GetBotInputDetails(obj.PHMID);

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
        public class BotPreLoadData
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getBotInputPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetBotInputPreLoadData(BotPreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repMaster.GetBotInputPreLoadData(obj.PHMID, obj.UserID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    SupplyFields = objResult.Item1


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

        public struct AddBotInput
        {


            public int PHMID { get; set; }
            public int FieldID { get; set; }
            public string InventroryPayerName { get; set; }
            public string BotInputName { get; set; }
            public string CreatedBy { get; set; }
            public int BotInputID { get; set; }


        }
        /// <summary>
        /// THIS FUNCTION IS TO ADD THE BotInput
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddBotInput")]
        [HttpPost]
        public IActionResult addBotInputDetails(AddBotInput obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.SaveBotInputOrder(obj.PHMID, obj.FieldID, obj.InventroryPayerName, obj.BotInputName, obj.CreatedBy);

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


        [Route("UpdateBotInput")]
        [HttpPost]
        public IActionResult UpdateBOTInputDetails(AddBotInput obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.UpdateBotInputOrder(obj.PHMID, obj.FieldID, obj.InventroryPayerName, obj.BotInputName, obj.CreatedBy, obj.BotInputID);

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

        public struct BotInput
        {

            public int BotInputID { get; set; }

        }

        [Route("BotInputEnableOrDisable")]
        [HttpPost]
        public IActionResult BotInputEnableOrDisable(BotInput obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.BotInputEnableOrDisable(obj.BotInputID);

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


        #endregion






        #region "ClientDetails"
        public struct addClientDetails
        {
            public string CompanyName { get; set; }
            public string ClientName { get; set; }
            public string VerticalName { get; set; }
            public string LocationName { get; set; }
            public string ProjectName { get; set; }
            public string mode { get; set; }

        }
        [Route("addClientDetails")]
        [HttpPost]
        public async Task<IActionResult> AddClientDetails(addClientDetails obj)
        {
            try
            {
                if (obj.mode == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }


                //This is to call the repository method.
                var myResult = await repMaster.AddClientDetails(obj.CompanyName, obj.ClientName, obj.VerticalName, obj.LocationName, obj.ProjectName, obj.mode);

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
        #endregion

        #endregion "Additional Capture Master"




        #region "process flow"

        public struct ProcessFlow
        {
            public int PHMID { get; set; }
        }

        [Route("GetMasterProcessflowPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetMasterProcessflowPreLoadData(ProcessFlow ObjProcess)
        {
            try
            {

                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repMaster.GetMasterProcessflowPreLoadData(ObjProcess.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    QAUserDetails = objResult.Item1,
                    AllScenarioMappingDetails = objResult.Item2


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

        [Route("GetProcessFlowDeails")]
        [HttpPost]
        public async Task<IActionResult> GetProcessFlowDeails(ProcessFlow ObjProcess)
        {
            try
            {

                Tuple<IEnumerable<dynamic>> objResult = await repMaster.GetProcessFlowDeails(ObjProcess.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {

                    ProcessflowDetails = objResult.Item1,
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

        public struct Processflow
        {

            public int ProcessFlowID { get; set; }

        }

        [Route("EnableOrDisableProcessflow")]
        [HttpPost]
        public IActionResult EnableOrDisableProcessflow(Processflow obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.EnableOrDisableProcessflow(obj.ProcessFlowID);

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



        public struct SaveProcessflow
        {
            public int PHMID { get; set; }
            public int ScenarioMappingID { get; set; }
            public int QAUserID { get; set; }

        }
        /// <summary>
        /// THIS FUNCTION IS TO ADD THE ProcessFlows 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddProcessflow")]
        [HttpPost]
        public IActionResult AddProcessflow(SaveProcessflow obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.SaveProcessflow(obj.PHMID, obj.ScenarioMappingID, obj.QAUserID);

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
        #endregion  "process flow"

        #region "User Master Mapping"
        public struct UserDetailsMapping
        {
            public int PHMID { get; set; }
            public int EMPID { get; set; }
            public String RoleCode { get; set; }
        }

        /// <summary>
        /// THIS FUNCTION IS TO USER MAPPING DETAILS
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetUsersMappingDetails")]
        [HttpPost]
        public async Task<IActionResult> GetUserMappingDetails(UserDetailsMapping obj)
        {
            try
            {
                if (obj.PHMID == 0 || string.IsNullOrEmpty(obj.RoleCode))
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repMaster.GetUsersMappingDetails(obj.PHMID, obj.RoleCode, obj.EMPID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    UserMapped = objResult.Item1,
                    UserDetails = objResult.Item2,
                    ClientUserDetails = objResult.Item3
                };

                // Requested data are transfered as json data.
                return Ok(myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                //ExceptionLogging.SendErrorToText(ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        #endregion


        #region "User Master Details"

        /// <summary>
        /// THIS FUNCTION IS TO USER Master DETAILS
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetUserMasterPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetUserMasterPreLoadData(ProjectDetails project)
        {
            try
            {

                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repMaster.GetUserMasterPreLoadData(project.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    EmployeeTypes = objResult.Item1,
                    LeadUsers = objResult.Item2,
                    ManagerUsers = objResult.Item3,
                    Roles = objResult.Item4,
                    QCADetails = objResult.Item5,
                    UserTypes = objResult.Item6,
                    ShiftTiming = objResult.Item7

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
        public struct ProjectDetails
        {
            public int PHMID { get; set; }

        }


        public struct EmployeeDetails
        {
            public int PHMID { get; set; }
            public int EmployeeTypeID { get; set; }
            public string EmployeeID { get; set; }
            public string EmpName { get; set; }
            public string EmailID { get; set; }
            public string Designation { get; set; }
            public int Tenure { get; set; }
            public int LeadUserID { get; set; }
            public int ManagerUserID { get; set; }

            public int UserID { get; set; }
            public int ShiftID { get; set; }

        }
        /// <summary>
        /// THIS FUNCTION IS TO ADD THE Add Employee Details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddEmployeeDetails")]
        [HttpPost]
        public IActionResult AddEmployeeDetails(EmployeeDetails obj)
        {
            try
            {
                //This is to call the repository method.
                Nullable<int> myResult = new int?();
                myResult = repMaster.AddEmployeeDetails(obj.PHMID, obj.EmployeeTypeID, obj.EmployeeID, obj.EmpName, obj.EmailID, obj.Designation, obj.Tenure, obj.LeadUserID, obj.ManagerUserID, obj.UserID, obj.ShiftID);

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
        /// GetUsersMappingDetailsEmpID
        /// </summary>
        public class GetUsersMappingDetailsEmpID
        {
            public string EmpId { get; set; }
            public string EmailID { get; set; }

        }
        /// <summary>
        /// This To Get the Client Access Mapping details Based on EmpID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("GetUsersMappingDetailsByEmpID")]
        [HttpPost]

        public async Task<IActionResult> GetUsersMappingDetailsByEmpID(GetUsersMappingDetailsEmpID obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repMaster.GetClientAccessMappingByEmpID(obj.EmpId, obj.EmailID);

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
                // ExceptionLogging.SendErrorToText(ex);
              return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public class UserDetails
        {
            public int PHMID { get; set; }
            public int ClientID { get; set; }
            public int LocationID { get; set; }
            public int ProjectID { get; set; }
            public string EMPID { get; set; }
            public int? QAID { get; set; }
            public int Target { get; set; }
            public int RoleID { get; set; }
            public int UserTypeID { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            public string loginNTLG { get; set; }
            public bool IsActive { get; set; }
            public int Audit { get; set; }
        }


        /// <summary>
        /// To save mapped user
        /// </summary>
        /// <param name="listObject"></param>
        /// <returns></returns>
        [Route("SaveUserMappingData")]
        [HttpPost]
        public async Task<IActionResult> SaveMappUserData([FromBody] List<UserDetails> listObject)
        {
            try
            {
                var loginNtlg = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("PHMID", typeof(int));
                dt.Columns.Add("ClientID", typeof(int));
                dt.Columns.Add("LocationID", typeof(int));
                dt.Columns.Add("projectID", typeof(int));
                dt.Columns.Add("EMPID", typeof(string));
                dt.Columns.Add("QAID", typeof(int));
                dt.Columns.Add("Target", typeof(int));
                dt.Columns.Add("Audit", typeof(int));
                dt.Columns.Add("RoleID", typeof(int));
                dt.Columns.Add("UserTypeID", typeof(int));
                dt.Columns.Add("UserName", typeof(string));
                dt.Columns.Add("UserEmail", typeof(string));
                dt.Columns.Add("IsActive", typeof(bool));
                DataRow drRow;
                drRow = dt.NewRow();

                foreach (UserDetails obj in listObject)
                {
                    drRow = dt.NewRow();
                    drRow["PHMID"] = obj.PHMID;
                    drRow["ClientID"] = obj.ClientID;
                    drRow["LocationID"] = obj.LocationID;
                    drRow["projectID"] = obj.ProjectID;
                    drRow["EMPID"] = obj.EMPID;
                    drRow["QAID"] = (obj.QAID == 0 || obj.QAID == null) ? (object)DBNull.Value : obj.QAID;
                    drRow["Target"] = obj.Target;
                    drRow["Audit"] = obj.Audit;
                    drRow["RoleID"] = obj.RoleID;
                    drRow["UserTypeID"] = obj.UserTypeID;
                    drRow["UserName"] = obj.UserName;
                    drRow["UserEmail"] = obj.UserEmail;
                    drRow["IsActive"] = obj.IsActive;
                    loginNtlg = obj.loginNTLG;

                    dt.Rows.Add(drRow);
                }

                //This is to call the repository method.
                var myRoleResult = await repMaster.SaveMappUserData(dt, loginNtlg);

                if (myRoleResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }

                // Requested data are transfered as json data.
                return Ok(myRoleResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                //ExceptionLogging.SendErrorToText(ex);
             return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        public struct AccountHistoryDetails
        {
            public int PHMID { get; set; }
            public string AccountNo { get; set; }
        }
        /// <summary>
        /// This is to get the Case history details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [Route("getAccountHistoryDetails")]
        [HttpPost]
        public IActionResult GetAccountHistoryDetails(AccountHistoryDetails obj)
        {

            try
            {
                //This is to call the repository method.
                var myResult = repMaster.GetAccountHistoryDetails(obj.PHMID, obj.AccountNo);

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


        public struct AccountBotReponseDetails
        {
            public string AccountNo { get; set; }
        }

        [Route("GetAccountBotReponseDetails")]
        [HttpPost]
        public IActionResult GetAccountBotReponseDetails(AccountBotReponseDetails obj)
        {

            try
            {
                //This is to call the repository method.
                var myResult = repMaster.GetAccountBotReponseDetails(obj.AccountNo);

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
        [Route("getManualEntryPreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetManualEntryPreLoadData(PreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repMaster.GetManualEntryPreLoadData(obj.PHMID, obj.UserID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    SupplyFields = objResult.Item1


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

        #region "addmanauldata"

        public class AddManaulCapture
        {
            public int ManualCaptureID { get; set; }
            public int PHMID { get; set; }
            public int FieldID { get; set; }
            public string ControlTypeID { get; set; }
            public string CommaValues { get; set; }
            public bool Required { get; set; }
            public int LoginID { get; set; }
        }
        /// <summary>
        /// To add Fileds for manual account transaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("AddManaulCaptures")]
        [HttpPost]
        public IActionResult AddManaulCaptures(AddManaulCapture obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.SaveManaulCapture(obj.PHMID, obj.FieldID, obj.ControlTypeID, obj.CommaValues, obj.Required, obj.LoginID);

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




        public class ManaulCapturedata
        {
            public int PHMID { get; set; }
        }
        /// <summary>
        /// To get manual captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getManualCaptures")]
        [HttpPost]
        public IActionResult GetManualCaptures(ManaulCapturedata obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.GetManualCaptures(obj.PHMID);

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
        /// To delete the manual captures
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [Route("DeleteManualCapture")]
        [HttpPost]
        public IActionResult DeleteManualCapture(AddManaulCapture obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.DeleteManualCapture(obj.ManualCaptureID);

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

        #endregion "addmanauldata"

        public class ResetdataObj
        {
            public int PHMID { get; set; }
        }
        [Route("resetdata")]
        [HttpPost]
        public IActionResult resetdata(ResetdataObj obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.ResetData(obj.PHMID);

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

        #region "SearcAccountDetail"
        public class SearchAccountString
        {
            public int PHMID { get; set; }
            public string stringToFind { get; set; }
        }
        [Route("SearcAccountInformation")]
        [HttpPost]
        public IActionResult SearchAccountInformation(SearchAccountString obj)
        {
            try
            {
                //This is to call the repository method.
                var myResult = repMaster.SearchAccountInformation(obj.PHMID, obj.stringToFind);

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
        #endregion
    }

}
