
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthorisationService.Services {


    public class PasswordService : IPasswordService {

        private readonly IConfiguration _config;

        public string HashPassword(string password) {
            var hashIterations = int.Parse(_config["Encryption:HashIterations"]);
            var hashSaltSize = int.Parse(_config["Encryption:HashSaltSize"]);
            var hashSize = int.Parse(_config["Encryption:HashSize"]);
            //Create salt
            using (var rng = new RNGCryptoServiceProvider()) {
                byte[] salt;
                rng.GetBytes(salt = new byte[hashSaltSize]);
                // Combine hash and salt, convert to base64 and format hash with extra Information
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, hashIterations)) {
                    var hash = pbkdf2.GetBytes(hashSize);
                    var hashBytes = new byte[hashSaltSize + hashSize];
                    Array.Copy(salt, 0, hashBytes, 0, hashSaltSize);
                    Array.Copy(hash, 0, hashBytes, hashSaltSize, hashSize);
                    var base64Hash = Convert.ToBase64String(hashBytes);
                    return $"$HASH|V1${hashIterations}${base64Hash}";
                }
            }
        }

        public bool VerifyPassword(string password, string hashedPassword) {

            var hashSaltSize = int.Parse(_config["Encryption:HashSaltSize"]);
            var hashSize = int.Parse(_config["Encryption:HashSize"]);
            if (!IsHashSupported(hashedPassword)) throw new NotSupportedException("The hash type is not supported");

            //Exctract information and Base64 string
            var splittedHashString = hashedPassword.Replace("$HASH|V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //Get salt
            var salt = new byte[hashSaltSize];
            Array.Copy(hashBytes, 0, salt, 0, hashSaltSize);

            // Create hash with given salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations)) {
                byte[] hash = pbkdf2.GetBytes(hashSize);
                // Get result
                for (var i = 0; i < hashSize; i++) {
                    if (hashBytes[i + hashSaltSize] != hash[i]) {
                        return false;
                    }
                }
                return true;
            }
        }

        private static bool IsHashSupported(string hashString) {
            return hashString.Contains("HASH|V1$");
        }


        public PasswordService(IConfiguration config) {
            _config = config;
        }
    }
}
