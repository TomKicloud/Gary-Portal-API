using System;
namespace GaryPortalAPI.Models.Feed
{
    public class AditLog
    {
        public int AditLogId { get; set; }
        public string AditLogUrl { get; set; }
        public string AditLogThumbnailUrl { get; set; }
        public string PosterUUID { get; set; }
        public int AditLogTeamId { get; set; }
        public bool IsVideo { get; set; }
        public DateTime DatePosted { get; set; }
        public int AditLogViews { get; set; }
        public string Caption { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Poster { get; set; }
        public virtual UserDTO PosterDTO { get; set; }
        public virtual Team AditLogTeam { get; set; }
    }

    public class AditLogUrlResult
    {
        public string AditLogUrl { get; set; }
        public string AditLogThumbnailUrl { get; set; }
    }
}
