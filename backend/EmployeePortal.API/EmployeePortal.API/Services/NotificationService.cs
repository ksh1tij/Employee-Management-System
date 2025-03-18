using EmployeePortal.API.Data;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EmployeePortal.API.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly EmployeeManagementContext _context;

        public NotificationService(IHubContext<NotificationHub> hubContext, EmployeeManagementContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task SendNotificationAsync(string message, List<string> recipientUserIds)
        {
            if (string.IsNullOrEmpty(message) || recipientUserIds == null || recipientUserIds.Count == 0)
            {
                throw new ArgumentException("Message and recipient user IDs must be provided.");
            }

            var notifications = recipientUserIds.Select(userId => new Notification
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                RecipientUserId = userId
            }).ToList();

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();

            foreach (var userId in recipientUserIds)
            {
                Console.WriteLine(message);
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
            }
        }
    }
}
