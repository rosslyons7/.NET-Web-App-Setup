using AuthorisationService.Requests;
using AuthorisationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase {

        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        [HttpPost(nameof(Authenticate))]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request) {

            try {
                var result = await _authenticationService.Authenticate(request);
                return result != null ? Ok(result) : StatusCode(401, new { Error = "Invalid username/password combination." });
            }catch(Exception e) {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return StatusCode(500, e.Message);
            }
        }

        public AuthenticationController(IAuthenticationService authenticateService, ILogger<AuthenticationController> logger) {

            _authenticationService = authenticateService;
            _logger = logger;
        }
    }
}
