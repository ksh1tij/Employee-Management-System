using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Data;
using EmployeePortal.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupMemberController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;

        public UserGroupMemberController(EmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: api/UserGroupMembers/Group/{groupId}
        [HttpGet("Group/{groupId}")]
        public async Task<ActionResult<IEnumerable<UserGroupMember>>> GetUserGroupMembersByGroupId(int groupId)
        {
            var userGroupMembers = await _context.UserGroupMembers
                .Where(ugm => ugm.GroupId == groupId)
                .ToListAsync();

            if (userGroupMembers == null || userGroupMembers.Count == 0)
            {
                return NotFound();
            }

            return userGroupMembers;
        }

        // GET: api/UserGroupMembers/Manager/{managerId}
        [HttpGet("Manager/{managerId}")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<UserGroupMember>>> GetUserGroupMembersUnderManager(int managerId)
        {
            var departments = await _context.Departments
                .Where(d => d.ManagerId == managerId)
                .Include(d => d.UserDepartments)
                .ToListAsync();

            var userIds = departments
                .SelectMany(d => d.UserDepartments.Select(ud => ud.UserId))
                .Distinct()
                .ToList();

            var userGroupMembers = await _context.UserGroupMembers
                .Where(ugm => userIds.Contains(ugm.UserId))
                .ToListAsync();

            if (userGroupMembers == null || userGroupMembers.Count == 0)
            {
                return NotFound();
            }

            return userGroupMembers;
        }

        // POST: api/UserGroupMembers
        [HttpPost]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<UserGroupMember>> PostUserGroupMember(UserGroupMemberDto userGroupMemberDto)
        {
            var user = await _context.Users.FindAsync(userGroupMemberDto.UserId);
            var userGroup = await _context.UserGroups.FindAsync(userGroupMemberDto.GroupId);

            if (user == null || userGroup == null)
            {
                return BadRequest("User or User Group not found.");
            }

            var userGroupMember = new UserGroupMember
            {
                UserId = userGroupMemberDto.UserId,
                GroupId = userGroupMemberDto.GroupId,
                User = user
            };

            _context.UserGroupMembers.Add(userGroupMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserGroupMembersUnderManager", new { managerId = userGroupMemberDto.UserId }, userGroupMember);
        }

        // DELETE: api/UserGroupMembers/{userId}/{groupId}
        [HttpDelete("{userId}/{groupId}")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteUserGroupMember(int userId, int groupId)
        {
            var userGroupMember = await _context.UserGroupMembers
                .FirstOrDefaultAsync(ugm => ugm.UserId == userId && ugm.GroupId == groupId);
            if (userGroupMember == null)
            {
                return NotFound();
            }

            _context.UserGroupMembers.Remove(userGroupMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/UserGroupMembers/{userId}/{groupId}
        [HttpPatch("{userId}/{groupId}")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchUserGroupMember(int userId, int groupId, [FromBody] UserGroupMemberDto userGroupMemberDto)
        {
            if (userGroupMemberDto == null)
            {
                return BadRequest();
            }

            var userGroupMember = await _context.UserGroupMembers
                .FirstOrDefaultAsync(ugm => ugm.UserId == userId && ugm.GroupId == groupId);
            if (userGroupMember == null)
            {
                return NotFound();
            }

            // Remove the existing UserGroupMember entity
            _context.UserGroupMembers.Remove(userGroupMember);
            await _context.SaveChangesAsync();

            // Find the user to set the User property
            var user = await _context.Users.FindAsync(userGroupMemberDto.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Create a new UserGroupMember entity with the updated composite key values
            var newUserGroupMember = new UserGroupMember
            {
                UserId = userGroupMemberDto.UserId,
                GroupId = userGroupMemberDto.GroupId,
                User = user
            };

            _context.UserGroupMembers.Add(newUserGroupMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
