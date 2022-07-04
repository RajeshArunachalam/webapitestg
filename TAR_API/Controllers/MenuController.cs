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
    public class MenuController : ControllerBase
    {
        private MenuRepository menuRep = null;

        public MenuController()
        {
            menuRep = new MenuRepository();
        }

        public class Menu
        {
            public string RoleCode { get; set; }
            public int UserID { get; set; }
            public int PHMID { get; set; }
        }
        /// <summary>
        /// This is to get the Menudetails directly with roleid
        /// </summary>
        /// <param name="menuobj"></param>
        /// <returns></returns>
        [Route("getAllMenu")]
        [HttpPost]
        public async Task<IActionResult> GetMenuDetails(Menu menuobj)
        {
            try
            {
                if (menuobj == null)
                {
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                IEnumerable<dynamic> objResult = await menuRep.GetAllMenu(menuobj.UserID, menuobj.RoleCode, menuobj.PHMID);

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
                // ExceptionLogging.SendErrorToText(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
                }

        }

    }

}
