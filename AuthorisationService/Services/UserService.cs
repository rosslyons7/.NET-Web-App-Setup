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
using AuthorisationService.Producers;

namespace AuthorisationService.Services {
    public class UserService : IUserService {

        private readonly string _connString;
        private readonly IPasswordService _passwordService;
        private readonly IRabbitProducer _rabbit;

        public async Task<User> GetUserByUsername(string username) {

            using (var db = new MySqlConnection(_connString)) {

                string sql = "SELECT * FROM users WHERE username=@Username";
                var result = await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
                await db.DisposeAsync();
                return result;
            }

        }

        public async Task CreateUser(CreateUserRequest user) {
            using (var db = new MySqlConnection(_connString)) {
                string sql = "INSERT INTO users VALUES (@Id, @Username, @Password, @Role)";
                var hash = _passwordService.HashPassword(user.Password);
                var obj = new { user.Id, user.Username, Password = hash, user.Role };
                await db.ExecuteAsync(sql, obj);
                await db.DisposeAsync();
                _rabbit.ProduceMessage(user, "user.exchange", "user.create.*");
            }
        }


        public UserService(IConfiguration config, IPasswordService passwordService, IRabbitProducer rabbit) {


            _connString = config.GetConnectionString("DefaultConnection");
            _passwordService = passwordService;
            _rabbit = rabbit;
        }
    }
}
