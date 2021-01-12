using System;
using System.Collections.Generic;
using GaryPortalAPI.Models;
using GaryPortalAPI.Models.Feed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GaryPortalAPI.Models
{

    public class UserDTO
    {
        public string UserUUID { get; set; }
        public string UserFullName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public bool UserIsStaff { get; set; }
        public bool UserIsAdmin { get; set; }
    }

    public class UserDetails
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    public class User
    {
        public string UserUUID { get; set; }
        public string UserFullName { get; set; }
        public string UserSpanishName { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public string UserQuote { get; set; }
        public string UserBio { get; set; }
        public string UserGender { get; set; }
        public bool UserIsStaff { get; set; }
        public bool UserIsAdmin { get; set; }
        public string UserStanding { get; set; }
        public bool IsQueued { get; set; }
        public DateTime UserCreatedAt { get; set; }
        public DateTime UserDateOfBirth { get; set; }
        public bool IsDeleted { get; set; }

        public UserAuthenticationTokens UserAuthTokens { get; set; }

        public virtual UserAuthentication UserAuthentication { get; set; }
        public virtual UserTeam UserTeam { get; set; }
        public virtual UserRanks UserRanks { get; set; }
        public virtual UserPoints UserPoints { get; set; }
        //public virtual UserBans UserBans { get; set; }

        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual ICollection<FeedPost> FeedPosts { get; set; }
        public virtual ICollection<FeedAnswerVote> FeedVotes { get; set; }
        public virtual ICollection<FeedLike> FeedLikes { get; set; }
        public virtual ICollection<FeedComment> FeedComments { get; set; }
        public virtual ICollection<AditLog> AditLogs { get; set; }

        public virtual UserDTO ConvertToDTO()
        {
            return new UserDTO
            {
                UserUUID = UserUUID,
                UserFullName = UserFullName,
                UserIsAdmin = UserIsAdmin,
                UserIsStaff = UserIsStaff,
                UserProfileImageUrl = UserProfileImageUrl,
            };
        }

    }

    public class UserTeam
    {
        public string UserUUID { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }

    }

    public class UserRanks
    {
        public string UserUUID { get; set; }
        public int AmigoRankId { get; set; }
        public int PositivtyRankId { get; set; }

        public virtual Rank AmigoRank { get; set; }
        public virtual Rank PositivityRank { get; set; }
        public virtual User User { get; set; }
    }

    public class UserPoints
    {
        public string UserUUID { get; set; }
        public int AmigoPoints { get; set; }
        public int PositivityPoints { get; set; }
        public int BowelsRelieved { get; set; }
        public int Prayers { get; set; }
        public int MeaningfulPrayers { get; set; }

        public virtual User User { get; set; }
    }

    public class UserBans
    {
        public string UserUUID { get; set; }
        public bool IsBanned { get; set; }
        public bool IsChatBanned { get; set; }
        public bool IsFeedBanned { get; set; }
        public string BanReason { get; set; }

        public virtual User User { get; set; }
    }
}