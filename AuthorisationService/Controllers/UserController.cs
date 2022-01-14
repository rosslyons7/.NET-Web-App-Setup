using AuthorisationService.Requests;
using AuthorisationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase {
        private readonly IUserService _userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request) {

            try {
                await _userService.CreateUser(request);
                return Ok();
            } 
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id) {
            try {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request) {
            try {
                await _userService.ChangePassword(request);
                return Ok();
            }
            catch(Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(nameof(UpdateUsername))]
        public async Task<IActionResult> UpdateUsername(UpdateUsernameRequest request) {
            try {
                await _userService.UpdateUsername(request);
                return Ok();
            }
            catch(Exception e) {
                return BadRequest(e.Message);
            }
        }

        public UserController(IUserService userService) {

            _userService = userService;
        }
    }
}
