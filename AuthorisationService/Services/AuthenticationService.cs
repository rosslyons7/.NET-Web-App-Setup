using AuthorisationService.Entities;
using AuthorisationService.Requests;
using AuthorisationService.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorisationService.Services {
    public class AuthenticationService : IAuthenticationService {
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;


        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request) {

            var user = await _userService.GetUserByUsername(request.Username);
            if (user == null) return null;

            var isPasswordCorrect = _passwordService.VerifyPassword(request.Password, user.Password);
            if (!isPasswordCorrect) return null;

            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        private string GenerateJwtToken(User user) {
            var key = Encoding.ASCII.GetBytes(_configuration["Encryption:JwtSecret"].PadLeft(32, 'X'));
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Permissions", user.Role),
                    new Claim("Id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthenticationService(IConfiguration configuration, IPasswordService passwordService, IUserService userService) {
            _configuration = configuration;
            _passwordService = passwordService;
            _userService = userService;
        }
    }
}
