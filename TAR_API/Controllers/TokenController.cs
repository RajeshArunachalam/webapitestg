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
using TAR_API.Common;
using System.Security.Claims;
using System.IO;
using System.Text;

namespace TAR_API.Controllers
{
    [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]

    public class TokenController : ControllerBase
    {
        TokenRepository repToken = null;
        TokenService tokenService = new TokenService();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenController(IHttpContextAccessor httpContextAccessor)
        {
            repToken = new TokenRepository();
            _httpContextAccessor = httpContextAccessor;
        }
       

       

        public class TokenRequest
        {
            public string data { get; set; }
            //public string password { get; set; }
            //public string grant_type { get; set; }
        }
        #region "SaveTokenDetail"

        public sealed class Tokenn
        {
            public string Username { get; set; }
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string UserIP { get; set; }
        }
        /// <summary>
        /// This is to save token detail
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public class BearerToken
        {

            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string token_type { get; set; }
            public string userName { get; set; }
        }

        [HttpPost]
        //[Consumes("application/json")]
        public async Task<IActionResult> Token()
        {
            string RequestContent = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                RequestContent = await reader.ReadToEndAsync();
                
            }
            Dictionary<string, string> UserData = RequestContent.Split('&').Select(x => x.Split('=')).ToDictionary(key => key[0].Trim(), value => value[1].Trim());
            
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, UserData["userName"]),

        };
            bool isValid = tokenService.validatelogin(UserData["userName"], UserData["password"]);
            if (isValid == true)
            {
                var result = new BearerToken()
                {
                    access_token = tokenService.GenerateAccessToken(claims),//"TCcu1gmY9pMpjFhfDPP3rM0rO8cwJn2Db6yjWOosT7p92Iov-r7VMhzPbhL2wxmQb3_HHkL-muTApe3W-dpPYBuDy0YwuxAtfYrHIEOJQ4-0rrZMpXdw1OoXTEv8TiC4dSaVaFcreoWD3PVY0dgsnDhSvP-g8r_PT2hkMzfOLKD3oiQihM-fRw300W15aKinMLybrkZeVGZSR8fUUd4_vipOd0nY0lk8hIlWIhUf9sE",
                    expires_in = "86399",
                    refresh_token = tokenService.GenerateRefreshToken(),
                    token_type = "bearer",
                    userName = UserData["userName"],

                };
                return Ok(result);
            }
            else
            {
                return Ok("UnAuthorized User");
            }


        }
        [Route("saveTokenDetail")]
        [HttpPost]
       
        public async Task<IActionResult> SaveTokenDetail(Tokenn obj)
        {
            try
            {
                if (obj.access_token == null)
                {
                    //When expected parameters are not passed,error message is given as BadRequest.
                    return BadRequest(HttpStatusCode.BadRequest);
                }

                //This is to call the repository method.
                var myResult = await repToken.SaveTokenDetails(obj.Username, obj.access_token, obj.refresh_token, obj.UserIP);

                if (myResult == 0)
                {
                    //If there is no result is found , message is given as NotFound.
                    return NotFound(HttpStatusCode.NotModified);
                }

                // Requested data are transfered as json data.
                //return Ok(myResult);
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
    }

}
