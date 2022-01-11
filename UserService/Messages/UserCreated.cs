using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages {
    public interface UserCreated {

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
