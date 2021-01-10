using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GaryPortalAPI.Services
{
    public interface IUserService : IDisposable
    {
        Task<ICollection<User>> GetAllAsync(CancellationToken ct = default);
        Task<User> GetByIdAsync(string userUUID, CancellationToken ct = default);
        Task<bool> IsUsernameFreeAsync(string username, CancellationToken ct = default);
        Task<bool> IsEmailFreeAsync(string email, CancellationToken ct = default);
        Task<UserPoints> GetPointsForUserAsync(string userUUID, CancellationToken ct = default);
        Task<UserPoints> UpdatePointsForUserAsync(string userUUID, UserPoints points, CancellationToken ct = default);
        Task ClearAllPrayersAsync(CancellationToken ct = default);
        Task ClearPrayersForUserAsync(string uuid, CancellationToken ct = default);
        Task<User> UpdateUserDetailsAsync(string uuid, UserDetails details, CancellationToken ct = default);
        Task<string> UpdateUserProfilePictureAsync(string uuid, IFormFile file, CancellationToken ct = default);
        Task<User> CreateNewUserAsync(UserRegistration creatingUser, CancellationToken ct = default);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHashingService _hashingService;

        public UserService(AppDbContext context, IHashingService hashingService)
        {
            _context = context;
            _hashingService = hashingService;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<ICollection<User>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Users
                                    .Include(u => u.UserAuthentication)
                                    .Include(u => u.UserPoints)
                                    .Include(u => u.UserRanks)
                                    .Include(u => u.UserTeam)
                                    .ToListAsync(ct);
        }

        public async Task<User> GetByIdAsync(string userUUID, CancellationToken ct = default)
        {
            return await _context.Users
                                    .Include(u => u.UserAuthentication)
                                    .Include(u => u.UserPoints)
                                    .Include(u => u.UserRanks)
                                    .Include(u => u.UserTeam)
                                    .FirstOrDefaultAsync(u => u.UserUUID == userUUID, ct);
        }

        public async Task<bool> IsEmailFreeAsync(string email, CancellationToken ct = default)
        {
            return await _context.UserAuthentications.FirstOrDefaultAsync(u => u.UserEmail == email, ct) == null;
        }

        public async Task<bool> IsUsernameFreeAsync(string username, CancellationToken ct = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username, ct) == null;
        }

        public async Task<UserPoints> GetPointsForUserAsync(string userUUID, CancellationToken ct = default)
        {
            return await _context.UserPoints.FindAsync(userUUID);
        }



        public async Task<UserPoints> UpdatePointsForUserAsync(string uuid, UserPoints points, CancellationToken ct = default)
        {
            UserPoints userpoints = await _context.UserPoints
                .AsNoTracking().FirstOrDefaultAsync(u => u.UserUUID == uuid);
            if (userpoints != null)
            {
                userpoints = points;
                _context.Update(userpoints);
                await _context.SaveChangesAsync();
            }

            return userpoints ?? null;
        }

        public async Task ClearAllPrayersAsync(CancellationToken ct = default)
        {
            await _context.UserPoints.ForEachAsync(up =>
            {
                up.Prayers = 0;
                up.MeaningfulPrayers = 0;
            });
            await _context.SaveChangesAsync();
        }

        public async Task ClearPrayersForUserAsync(string uuid, CancellationToken ct = default)
        {
            UserPoints userpoints = await _context.UserPoints.FindAsync(uuid);
            userpoints.Prayers = 0;
            userpoints.MeaningfulPrayers = 0;
            await _context.SaveChangesAsync();
        }

        public async Task<User> UpdateUserDetailsAsync(string uuid, UserDetails details, CancellationToken ct = default)
        {
            User user = await GetByIdAsync(uuid, ct);
            if (user == null) return null;

            user.UserAuthentication.UserEmail = details.UserEmail;
            user.UserName = details.UserName;
            user.UserFullName = details.FullName;
            user.UserProfileImageUrl = details.ProfilePictureUrl;
            await _context.SaveChangesAsync(ct);
            return user;
        }


        public async Task<string> UpdateUserProfilePictureAsync(string uuid, IFormFile file, CancellationToken ct = default)
        {
            if (file == null) return null;

            User user = await _context.Users.FindAsync(uuid);
            Directory.CreateDirectory($"/var/www/cdn/GaryPortal/ProfilePics/{uuid}");
            var fileName = $"{Guid.NewGuid():N}.jpg";
            var filePath = $"/var/www/cdn/GaryPortal/ProfilePics/{uuid}/{fileName}";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string newProfileUrl = $"https://cdn.tomk.online/GaryPortal/ProfilePics/{uuid}/{fileName}";
            user.UserProfileImageUrl = newProfileUrl;
            await _context.SaveChangesAsync();
            return newProfileUrl;
        }

        public async Task<User> CreateNewUserAsync(UserRegistration creatingUser, CancellationToken ct = default)
        {
            if (await IsEmailFreeAsync(creatingUser.UserEmail, ct) && await IsUsernameFreeAsync(creatingUser.UserName, ct))
            {
                string newUUID = Guid.NewGuid().ToString("N");
                Tuple<string, string> hashedPassValues = _hashingService.NewHashAndSalt(creatingUser.UserPassword);
                User newUser = new User
                {
                    UserUUID = newUUID,
                    UserName = creatingUser.UserName,
                    UserFullName = creatingUser.UserFullName,
                    UserBio = "",
                    UserQuote = "",
                    UserSpanishName = "",
                    UserStanding = "",
                    UserProfileImageUrl = "",
                    UserIsAdmin = false,
                    UserIsStaff = false,
                    IsQueued = true,
                    UserAuthentication = new UserAuthentication
                    {
                        UserEmail = creatingUser.UserEmail,
                        UserEmailConfirmed = false,
                        UserPassHash = hashedPassValues.Item1,
                        UserPassSalt = hashedPassValues.Item2
                    },
                    UserPoints = new UserPoints
                    {
                        AmigoPoints = 0,
                        BowelsRelieved = 0,
                        MeaningfulPrayers = 0,
                        PositivityPoints = 0,
                        Prayers = 0
                    }
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return await GetByIdAsync(newUser.UserUUID);
            }

            return null;
        }
    }
}
