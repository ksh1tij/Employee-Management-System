using EmployeePortal.API.Data;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetencyController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;
        private readonly ILogger<CompetencyController> _logger;

        public CompetencyController(EmployeeManagementContext context, ILogger<CompetencyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Competencies/User/{userId}
        [HttpGet("User/{userId}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<ActionResult<IEnumerable<CompetencyDto>>> GetUserCompetencies(int userId)
        {
            _logger.LogInformation("Getting competencies for user with ID {UserId}", userId);

            var competencies = await _context.Competencies
                .Where(c => c.UserId == userId)
                .Select(c => new CompetencyDto
                {
                    UserId = c.UserId,
                    TechnicalSkills = c.TechnicalSkills,
                    Communication = c.Communication,
                    ProblemSolving = c.ProblemSolving,
                    Teamwork = c.Teamwork,
                    Leadership = c.Leadership,
                    LastUpdated = c.LastUpdated,
                    ManagerId = c.ManagerId
                })
                .ToListAsync();

            if (competencies == null || competencies.Count == 0)
            {
                _logger.LogWarning("No competencies found for user with ID {UserId}", userId);
                return NotFound();
            }

            return Ok(competencies);
        }

        // GET: api/Competencies/Manager/{managerId}
        [HttpGet("Manager/{managerId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<Competency>>> GetUsersUnderManager(int managerId)
        {
            _logger.LogInformation("Getting competencies for users under manager with ID {ManagerId}", managerId);

            var competencies = await _context.Competencies
                .Where(c => c.ManagerId == managerId)
                .ToListAsync();

            if (competencies == null)
            {
                _logger.LogWarning("No competencies found for manager with ID {ManagerId}", managerId);
                return NotFound();
            }

            return Ok(competencies);
        }

        // GET: api/Competencies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Competency>> GetCompetency(int id)
        {
            _logger.LogInformation("Getting competency with ID {CompetencyId}", id);

            var competency = await _context.Competencies.FindAsync(id);

            if (competency == null)
            {
                _logger.LogWarning("Competency with ID {CompetencyId} not found", id);
                return NotFound();
            }

            return Ok(competency);
        }

        // POST: api/Competencies
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<Competency>> PostCompetency(CompetencyDto competencyDto)
        {
            _logger.LogInformation("Creating new competency for user with ID {UserId}", competencyDto.UserId);

            var user = await _context.Users.FindAsync(competencyDto.UserId);
            var manager = await _context.Users.FindAsync(competencyDto.ManagerId);

            if (user == null || manager == null)
            {
                _logger.LogWarning("User or Manager not found for competency creation");
                return BadRequest("User or Manager not found.");
            }

            var competency = new Competency
            {
                UserId = competencyDto.UserId,
                TechnicalSkills = competencyDto.TechnicalSkills,
                Communication = competencyDto.Communication,
                ProblemSolving = competencyDto.ProblemSolving,
                Teamwork = competencyDto.Teamwork,
                Leadership = competencyDto.Leadership,
                LastUpdated = competencyDto.LastUpdated,
                ManagerId = competencyDto.ManagerId,
                User = user,
                Manager = manager
            };

            _context.Competencies.Add(competency);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Competency created with ID {CompetencyId}", competency.CompetencyId);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(competency, options);

            return CreatedAtAction("GetCompetency", new { id = competency.CompetencyId }, jsonString);
        }

        // DELETE: api/Competencies/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCompetency(int id)
        {
            _logger.LogInformation("Deleting competency with ID {CompetencyId}", id);

            var competency = await _context.Competencies.FindAsync(id);
            if (competency == null)
            {
                _logger.LogWarning("Competency with ID {CompetencyId} not found for deletion", id);
                return NotFound();
            }

            _context.Competencies.Remove(competency);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Competency with ID {CompetencyId} deleted", id);
            return NoContent();
        }

        // PATCH: api/Competencies/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchCompetency(int id, [FromBody] CompetencyDto competencyDto)
        {
            _logger.LogInformation("Patching competency with ID {CompetencyId}", id);

            if (competencyDto == null)
            {
                _logger.LogWarning("Competency DTO is null for competency with ID {CompetencyId}", id);
                return BadRequest();
            }

            var competency = await _context.Competencies.FindAsync(id);
            if (competency == null)
            {
                _logger.LogWarning("Competency with ID {CompetencyId} not found for patching", id);
                return NotFound();
            }

            competency.UserId = competencyDto.UserId;
            competency.TechnicalSkills = competencyDto.TechnicalSkills;
            competency.Communication = competencyDto.Communication;
            competency.ProblemSolving = competencyDto.ProblemSolving;
            competency.Teamwork = competencyDto.Teamwork;
            competency.Leadership = competencyDto.Leadership;
            competency.LastUpdated = competencyDto.LastUpdated;
            competency.ManagerId = competencyDto.ManagerId;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Competency with ID {CompetencyId} patched successfully", id);
            return NoContent();
        }
    }
}