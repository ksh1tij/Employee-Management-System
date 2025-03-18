using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeePortal.API.Data.Models
{
    public class PerformanceMetric
    {
        [Key]
        public int MetricId { get; set; } // Primary Key

        public int UserId { get; set; } // Foreign Key to User table

        public float TaskCompletionRate { get; set; } // Performance metric: task completion rate
        public float QualityOfWork { get; set; } // Performance metric: quality of work
        public float AttendanceRate { get; set; } // Performance metric: attendance rate
        public float CustomerSatisfaction { get; set; } // Performance metric: customer satisfaction
        public float Efficiency { get; set; } // Performance metric: efficiency
        public float Teamwork { get; set; } // Performance metric: teamwork

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
        public User Manager { get; set; } // Reference to the manager who can edit this performance metric
    }
}