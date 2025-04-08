using EmployeePortal.API.Data;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDepartmentController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;

        public UserDepartmentController(EmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: api/Departments/{departmentId}/Users
        [HttpGet("{departmentId}/Users")]
        public async Task<ActionResult<IEnumerable<UserDepartment>>> GetUsersByDepartmentId(int departmentId)
        {
            var userDepartmentMembers = await _context.UserDepartments
                .Where(ud => ud.DepartmentId == departmentId)
                .ToListAsync();

            if (userDepartmentMembers == null || userDepartmentMembers.Count == 0)
            {
                return NotFound();
            }

            return userDepartmentMembers;
        }

        // GET: api/UserDepartments/User/{userId}
        [HttpGet("User/{userId}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<ActionResult<IEnumerable<UserDepartment>>> GetUserDepartments(int userId)
        {
            //var usernameClaim = GetCurrentUser();
            //if (usernameClaim == null)
            //{
            //    return Unauthorized();
            //}

            var currentUserId = await _context.Users
                                .Where(u => u.UserId == userId)
                                .Select(u => u.UserId)
                                .FirstOrDefaultAsync();
            if (User.IsInRole("User") && currentUserId != userId)
            {
                return Forbid();
            }

            var userDepartments = await _context.UserDepartments
                .Where(ud => ud.UserId == userId)
                .ToListAsync();

            if (userDepartments == null || userDepartments.Count == 0)
            {
                return NotFound();
            }

            return userDepartments;
        }

        // GET: api/UserDepartments/Manager/{managerId}
        [HttpGet("Manager/{managerId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersUnderManager(int managerId)
        {
            var departments = await _context.Departments
                .Where(d => d.ManagerId == managerId)
                .Include(d => d.UserDepartments)
                .ToListAsync();

            if (departments == null || departments.Count == 0)
            {
                return NotFound();
            }

            var userIds = departments.SelectMany(d => d.UserDepartments.Select(ud => ud.UserId)).Distinct().ToList();
            var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();

            return users;
        }

        // POST: api/UserDepartments
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<UserDepartment>> PostUserDepartment(UserDepartmentDto userDepartmentDto)
        {
            var user = await _context.Users.FindAsync(userDepartmentDto.UserId);
            var department = await _context.Departments.FindAsync(userDepartmentDto.DepartmentId);

            if (user == null || department == null)
            {
                return BadRequest("User or Department not found.");
            }

            var userDepartment = new UserDepartment
            {
                UserId = userDepartmentDto.UserId,
                DepartmentId = userDepartmentDto.DepartmentId,
                Department = department
            };

            _context.UserDepartments.Add(userDepartment);
            await _context.SaveChangesAsync();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(userDepartment, options);

            return CreatedAtAction("GetUserDepartments", new { userId = userDepartment.UserId }, jsonString);
        }

        // DELETE: api/UserDepartments/{userId}/{departmentId}
        [HttpDelete("{userId}/{departmentId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteUserDepartment(int userId, int departmentId)
        {
            var userDepartment = await _context.UserDepartments
                .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DepartmentId == departmentId);
            if (userDepartment == null)
            {
                return NotFound();
            }

            _context.UserDepartments.Remove(userDepartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/UserDepartments/{userId}/{departmentId}
        [HttpPatch("{userId}/{departmentId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchUserDepartment(int userId, int departmentId, [FromBody] UserDepartmentDto userDepartmentDto)
        {
            if (userDepartmentDto == null)
            {
                return BadRequest();
            }

            var userDepartment = await _context.UserDepartments
                .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DepartmentId == departmentId);
            if (userDepartment == null)
            {
                return NotFound();
            }

            // Remove the existing UserDepartment entity
            _context.UserDepartments.Remove(userDepartment);
            await _context.SaveChangesAsync();

            // Create a new UserDepartment entity with the updated composite key values
            var newUserDepartment = new UserDepartment
            {
                UserId = userDepartmentDto.UserId,
                DepartmentId = userDepartmentDto.DepartmentId
            };

            _context.UserDepartments.Add(newUserDepartment);
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
    }
}