using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace GaryPortalAPI.Services
{
    public interface IStaffService : IDisposable
    {
        Task<ICollection<StaffRoomAnnouncement>> GetStaffRoomAnnouncementsAsync(CancellationToken ct = default);
        Task<StaffRoomAnnouncement> GetStaffRoomAnnouncementAsync(int id, CancellationToken ct = default);
        Task<StaffRoomAnnouncement> PostStaffRoomAnnouncementAsync(StaffRoomAnnouncement announcement, CancellationToken ct = default);
        Task MarkAnnouncementAsDeletedAsync(int id, CancellationToken ct = default);
    }

    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<StaffRoomAnnouncement> GetStaffRoomAnnouncementAsync(int id, CancellationToken ct = default)
        {
            StaffRoomAnnouncement announcement = await _context.StaffRoomAnnouncements
                .AsNoTracking()
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AnnouncementId == id && !a.IsDeleted, cancellationToken: ct);
            announcement.UserDTO = announcement.User.ConvertToDTO();
            announcement.User = null;
            return announcement;
        }

        public async Task<ICollection<StaffRoomAnnouncement>> GetStaffRoomAnnouncementsAsync(CancellationToken ct = default)
        {
            ICollection<StaffRoomAnnouncement> announcements = await _context.StaffRoomAnnouncements
               .AsNoTracking()
               .Include(a => a.User)
               .Where(a => !a.IsDeleted)
               .OrderByDescending(a => a.AnnouncementDate)
               .ToListAsync(ct);
            foreach (StaffRoomAnnouncement announcement in announcements)
            {
                announcement.UserDTO = announcement.User.ConvertToDTO();
                announcement.User = null;
            }
            return announcements;
        }

        public async Task MarkAnnouncementAsDeletedAsync(int id, CancellationToken ct = default)
        {
            StaffRoomAnnouncement announcement = await _context.StaffRoomAnnouncements
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AnnouncementId == id, cancellationToken: ct);
            announcement.IsDeleted = true;
            _context.Update(announcement);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<StaffRoomAnnouncement> PostStaffRoomAnnouncementAsync(StaffRoomAnnouncement announcement, CancellationToken ct = default)
        {
            await _context.StaffRoomAnnouncements.AddAsync(announcement, ct);
            await _context.SaveChangesAsync();
            return await GetStaffRoomAnnouncementAsync(announcement.AnnouncementId, ct);
        }
    }
}
