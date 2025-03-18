namespace EmployeePortal.API.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RecipientUserId { get; set; }
    }
}
