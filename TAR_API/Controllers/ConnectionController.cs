using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAR_API.App_Code;

namespace TAR_API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ConnectionController> _logger;

        public ConnectionController(ILogger<ConnectionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using (SqlConnection connection = new SqlConnection(ClsCommon._ConnectionString))
            {
                try
                {
                    connection.Open();
                    return Ok("DB Connected Successfully..!!");
                }
                catch (SqlException ex)
                {
                    return Ok(ex.Message);
                }
            }

        }
    }
}
