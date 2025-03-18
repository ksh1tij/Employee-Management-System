using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Data;
using EmployeePortal.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;

        public UserGroupController(EmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: api/UserGroups/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroup>> GetUserGroupById(int id)
        {
            var userGroup = await _context.UserGroups.FindAsync(id);

            if (userGroup == null)
            {
                return NotFound();
            }

            return userGroup;
        }

        // GET: api/UserGroups/User/{userId}
        [HttpGet("User/{userId}")]
        //[Authorize(Roles = "Admin,Manager,User")]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups(int userId)
        {
            var currentUserId = await _context.Users
                                .Where(u => u.UserId == userId)
                                .Select(u => u.UserId)
                                .FirstOrDefaultAsync();
            if (User.IsInRole("User") && currentUserId != userId)
            {
                return Forbid();
            }

            var userGroups = await _context.UserGroupMembers
                .Where(ugm => ugm.UserId == userId)
                .Select(ugm => ugm.GroupId)
                .ToListAsync();

            if (userGroups == null || userGroups.Count == 0)
            {
                return NotFound();
            }

            var groups = await _context.UserGroups
                .Where(ug => userGroups.Contains(ug.GroupId))
                .ToListAsync();

            return groups;
        }

        // GET: api/UserGroups/Manager/{managerId}
        [HttpGet("Manager/{managerId}")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersUnderManager(int managerId)
        {
            var departments = await _context.Departments
                .Where(d => d.ManagerId == managerId)
                .Include(d => d.UserDepartments)
                .ToListAsync();

            var userIds = departments
                .SelectMany(d => d.UserDepartments.Select(ud => ud.UserId))
                .Distinct()
                .ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.UserId))
                .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            return users;
        }

        // POST: api/UserGroups
        [HttpPost]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<UserGroup>> PostUserGroup(UserGroupDto userGroupDto)
        {
            var userGroup = new UserGroup
            {
                GroupName = userGroupDto.GroupName
            };

            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(userGroup, options);

            return CreatedAtAction(nameof(GetUserGroupById), new { id = userGroup.GroupId }, jsonString);
        }

        // DELETE: api/UserGroups/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserGroup(int id)
        {
            var userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            // Find all user-group member relationships for the given groupId
            var userGroupMembers = _context.UserGroupMembers.Where(ugm => ugm.GroupId == id).ToList();
            if (userGroupMembers.Any())
            {
                _context.UserGroupMembers.RemoveRange(userGroupMembers);
            }

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/UserGroups/{id}
        [HttpPatch("{id}")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchUserGroup(int id, [FromBody] UserGroupDto userGroupDto)
        {
            if (userGroupDto == null)
            {
                return BadRequest();
            }

            var userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            userGroup.GroupName = userGroupDto.GroupName;

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

                return Username;
            }
            return null;
        }
    }
}
