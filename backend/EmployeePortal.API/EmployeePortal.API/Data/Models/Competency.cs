using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeePortal.API.Data.Models
{
    public class Competency
    {
        [Key]
        public int CompetencyId { get; set; } // Primary Key

        public int UserId { get; set; } // Foreign Key to User table

        public float TechnicalSkills { get; set; } // Competency: technical skills
        public float Communication { get; set; } // Competency: communication
        public float ProblemSolving { get; set; } // Competency: problem-solving
        public float Teamwork { get; set; } // Competency: teamwork
        public float Leadership { get; set; } // Competency: leadership

        public DateTime LastUpdated { get; set; } // Date and time of the last update

        public int ManagerId { get; set; } // Foreign Key to User table (Manager)

        // Navigation Properties
        [ForeignKey("UserId")]
        [JsonIgnore]
        [Required]
        public User User { get; set; } // Reference to the user

        [ForeignKey("ManagerId")]
        [JsonIgnore]
        [Required]
        public User Manager { get; set; } // Reference to the manager who can edit this competency
    }
}