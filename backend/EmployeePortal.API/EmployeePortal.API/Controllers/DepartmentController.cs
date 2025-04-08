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
    public class DepartmentController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;

        public DepartmentController(EmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Departments/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // POST: api/Departments
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDto departmentDto)
        {
            var manager = await _context.Users.FindAsync(departmentDto.ManagerId);

            if (manager == null)
            {
                return BadRequest("Manager not found.");
            }

            var department = new Department
            {
                DepartmentName = departmentDto.DepartmentName,
                ManagerId = departmentDto.ManagerId,
                Manager = manager,
                UserDepartments = []
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Departments/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.DepartmentName = departmentDto.DepartmentName;
            department.ManagerId = departmentDto.ManagerId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
