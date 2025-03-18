using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.API.DTOs
{
    public class NotificationRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public required string Message { get; set; }

        [Required]
        public required List<string> RecipientUserIds { get; set; }
    }
}
