using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;

namespace AuthorisationService.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get() {
            
            try {
                using (IDbConnection db = new MySqlConnection("Server=db,3306;Database=authorisation_db;Uid=root;Pwd=password;")) {

                    return Ok(db.Query("SELECT * FROM users"));
                }

            }
            catch (Exception e) {
                return BadRequest($"SQL_ERROR: {e}");
            }

        }
    }
}
