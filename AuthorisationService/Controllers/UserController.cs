﻿using AuthorisationService.Requests;
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

        [HttpPost(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(CreateUserRequest request) {

            try {
                await _userService.CreateUser(request);
                return Ok();
            }catch(Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        public UserController(IUserService userService) {

            _userService = userService;
        }
    }
}