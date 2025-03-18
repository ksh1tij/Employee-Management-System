using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeePortal.API.Data.Models
{
    public class UserGroupMember
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; } // Composite Key, Foreign Key to User table

        [Key, Column(Order = 1)]
        public int GroupId { get; set; } // Composite Key, Foreign Key to UserGroup table

        // Navigation Properties
        [ForeignKey("UserId")]
        [JsonIgnore]
        [Required]
        public User? User { get; set; } // Reference to the user
    }
}