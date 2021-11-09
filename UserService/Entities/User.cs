using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace UserService.Entities {
    [Table("users")]
    public record User {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
