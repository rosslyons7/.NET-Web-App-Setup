using AuthorisationService.Entities;
using AuthorisationService.Requests;
using System;
using System.Threading.Tasks;

namespace AuthorisationService.Services {
    public interface IUserService {
        Task CreateUser(CreateUserRequest user);
        Task<User> GetUserByUsername(string username);
        Task DeleteUser(Guid id);
    }
}