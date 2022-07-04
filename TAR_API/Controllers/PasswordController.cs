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
    public class PasswordController : ControllerBase
    {
        PasswordRepository repPassword = new PasswordRepository();

        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult  ResetPassword(EmployeePasswordDetails obj)
        {
            try
            {
                //This is to call the repository method.

                var myResult = repPassword.ResetPassword(obj);

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
    }

    public class EmployeePasswordDetails
    {
        public string UserID { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
    }
}
