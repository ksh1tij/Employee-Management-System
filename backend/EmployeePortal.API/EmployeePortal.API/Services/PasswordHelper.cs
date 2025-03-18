using System.Security.Cryptography;
using EmployeePortal.API.Data;
using EmployeePortal.API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeePortal.API.Services
{
    public class PasswordHelper
    {
        private readonly IConfiguration _configuration;

        public PasswordHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string HashPassword(string password)
        {
            var configsalt = _configuration["UserSettings:UserSalt"];
            if (string.IsNullOrEmpty(configsalt))
            {
                throw new InvalidOperationException("Password salt is not configured.");
            }
            var salt = Convert.FromBase64String(configsalt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32); // Generate a 32-byte hash
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var configsalt = _configuration["UserSettings:UserSalt"];
            if (string.IsNullOrEmpty(configsalt))
            {
                throw new InvalidOperationException("Password salt is not configured.");
            }
            var salt = Convert.FromBase64String(configsalt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hashToVerify = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hashToVerify) == hash;
        }
    }
}
