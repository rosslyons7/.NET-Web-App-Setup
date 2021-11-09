using AuthorisationService.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService.Messages {
    public class UserCreated {

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public UserCreated(CreateUserRequest request) {

            Id = request.Id;
            Username = request.Username;
            Role = request.Role;
            FirstName = request.FirstName;
            LastName = request.LastName;
            JobTitle = request.JobTitle;
            Email = request.Email;
            DateOfBirth = request.DateOfBirth;
        }
    }
}
