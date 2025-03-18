namespace EmployeePortal.API.DTOs
{
    public class UserGroupDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public required string GroupName { get; set; }
    }
}
