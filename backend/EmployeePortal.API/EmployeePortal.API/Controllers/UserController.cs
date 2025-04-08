using EmployeePortal.API.Data;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.DTOs;
using EmployeePortal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;
        private readonly EmployeeService _employeeService;
        private readonly IConfiguration _configuration;

        public UserController(EmployeeManagementContext context, EmployeeService employeeService, IConfiguration configuration)
        {
            _context = context;
            _employeeService = employeeService;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    DateOfJoining = u.DateOfJoining,
                    Designation = u.Designation,
                    Role = u.Role,
                    UserName = u.UserName
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            //var usernameClaim = GetCurrentUser();
            //if (usernameClaim == null)
            //{
            //    return Unauthorized();
            //}

            var currentUserId = await _context.Users
                                .Where(u => u.UserId == id)
                                .Select(u => u.UserId)
                                .FirstOrDefaultAsync();
            if (User.IsInRole("User") && currentUserId != id)
            {
                return Forbid();
            }

            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    DateOfJoining = u.DateOfJoining,
                    Designation = u.Designation,
                    Role = u.Role,
                    UserName = u.UserName
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            var passwordHelper = new PasswordHelper(_configuration);

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Address = userDto.Address,
                DateOfBirth = userDto.DateOfBirth,
                DateOfJoining = userDto.DateOfJoining,
                Designation = userDto.Designation,
                Role = userDto.Role,
                UserName = userDto.UserName,
                PasswordHash = passwordHelper.HashPassword("Default_Password"), // Hash the password
                PerformanceMetrics = new List<PerformanceMetric>(),
                Competencies = new List<Competency>(),
                UserGroupMembers = new List<UserGroupMember>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Users/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] PatchUserRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var passwordHelper = new PasswordHelper(_configuration);

            // Check the original password
            if (!passwordHelper.VerifyPassword(request.OriginalPassword, user.PasswordHash))
            {
                return BadRequest("Original password is incorrect.");
            }

            // Update the entity with the values from the DTO
            user.Name = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            user.Designation = request.Designation;
            user.UserName = request.UserName;

            // If a new password is provided, hash it and update the PasswordHash
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                user.PasswordHash = passwordHelper.HashPassword(request.NewPassword);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string? GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                var Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;

                return Username; //new UserModel
            }
            return null;
        }

        //[HttpPost("upload-csv")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UploadCsv(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("File is empty");

        //    using (var stream = file.OpenReadStream())
        //    {
        //        await _employeeService.AddEmployeesFromCsvAsync(stream);
        //    }

        //    return Ok("Employees added successfully");
        //}

        [HttpPost("upload-excel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using (var stream = file.OpenReadStream())
            {
                await _employeeService.AddEmployeesFromExcelAsync(stream);
            }

            return Ok("Employees added successfully");
        }
    }
}