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
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("", "")]
    public class BOTController : ControllerBase
    {

        BOTRepository repBOT = new BOTRepository();
        public static dynamic availityconfig = null;
        public class RequestBOT //priyanka
        {
            public String AccountIDs { get; set; }
            public int ServiceType { get; set; }
            public int UserID { get; set; }

            public int PHMID { get; set; }
        }

        public class AvailityManualRequest
        {
            public String JsonRequest { get; set; }
        }

        public class BotPreLoadData
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }
        }



        [Route("GetBotProcessTypePreLoadData")]
        [HttpPost]
        public async Task<IActionResult> GetBotProcessTypePreLoadData(BotPreLoadData obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repBOT.getImportBotServiceList(obj.PHMID, obj.UserID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    Services = objResult.Item1,
                    Users = objResult.Item2

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
        public class BotAccountDetails
        {
            public int PHMID { get; set; }
            public int UserID { get; set; }

            public int ImportFileID { get; set; }
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
        public async Task<IActionResult> GetAccounts(BotAccountDetails obj)
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
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objResult = await repBOT.GetAccounts(obj.PHMID, obj.UserID, obj.ImportFileID, obj.RoleCode, obj.StatusCode, dtSearch);

                if (objResult == null)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotFound);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    AccountDetails = objResult.Item1,
                    AccountSummary = objResult.Item2


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


        public class BotResponse
        {
            public int AccountID { get; set; }

        }
        [Route("GetBotResponseView")]
        [HttpPost]
        public async Task<IActionResult> GetBotResponseView(BotResponse obj)
        {
            try
            {

                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                Tuple<IEnumerable<dynamic>> objResult = await repBOT.GetBotResponseView(obj.AccountID);

                if (objResult == null)
                {
                    //If there is no result is found , ,error message is given as NoContent.
                    return NotFound(HttpStatusCode.NoContent);
                }
                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    Response = objResult.Item1


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

        [HttpPost]
        [Route("AvailityCheck")]
        public IActionResult AvailityCheck(RequestBOT obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var x = repBOT.InsertAvailityRequestAsync(obj);

                if (x == null)
                {
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                return Ok(x);

            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("ManualAvailityCheck")]
        public async Task<IActionResult> ManualAvailityCheck(AvailityManualRequest availityManualRequest)
        {
            Task<int> result = null;
            try
            {
                if (availityManualRequest == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(availityManualRequest.JsonRequest.Replace("__", "."));
                string AccID = obj.AccountID;
                string method = obj.Method;
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(obj.DATA).ToString();
                Tuple<IEnumerable<dynamic>> objResult = await repBOT.GetAvailityConfigDetails();
                availityconfig = objResult.Item1;
                string response = ConsumeAvailityAPI(jsonData, method);
                object finalobj = new { };
                //Console.WriteLine("From Availity Queue the Account ID " + AccID + " has been Pushed to API Successfully..!! ");
                if (response != "" && response != "Availity Service Error")
                {
                    result = repBOT.UpdateAvailityRequest(AccID, response, "1", availityManualRequest.JsonRequest);
                    finalobj = new
                    {
                        result = "success"
                    };
                }
                if (response == "Availity Service Error" || response == "")
                {
                    finalobj = new
                    {
                        result = "failed"
                    };

                }


                // Requested data are transfered as json data.
                return Ok(finalobj);

            }
            catch (Exception ex)
            {
                //This is to show the error occurance place.
               return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }






        public static string ConsumeAvailityAPI(string input, string method)
        {
            string res = "";
            HttpClient client = new HttpClient();
            try
            {
                client.DefaultRequestHeaders.Add("clientid", availityconfig[0].HeaderClientID);
                client.DefaultRequestHeaders.Add("user", availityconfig[0].HeaderUserID);
                HttpContent inputContent = new StringContent(input, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(availityconfig[0].AvailityURL + method + "/", inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsStringAsync().Result;
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res);
                    string Data = obj.data;
                    using (var clientg = new HttpClient())
                    {
                        var responsenew = clientg.GetAsync(availityconfig[0].AvailityURL + method + "/" + Data).GetAwaiter().GetResult();
                        if (responsenew.IsSuccessStatusCode)
                        {
                            var responseContentnew = responsenew.Content;
                            res = responseContentnew.ReadAsStringAsync().GetAwaiter().GetResult();

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                res = "Availity Service Error";
            }
            return res;
        }

    }

}
