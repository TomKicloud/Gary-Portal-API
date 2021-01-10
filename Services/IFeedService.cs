using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Models.Feed;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GaryPortalAPI.Services
{
    public interface IFeedService : IDisposable
    {
        Task<ICollection<FeedPost>> GetAllAsync(int startfrom, int limit = 10, CancellationToken ct = default);
        Task<FeedPost> GetByIdAsync(int feedPostId, CancellationToken ct = default);
        Task ToggleLikeForPostAsync(int feedPostId, string userUUID, CancellationToken ct = default);
        Task<bool> HasUserLikedPostAsync(string userUUID, int feedPostId, CancellationToken ct = default);
        Task<string> UploadMediaAttachment(IFormFile file, CancellationToken ct = default);
    }

    public class FeedService : IFeedService
    {
        private readonly AppDbContext _context;

        public FeedService(AppDbContext context)
        {
            _context = context;
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }

        public async Task<ICollection<FeedPost>> GetAllAsync(int startfrom, int limit = 10, CancellationToken ct = default)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(startfrom);
            ICollection<FeedPost> posts = await _context.FeedPosts
                .Include(fp => fp.Likes)
                .Include(fp => fp.Poster)
                .Include(fp => fp.PostTeam)
                .Where(fp => fp.PostCreatedAt >= new DateTime(timeSpan.Ticks) && !fp.IsDeleted)
                .Take(10)
                .ToListAsync(ct);
            foreach (FeedPost post in posts)
            {
                post.PosterDTO = post.Poster.ConvertToDTO();
                post.Poster = null;
            }
            return posts;
        }

        public async Task<FeedPost> GetByIdAsync(int feedPostId, CancellationToken ct = default)
        {
            FeedPost post = await _context.FeedPosts
                    .Include(fp => fp.Likes)
                    .Include(fp => fp.Poster)
                    .Include(fp => fp.PostTeam)
                    .FirstOrDefaultAsync(fp => fp.PostId == feedPostId);
            Console.WriteLine(feedPostId);
            post.PosterDTO = post.Poster.ConvertToDTO();
            post.Poster = null;
            return post;
        }

        public async Task<bool> HasUserLikedPostAsync(string userUUID, int feedPostId, CancellationToken ct = default)
        {
            FeedPost post = await _context.FeedPosts
                    .Include(fp => fp.Likes)
                    .FirstOrDefaultAsync(ct);
            if (post == null)
                return false;

            return post.Likes.FirstOrDefault(fl => fl.UserUUID == userUUID && fl.IsLiked) != null;
        }

        public async Task ToggleLikeForPostAsync(int feedPostId, string userUUID, CancellationToken ct = default)
        {
            FeedPost post = await _context.FeedPosts
                    .Include(fp => fp.Likes)
                    .FirstOrDefaultAsync(ct);

            if (post == null)
                return;

            FeedLike like = post.Likes.FirstOrDefault(fl => fl.UserUUID == userUUID);
            if (await HasUserLikedPostAsync(userUUID, feedPostId))
            {
                like.IsLiked = false;
                _context.Update(like);
            } else
            {
                if (like == null)
                {
                    FeedLike newLike = new FeedLike
                    {
                        PostId = feedPostId,
                        UserUUID = userUUID,
                        IsLiked = true
                    };
                    await _context.FeedPostLikes.AddAsync(newLike);
                } else
                {
                    like.IsLiked = !like.IsLiked;
                    _context.Update(like);
                }
            }
            await _context.SaveChangesAsync();
        }
 
        public async Task<string> UploadMediaAttachment(IFormFile file, CancellationToken ct = default)
        {
            if (file == null) return null;

            string uuid = Guid.NewGuid().ToString();
            Directory.CreateDirectory("/var/www/cdn/GaryPortal/Feed/Attachments/Media/");
            string newFileName = file.FileName.Replace(Path.GetFileNameWithoutExtension(file.FileName), uuid);
            var filePath = $"/var/www/cdn/GaryPortal/Feed/Attachments/Media/{newFileName}";
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream, ct);
            return $"https://cdn.tomk.online/GaryPortal/Feed/Attachments/Media/{newFileName}";
        }

        public async Task<FeedPost> UploadNewPost(FeedPost post)
        {
            return null;
        }
    }
}
