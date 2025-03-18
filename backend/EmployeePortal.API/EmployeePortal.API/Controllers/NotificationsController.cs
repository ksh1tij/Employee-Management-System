using System.Security.Claims;
using EmployeePortal.API.DTOs;
using EmployeePortal.API.Hubs;
using EmployeePortal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
            return Ok(new {Message = "Notification sent successfully"});
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin, Manager")]
        //public async Task<IActionResult> Post([FromBody] NotificationRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Ensure message is a string
        //    if (request.Message == null)
        //    {
        //        return BadRequest("Message cannot be null.");
        //    }

        //    string message = request.Message.ToString();
        //    Console.WriteLine(message);
        //    await _notificationService.SendNotificationAsync(message, request.RecipientUserIds);
        //    return Ok();

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}

            //await _notificationService.SendNotificationAsync(request.Message, request.RecipientUserIds);
            //return Ok();
    }
}
