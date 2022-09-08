using Dashboard.Models;
using Microsoft.AspNetCore.SignalR;

namespace Dashboard;

public class EmergencyHub:Hub
{
    public void GroupMessage(GroupMessage msg)
    {
        Clients.Groups(msg.group).SendAsync("emergencyReceived", msg.message);
    }

    public async Task ServiceConnected(string department)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, department);
    }
}
