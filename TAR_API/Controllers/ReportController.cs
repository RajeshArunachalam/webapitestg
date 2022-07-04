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
    public class ReportController : ControllerBase
    {
        private ReportRepository reportRep = null;

        public ReportController()
        {
            reportRep = new ReportRepository();
        }


        public class Report
        {
            public string Reporturl { get; set; }

            public int PHMID { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuobj"></param>
        /// <returns></returns>
        [Route("getReportRDLFileDetails")]
        [HttpPost]
        public async Task<IActionResult> GetReportRDLFileDetails(Report reportobj)
        {
            try
            {
                if (reportobj == null)
                {
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                IEnumerable<dynamic> objResult = await reportRep.GetReportRDLFileDetails(reportobj.Reporturl, reportobj.PHMID);

                if (objResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }

                // Requested data are transfered as json data.
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                ExceptionLogging.SendErrorToText(ex);
               return StatusCode(StatusCodes.Status500InternalServerError); 
            }

        }

        public struct FileDetails
        {
            public int PHMID { get; set; }
        }
        /// <summary>
        /// THIS FUNCTION IS TO GET File Details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("getFileDetails")]
        [HttpPost]
        public IActionResult GetFileDetails(FileDetails obj)
        {

            try
            {
                if (obj.PHMID == 0)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = reportRep.GetFileDetails(obj.PHMID);

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
    }

}
