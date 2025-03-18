using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeePortal.API.Data.Models
{
    public class UserDepartment
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; } // Composite Key, Foreign Key to User table

        [Key, Column(Order = 1)]
        public int DepartmentId { get; set; } // Composite Key, Foreign Key to Department table

        // Navigation Property to Department
        [ForeignKey("DepartmentId")]
        [JsonIgnore]
        [Required]
        public Department Department { get; set; } // Reference to the department
    }
}