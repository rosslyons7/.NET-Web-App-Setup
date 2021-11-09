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

        [HttpGet(nameof(GetUser))]
        public async Task<IActionResult> GetUser(Guid Id) {
            try {

                return Ok(await _userService.GetUser(Id));
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(CreateUserRequest request) {

            try {
                var result = await _userService.CreateUser(request);
                return Ok(result);
            }
            catch(Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete(nameof(DeleteUser))]
        public async Task<IActionResult> DeleteUser(Guid id) {
            try {

                var result = await _userService.DeleteUser(id);
                if (result > 0) return Ok();
                return BadRequest($"Unable to delete user with Id {id}");
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut(nameof(UpdateUser))]
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
