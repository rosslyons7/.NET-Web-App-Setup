using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService.Requests {
    public record CreateUserRequest {

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; private set; } = "User";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
