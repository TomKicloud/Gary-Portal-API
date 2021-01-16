using System;
namespace GaryPortalAPI.Models
{
    public class StaffRoomAnnouncement
    {
        public int AnnouncementId { get; set; }
        public string Announcement { get; set; }
        public string UserUUID { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual UserDTO UserDTO { get; set; }
    }
}
