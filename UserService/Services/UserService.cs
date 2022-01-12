using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;
using Messages;
using UserService.Requests;


namespace UserService.Services {
    public class UserService : IUserService {

        private readonly string _connString;

        public async Task<IEnumerable<User>> GetUsers() {
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.QueryAsync<User>("SELECT * FROM users");
                return result;
            }
        }

        public async Task<User> GetUser(Guid id) {
            using (IDbConnection db = new MySqlConnection(_connString)) {
                var sql = "SELECT * FROM users WHERE Id=@Id";
                var result = await db.QueryFirstAsync<User>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> CreateUser(UserCreated request) {

            if (request.Id.Equals(Guid.Empty)) return 0;
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.ExecuteAsync("INSERT INTO users VALUES(@Id, @FirstName, @LastName, @Email, @JobTitle, @DateOfBirth)", request);
                return result;
            }
        }

        public async Task<int> UpdateUser(UpdateUserRequest request) {

            var sql = "UPDATE users SET ";
            var setters = new List<string>();
            if (request.FirstName != null) setters.Add("FirstName = @FirstName");
            if (request.LastName != null) setters.Add(GetSqlSetter("LastName = @LastName", setters.Count));
            if (request.Email != null) setters.Add(GetSqlSetter("Email = @Email", setters.Count));
            if (request.JobTitle != null) setters.Add(GetSqlSetter("JobTitle = @JobTitle", setters.Count));
            if (request.DateOfBirth != DateTime.MinValue) setters.Add(GetSqlSetter("DateOfBirth = @DateOfBirth", setters.Count));
            if (setters.Count == 0) throw new Exception("No update variables given.");
            foreach (var setter in setters) sql += setter;
            sql += " WHERE Id = @Id";
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.ExecuteAsync(sql, request);

                return result;
            }
        }

        private static string GetSqlSetter(string sqlSetter, int settersSize) {
            if (settersSize > 0) return $", {sqlSetter}";
            return sqlSetter;
        }

        public async Task<int> DeleteUser(UserDeleted request) {
            using (IDbConnection db = new MySqlConnection(_connString)) {

                var result = await db.ExecuteAsync("DELETE FROM users WHERE Id = @Id", new { Id = request.Id });
                return result;
            }
        }

        public UserService(IConfiguration config) {
            _connString = config.GetConnectionString("DefaultConnection");

        }
    }
}
