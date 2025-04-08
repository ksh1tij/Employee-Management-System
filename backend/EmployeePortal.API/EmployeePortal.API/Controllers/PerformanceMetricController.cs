using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Data;
using EmployeePortal.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceMetricController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;

        public PerformanceMetricController(EmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: api/PerformanceMetrics/User/{userId}
        [HttpGet("User/{userId}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<ActionResult<IEnumerable<PerformanceMetric>>> GetUserPerformanceMetrics(int userId)
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

            var performanceMetrics = await _context.PerformanceMetrics
                .Where(pm => pm.UserId == userId)
                .ToListAsync();

            if (performanceMetrics == null || performanceMetrics.Count == 0)
            {
                return NotFound();
            }

            return performanceMetrics;
        }

        // GET: api/PerformanceMetrics/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PerformanceMetric>> GetPerformanceMetric(int id)
        {
            var performanceMetric = await _context.PerformanceMetrics.FindAsync(id);

            if (performanceMetric == null)
            {
                return NotFound();
            }

            return performanceMetric;
        }

        // GET: api/PerformanceMetrics/Manager/{managerId}
        [HttpGet("Manager/{managerId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<PerformanceMetric>>> GetUsersUnderManager(int managerId)
        {
            var performanceMetrics = await _context.PerformanceMetrics
                .Where(pm => pm.ManagerId == managerId)
                .ToListAsync();

            if (performanceMetrics == null)
            {
                return NotFound();
            }

            return performanceMetrics;
        }

        // POST: api/PerformanceMetrics
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PerformanceMetric>> PostPerformanceMetric(PerformanceMetricDto performanceMetricDto)
        {
            var user = await _context.Users.FindAsync(performanceMetricDto.UserId);
            var manager = await _context.Users.FindAsync(performanceMetricDto.ManagerId);

            if (user == null || manager == null)
            {
                return BadRequest("User or Manager not found.");
            }

            var performanceMetric = new PerformanceMetric
            {
                UserId = performanceMetricDto.UserId,
                TaskCompletionRate = performanceMetricDto.TaskCompletionRate,
                QualityOfWork = performanceMetricDto.QualityOfWork,
                AttendanceRate = performanceMetricDto.AttendanceRate,
                CustomerSatisfaction = performanceMetricDto.CustomerSatisfaction,
                Efficiency = performanceMetricDto.Efficiency,
                Teamwork = performanceMetricDto.Teamwork,
                LastUpdated = performanceMetricDto.LastUpdated,
                ManagerId = performanceMetricDto.ManagerId,
                User = user,
                Manager = manager
            };

            _context.PerformanceMetrics.Add(performanceMetric);
            await _context.SaveChangesAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(performanceMetric, options);

            return CreatedAtAction("GetPerformanceMetric", new { id = performanceMetric.MetricId }, jsonString);
        }

        // DELETE: api/PerformanceMetrics/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeletePerformanceMetric(int id)
        {
            var performanceMetric = await _context.PerformanceMetrics.FindAsync(id);
            if (performanceMetric == null)
            {
                return NotFound();
            }

            _context.PerformanceMetrics.Remove(performanceMetric);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/PerformanceMetrics/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchPerformanceMetric(int id, [FromBody] PerformanceMetricDto performanceMetricDto)
        {
            if (performanceMetricDto == null)
            {
                return BadRequest();
            }

            var performanceMetric = await _context.PerformanceMetrics.FindAsync(id);
            if (performanceMetric == null)
            {
                return NotFound();
            }

            // Update the entity with the values from the DTO
            performanceMetric.UserId = performanceMetricDto.UserId;
            performanceMetric.TaskCompletionRate = performanceMetricDto.TaskCompletionRate;
            performanceMetric.QualityOfWork = performanceMetricDto.QualityOfWork;
            performanceMetric.AttendanceRate = performanceMetricDto.AttendanceRate;
            performanceMetric.CustomerSatisfaction = performanceMetricDto.CustomerSatisfaction;
            performanceMetric.Efficiency = performanceMetricDto.Efficiency;
            performanceMetric.Teamwork = performanceMetricDto.Teamwork;
            performanceMetric.LastUpdated = performanceMetricDto.LastUpdated;
            performanceMetric.ManagerId = performanceMetricDto.ManagerId;

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
                var username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                return username;
            }
            return null;
        }
    }
}