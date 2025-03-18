using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeePortal.API.Data.Models
{
    public class UserGroup
    {
        [Key]
        public int GroupId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public required string GroupName { get; set; } // Name of the user group

        // Navigation Property to UserGroupMembers
        [JsonIgnore]
        [Required]
        public ICollection<UserGroupMember> UserGroupMembers { get; set; } // Many-to-many relationship with users
    }
}