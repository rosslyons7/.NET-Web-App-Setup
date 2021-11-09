using AuthorisationService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService.Responses {
    public record AuthenticateResponse {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public AuthenticateResponse(User user, string token) {

            Id = user.Id;
            Username = user.Username;
            Token = token;
            Role = user.Role;
        }

        public AuthenticateResponse() { }
    }
}
