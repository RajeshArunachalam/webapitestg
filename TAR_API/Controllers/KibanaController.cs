using TAR_API.App_Code;
using TAR_API.Common;
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
    public class KibanaController : ControllerBase
    {
        KibanaErrorHandler kibanaErrorHandler = new KibanaErrorHandler();

        [HttpGet]
        [Route("Start")]
        public void KibanaServiceTest()
        {
            kibanaErrorHandler.CheckIndex();
        }

        [HttpGet]
        [Route("Success")]
        public void Success()
        {
            kibanaErrorHandler.Success();
        }

        [HttpGet]
        [Route("Error")]
        public void KibanaErrorTest()
        {
            kibanaErrorHandler.Error();
        }
    }

}
