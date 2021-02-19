using System;
namespace GaryPortalAPI.Models.Chat
{
    public class ChatCommand
    {
        public string Command { get; set; }
        public string CommandFriendlyName { get; set; }
        public string CommandDescription { get; set; }
        public string CommandUsage { get; set; }
        public bool RequiresStaff { get; set; }
        public bool RequiresAdmin { get; set; }
        public Delegate action { get; set; }
    }

    public class ChatBotRequest
    {
        public string input { get; set; }
        public string version { get; set; }
    }
}
