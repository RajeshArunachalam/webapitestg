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
    public class DashboardController : ControllerBase
    {
        DashboardRepository repDashboard = new DashboardRepository();
        [Route("GetProductionHourChart")]
        [HttpPost]
        public async Task<IActionResult> GetProductionHourChart(HourChartData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repDashboard.GetProductionHourChart(obj.UserID, obj.CompletedOn, obj.PHMID);



                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                var myResult = new
                {

                    HourChart = objResult.Item1,
                    StatusCount = objResult.Item2,
                    Target = objResult.Item3


                };

                // Requested data are transfered as json data.
                return Ok(myResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

             return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Route("GetQCAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetQCAnalysis(HourChartData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repDashboard.GetQCAnalysis(obj.UserID, obj.PHMID);



                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                var myResult = new
                {

                    MTD = objResult.Item1,
                    AccuracyPer = objResult.Item2,
                    ErrorPer = objResult.Item3


                };

                // Requested data are transfered as json data.
                return Ok(myResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

              return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        public class ProductivityAnalysisData
        {
            public string UserId { get; set; }
            public string analysis { get; set; }
            public string flag { get; set; }
            public int PHMID { get; set; }
            public string fromdate { get; set; }
            public string todate { get; set; }

        }

        [Route("GetProductivityAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetProductivityAnalysis(ProductivityAnalysisData obj)
        {
            try
            {

                var myResult = new object();
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                if (obj.analysis != "E")
                {
                    //This is to call the repository method.
                    Tuple<IEnumerable<dynamic>> objResult = await repDashboard.GetProductivityAnalysis(obj.UserId, obj.analysis, obj.PHMID, obj.flag, obj.fromdate, obj.todate);

                    //Tuple results are assigned to variable and returns as json data.
                    if (objResult == null)
                    {
                        return NotFound(HttpStatusCode.NotModified);
                    }

                    if (obj.analysis == "P")
                    {
                        myResult = new
                        {
                            productivity = objResult.Item1
                        };
                    }
                    else if (obj.analysis == "Q")
                    {
                        myResult = new
                        {
                            quality = objResult.Item1
                        };
                    }
                    else if (obj.analysis == "T")
                    {
                        myResult = new
                        {
                            Time = objResult.Item1
                        };
                    }
                    else if (obj.analysis == "A")
                    {
                        myResult = new
                        {
                            AHT = objResult.Item1
                        };
                    }
                    else if (obj.analysis == "PR")
                    {
                        myResult = new
                        {
                            Performance = objResult.Item1
                        };
                    }

                }
                else if (obj.analysis == "E")
                {

                    //This is to call the repository method.
                    Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repDashboard.GetErrorAnalysis(obj.UserId, obj.analysis, obj.PHMID, obj.flag, obj.fromdate, obj.todate);

                    //Tuple results are assigned to variable and returns as json data.
                    if (objResult == null)
                    {
                        return NotFound(HttpStatusCode.NotModified);
                    }
                    myResult = new
                    {
                        Error = objResult.Item1,
                        SubError = objResult.Item2
                    };

                }
                // Requested data are transfered as json data.
                return Ok(myResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public class InventoryAnalysisData
        {
            public string fromdate { get; set; }
            public string todate { get; set; }
            public int PHMID { get; set; }
            public string ImportFileIDs { get; set; }
            public string UserId { get; set; }

        }

        [Route("GetInventoryAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetInventoryAnalysis(InventoryAnalysisData obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repDashboard.GetInventoryAnalysis(obj.fromdate, obj.todate, obj.PHMID, obj.ImportFileIDs, obj.UserId);

                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }
                var myResult = new
                {
                    Inventory = objResult.Item1
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



        [Route("GetQualityAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetQualityAnalysis(HourChartData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repDashboard.GetQualityAnalysis(obj.UserID, obj.PHMID);



                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                var myResult = new
                {


                    Category = objResult.Item1,
                    SubCategory = objResult.Item2


                };

                // Requested data are transfered as json data.
                return Ok(myResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
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




        [Route("GetAllUsers")]
        [HttpPost]
        public async Task<IActionResult> GetAllUsers(PreLoadData obj)
        {
            try
            {



                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }



                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repDashboard.GetAllUsers(obj.PHMID, obj.UserID);



                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    Users = objResult.Item1
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

        [Route("GetProductionPerformanceChart")]
        [HttpPost]
        public async Task<IActionResult> GetProductionPerformanceChart(PerformanceChartData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repDashboard.GetProductionPerformanceChart(obj.UserID, obj.PHMID, obj.FromDate, obj.ToDate);



                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                var myResult = new
                {


                    PerformanceChart = objResult.Item1

                };

                // Requested data are transfered as json data.
                return Ok(myResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

              return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Route("GetAgentQualityChart")]
        [HttpPost]
        public async Task<IActionResult> GetAgentQualityChart(AgentQCData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repDashboard.GetAgentQualityAnalysis(obj.UserID, obj.PHMID);



                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                var myResult = new
                {

                    Overall = objResult.Item1,
                    DayWiseError = objResult.Item2,
                    DailyIQ = objResult.Item3,
                    WeeklyIQ = objResult.Item4


                };

                // Requested data are transfered as json data.
                return Ok(myResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

              return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        public class AgentQCData
        {
            public int UserID { get; set; }
            public int PHMID { get; set; }

        }




        public class HourChartData
        {
            public int UserID { get; set; }
            public string CompletedOn { get; set; }
            public int PHMID { get; set; }


        }

        public class PerformanceChartData
        {
            public int UserID { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public int PHMID { get; set; }


        }





        [Route("GetUserDetailsbyCondition")]
        [HttpPost]
        public async Task<IActionResult> GetUserDetailsbyCondition(GetUserDetailsbyConditionClass obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                IEnumerable<dynamic> objResult = repDashboard.GetUserDetailsbyCondition(obj.UserID, obj.RoleCode, obj.PHMID);



                //Tuple results are assigned to variable and returns as json data.
                if (objResult == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                return Ok(objResult);


                // Requested data are transfered as json data.
                //return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.

               return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public class GetUserDetailsbyConditionClass
        {
            public int UserID { get; set; }
            public string RoleCode { get; set; }
            public int PHMID { get; set; }
        }
    }

}
