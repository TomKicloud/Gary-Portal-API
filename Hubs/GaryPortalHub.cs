using System;
using System.Threading.Tasks;
using GaryPortalAPI.Models.Chat;
using Microsoft.AspNetCore.SignalR;

namespace GaryPortalAPI.Hubs
{
    public class GaryPortalHub : Hub
    {
        public async Task BanStatusChanged(string uuid)
        {
            await Clients.All.SendAsync("BanStatusUpdated", uuid);
        }
    }
}
