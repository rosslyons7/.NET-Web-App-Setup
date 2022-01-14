using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using UserService.Entities;
using Microsoft.Extensions.Configuration;
using UserService.Requests;
using UserService.Services;

namespace UserService.Controllers {



    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase {

        private readonly IUserService _userService;

        [HttpGet]
        public async Task<IActionResult> Get() {

            try {
                return Ok(await _userService.GetUsers());
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid Id) {
            try {

                return Ok(await _userService.GetUser(Id));
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request) {
            try {

                var result = await _userService.UpdateUser(request);
                if(result > 0) return Ok();
                return BadRequest($"Unable to update user with Id {request.Id}");
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }


        public UserController(IUserService userService) {

            _userService = userService;

        }
    }
}
