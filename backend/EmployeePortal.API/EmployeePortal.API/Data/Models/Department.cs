using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortal.API.Data.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public required string DepartmentName { get; set; } // Name of the department

        public required int ManagerId { get; set; } // Foreign Key to User table (Manager)

        // Navigation Property to Manager
        [ForeignKey("ManagerId")]
        public required User Manager { get; set; } // Reference to the manager of the department

        // Navigation Property to UserDepartments
        public required ICollection<UserDepartment> UserDepartments { get; set; } // Many-to-many relationship with users
    }
}