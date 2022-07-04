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
using System.Threading.Tasks;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("","")]
    public class LoginController : ControllerBase
    {
        //Repository declaration
        MasterRepository repMaster = null;
        LoginRepository repLogin = null;
        //constructor
        public LoginController()
        {
            repMaster = new MasterRepository();
            repLogin = new LoginRepository();
        }

        #region "checkLogin"

        private sealed class clsLogin
        {
            public bool LoginStatus { get; set; }

            public string Password { get; set; }
            public bool IsMapped { get; set; }
            public int UserID { get; set; }
            public string EmployeeID { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public string EmailAddress { get; set; }
            public string Token { get; set; }
            public bool SuperUser { get; set; }
            public int HistoryID { get; set; }

        }

        /// <summary>
        /// This is to check login details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [Route("CheckLogin")]
        [HttpPost]
        public async Task<ActionResult> CheckLogin(LoginModel obj)
        {
            try
            {
                if (obj == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                clsLogin objLogin = new clsLogin();
                objLogin.LoginStatus = true;
                objLogin.IsMapped = false;

                var accessMapping = (dynamic)null;
                var userdata = (dynamic)null;
                //This is to call the repository method.
                IEnumerable<dynamic> checkUser = await repMaster.GetUserDetails(obj.Username, obj.UserIP, ClsCommon.ApplicationName, objLogin.SuperUser, obj.ApplicationVersion);
                if (checkUser.Count() > 0)
                {
                    objLogin.IsMapped = true;

                    foreach (var user in checkUser)
                    {
                        userdata = user;
                    }
                    objLogin.UserID = userdata.UserID;
                    objLogin.Name = userdata.Name;
                    objLogin.EmployeeID = (userdata.EmployeeID).ToString();
                    objLogin.UserName = obj.Username;
                    objLogin.HistoryID = userdata.HistoryID;
                    objLogin.Password = userdata.Password;

                    //This is to call the repository method.
                    accessMapping = await repMaster.GetClientProjectMapping(userdata.UserID, ClsCommon.GetLocalIPAddress(), obj.ApplicationVersion);

                }

                //Tuple results are assigned to variable and returns as json data.
                var myResult = new
                {
                    Login = objLogin,
                    AccessMapping = accessMapping
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

        #region "UpdateLogoutTime"

        public class LogoutTime
        {
            public int HistoryID { get; set; }
        }
        /// <summary>
        /// This is to update logout time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 
        //[AllowAnonymous]
        [Route("updateLogoutTime")]
        [HttpPost]
        public async Task<IActionResult> UpdateLogoutTime(LogoutTime obj)
        {

            try
            {
                if (obj.HistoryID < 0)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repLogin.UpdateLogoutTime(obj.HistoryID);

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
            finally
            {
                try
                {

                    //HttpContext.Current.Session.Clear();
                    //HttpContext.Current.Session.Abandon();

                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
                }
                catch (Exception ex)
                {
                    // ExceptionLogging.SendErrorToText(ex);
                }
            }
        }

        #endregion

    }

}
