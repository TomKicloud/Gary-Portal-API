using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GaryPortalAPI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderUUID, string groupToSendTo, string messageUUID)
        {
            //TODO: Send actual message object
            await Clients.Group(groupToSendTo).SendAsync("MessageReceived", groupToSendTo, senderUUID, messageUUID);
        }

        public async Task DeleteMessage(string groupToSendTo, string messageUUID)
        {
            await Clients.Group(groupToSendTo).SendAsync("RemoveMessage", groupToSendTo, messageUUID);
        }

        public async Task SubscribeToGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
