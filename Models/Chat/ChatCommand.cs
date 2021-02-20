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

    public class GifResponse
    {
        public GifArray[] results { get; set; }
    }

    public class GifArray
    {
        public string id { get; set; }
        public string title { get; set; }
        public string h1_title { get; set; }

        public GifDetails[] media { get; set; }
    }

    public class GifDetails
    {
        public Gif gif { get; set; }
        public Gif mediumgif { get; set; }
    }
    public class Gif
    {
        public string url { get; set; }
    }
}
