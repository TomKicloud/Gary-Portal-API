using System;
namespace GaryPortalAPI.Models
{
    public class UserAuthentication
    {
        public string UserUUID { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public bool UserEmailConfirmed { get; set; }
        public bool UserPhoneConfirmed { get; set; }
        public string UserPassHash { get; set; }
        public string UserPassSalt { get; set; }

        public virtual User User { get; set; }
    }

    public class UserAuthenticationTokens
    {
        public string AuthenticationToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserRefreshToken
    {
        public string UserUUID { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? TokenIssueDate { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
        public string TokenClient { get; set; }
        public bool TokenIsEnabled { get; set;  }

        public virtual User User { get; set; }
    }

    public class AuthenticatingUser
    {
        public string AuthenticatorString { get; set; }
        public string Password { get; set; }
    }
}
