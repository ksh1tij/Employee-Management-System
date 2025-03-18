using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Data;
using EmployeePortal.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly EmployeeManagementContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(EmployeeManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            var passwordHelper = new PasswordHelper(_configuration);

            if (user == null || !passwordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return new AuthResult(false, "Invalid username or password.", string.Empty);
            }

            var token = GenerateJwtToken(user);

            return new AuthResult(true, "Login successful.", token);
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
            {
                return new AuthResult(false, "Username already exists.", string.Empty);
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return new AuthResult(false, "Email already exists.", string.Empty);
            }

            var passwordHelper = new PasswordHelper(_configuration);

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                Name = request.Username, // Assuming the username is used as the name
                PasswordHash = passwordHelper.HashPassword(request.Password),
                Role = "User", // Default role, adjust as needed
                PerformanceMetrics = [],
                Competencies = [],
                UserGroupMembers = []
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResult(true, "Registration successful.", token);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Set the expiration time to 1 hour
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}