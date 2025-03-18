using Microsoft.AspNetCore.SignalR;

namespace EmployeePortal.API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
        //public override Task OnConnectedAsync()
        //{
        //    var userId = Context.UserIdentifier;
        //    if (userId != null)
        //    {
        //        Groups.AddToGroupAsync(Context.ConnectionId, userId);
        //    }
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    if (exception != null)
        //    {
        //        // Log the exception or perform any necessary actions
        //        Console.WriteLine($"An error occurred: {exception.Message}");
        //    }

        //    var userId = Context.UserIdentifier;
        //    if (userId != null)
        //    {
        //        Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        //    }
        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}