using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using Dapper;
using MySqlConnector;

namespace UserService.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get() {
            string result;
            try {
                using (IDbConnection db = new MySqlConnection("Server=db,3306;Database=user_db;Uid=root;Pwd=password;")) {

                    result = db.Query<string>("SELECT * FROM users").First();
                }

                return Ok(result);
            }catch(Exception e) {
                return BadRequest(e.Message);
            }
            
        }
    }
}
