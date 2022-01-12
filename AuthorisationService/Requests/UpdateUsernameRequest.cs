using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService.Requests {
    public class UpdateUsernameRequest {

        public Guid Id { get; set; }
        public string NewUsername { get; set; }
        public string Password { get; set; }
    }
}
