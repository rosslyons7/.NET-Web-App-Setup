using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using AuthorisationService.Entities;
using MySqlConnector;
using AuthorisationService.Requests;
using Newtonsoft.Json;
using Messages;
using MassTransit;

namespace AuthorisationService.Services {
    public class UserService : IUserService {

        private readonly string _connString;
        private readonly IPasswordService _passwordService;
        private readonly IPublishEndpoint _publishEndpoint;

        public async Task<User> GetUserByUsername(string username) {

            using (var db = new MySqlConnection(_connString)) {

                string sql = "SELECT * FROM users WHERE username=@Username";
                var result = await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
                await db.DisposeAsync();
                return result;
            }

        }

        public async Task CreateUser(CreateUserRequest user) {

            using var db = new MySqlConnection(_connString); 
            string sql = "INSERT INTO users VALUES (@Id, @Username, @Password, @Role)";
            var hash = _passwordService.HashPassword(user.Password);
            var obj = new { user.Id, user.Username, Password = hash, user.Role };
            await db.ExecuteAsync(sql, obj);
            await db.DisposeAsync();
            await _publishEndpoint.Publish<UserCreated>(new
            {
                user.Id,
                user.Username,
                user.Role,
                user.FirstName,
                user.LastName,
                user.JobTitle,
                user.Email,
                user.DateOfBirth
            });

        }

        public async Task DeleteUser(Guid id) {
            using var db = new MySqlConnection(_connString);
            var sql = "DELETE FROM users WHERE Id=@Id";
            await db.ExecuteAsync(sql, new { Id = id });
            await _publishEndpoint.Publish<UserDeleted>(new
            {
                Id = id
            });
        }

        public async Task UpdateUsername(UpdateUsernameRequest request) {

            using var db = new MySqlConnection(_connString);
            string sql = "SELECT * FROM users WHERE Id=@Id";
            var result = await db.QueryFirstOrDefaultAsync<User>(sql, new { request.Id });

            if (_passwordService.VerifyPassword(request.Password, result.Password)) {

                await db.ExecuteAsync("UPDATE users SET username=@NewUsername WHERE Id=@Id", new { request.NewUsername, request.Id });
                await db.DisposeAsync();
            }
            else {
                throw new Exception("Incorrect password.");
            }


            await db.DisposeAsync();
        }

        public async Task ChangePassword(ChangePasswordRequest request) {

            using var db = new MySqlConnection(_connString);
            string sql = "SELECT * FROM users WHERE Id=@Id";
            var result = await db.QueryFirstOrDefaultAsync<User>(sql, new { request.Id });

            if( _passwordService.VerifyPassword(request.OldPassword, result.Password)) {

                var hash = _passwordService.HashPassword(request.NewPassword);
                string updateSql = "UPDATE users SET password=@Hash WHERE Id=@Id";
                await db.ExecuteAsync(updateSql, new { Hash = hash, request.Id });
                await db.DisposeAsync();
            }
            else {
                throw new Exception("Incorrect password.");
            }
        }

        

        public UserService(IConfiguration config, IPasswordService passwordService, IPublishEndpoint publishEndpoint) {


            _connString = config.GetConnectionString("DefaultConnection");
            _passwordService = passwordService;
            _publishEndpoint = publishEndpoint;
        }
    }
}
