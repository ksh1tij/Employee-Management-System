namespace EmployeePortal.API.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string? Designation { get; set; }
        public required string Role { get; set; }
        public required string UserName { get; set; }
        public string? OriginalPassword { get; set; } // Add this field to check the original password
        public string? NewPassword { get; set; } // Add this field to update the new password
    }
}
