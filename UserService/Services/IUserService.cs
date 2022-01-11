using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Entities;
using Messages;
using UserService.Requests;

namespace UserService.Services {
    public interface IUserService {
        Task<int> CreateUser(UserCreated request);
        Task<int> DeleteUser(UserDeleted request);
        Task<User> GetUser(Guid id);
        Task<IEnumerable<User>> GetUsers();
        Task<int> UpdateUser(UpdateUserRequest request);
    }
}