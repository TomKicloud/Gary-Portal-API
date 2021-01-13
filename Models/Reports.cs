using System;
using GaryPortalAPI.Models.Feed;

namespace GaryPortalAPI.Models
{
    public class UserReport
    {
        public int UserReportId { get; set; }
        public string UserUUID { get; set; }
        public string ReportReason { get; set; }
        public DateTime ReportIssuedAt { get; set; }
        public string ReportByUUID { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User ReportedUser { get; set; }
        public virtual User Reporter { get; set; }
    }

    public class FeedReport
    {
        public int FeedReportId { get; set; }
        public int FeedPostId { get; set; }
        public string ReportReason { get; set; }
        public DateTime ReportIssuedAt { get; set; }
        public string ReportByUUID { get; set; }
        public bool IsDeleted { get; set; }

        public virtual FeedPost ReportedPost { get; set; }
        public virtual User Reporter { get; set; }
    }
}
