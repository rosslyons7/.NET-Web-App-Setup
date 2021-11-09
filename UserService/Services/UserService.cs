using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;
using UserService.Requests;

namespace UserService.Services {
    public class UserService : IUserService {

        private readonly string _connString;

        public async Task<IEnumerable<User>> GetUsers() {
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.QueryAsync<User>("SELECT id Id, first_name FirstName, last_name LastName, email Email, job_title JobTitle, date_of_birth DateOfBirth FROM users");
                return result;
            }
        }

        public async Task<User> GetUser(Guid id) {
            using (IDbConnection db = new MySqlConnection(_connString)) {
                var sql = "SELECT id Id, first_name FirstName, last_name LastName, email Email, job_title JobTitle, date_of_birth DateOfBirth FROM users WHERE id=@Id";
                var result = await db.QueryFirstAsync<User>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> CreateUser(CreateUserRequest request) {

            if (request.Id.Equals(Guid.Empty)) return 0;
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.ExecuteAsync("INSERT INTO users VALUES(@Id, @FirstName, @LastName, @Email, @JobTitle, @DateOfBirth)", request);
                return result;
            }
        }

        public async Task<int> UpdateUser(UpdateUserRequest request) {
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.ExecuteAsync("UPDATE users SET " +
                    "first_name = @FirstName, " +
                    "last_name = @LastName, " +
                    "email = @Email, " +
                    "job_title = @JobTitle, " +
                    "date_of_birth = @DateOfBirth" +
                    "WHERE id = @Id", request);

                return result;
            }
        }

        public async Task<int> DeleteUser(Guid id) {
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.ExecuteAsync("DELETE FROM users WHERE id = @Id", new { Id = id });
                return result;
            }
        }

        public UserService(IConfiguration config) {
            _connString = config.GetConnectionString("DockerConnection");

        }
    }
}
