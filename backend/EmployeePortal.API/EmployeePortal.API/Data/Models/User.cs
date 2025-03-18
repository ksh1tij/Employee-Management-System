using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeePortal.API.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public required string Name { get; set; } // User's full name

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; } // User's email address

        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; } // User's phone number

        public string? Address { get; set; } // User's address

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } // User's date of birth

        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; } // Date the user joined the organization

        [StringLength(50)]
        public string? Designation { get; set; } // User's job title

        [Required]
        [StringLength(20)]
        public required string Role { get; set; } // User's role (Admin, Manager, Employee)

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BaseSalary { get; set; }

        [Required]
        [StringLength(100)]
        public required string UserName { get; set; } // User's username for login

        [Required]
        public required string PasswordHash { get; set; } // Hashed password for secure authentication

        // Navigation Properties
        public required ICollection<PerformanceMetric> PerformanceMetrics { get; set; } // One-to-many relationship with performance metrics
        public required ICollection<Competency> Competencies { get; set; } // One-to-many relationship with competencies
        public required ICollection<UserGroupMember> UserGroupMembers { get; set; } // Many-to-many relationship with user groups
    }
}