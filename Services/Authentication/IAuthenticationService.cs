using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GaryPortalAPI.Services.Authentication
{
    public interface IAuthenticationService : IDisposable
    {
        Task<User> Authenticate(AuthenticatingUser authUser, bool needsTokens = false, CancellationToken ct = default);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppDbContext _context;
        private readonly IHashingService _hashingService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(AppDbContext context, IHashingService hashingService, IUserService userService, ITokenService tokenService)
        {
            _context = context;
            _hashingService = hashingService;
            _userService = userService;
            _tokenService = tokenService;
        }


        public async Task<User> Authenticate(AuthenticatingUser authUser, bool needsTokens = false, CancellationToken ct = default)
        {
            User user = await _context.Users
                .Include(u => u.UserAuthentication)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == authUser.AuthenticatorString.ToLower() || u.UserAuthentication.UserEmail.ToLower() == authUser.AuthenticatorString.ToLower());
            if (user == null) return null;

            if (!_hashingService.VerifyHash(user.UserAuthentication.UserPassHash, user.UserAuthentication.UserPassSalt, authUser.Password)) {
                return null;
            }

            if (await _userService.GetFirstBanOfTypeIfAnyAsnc(user.UserUUID, 1, ct) is UserBan ub && ub != null)
                throw new AuthenticationException($"User has received a global ban, ban expires: {ub.BanExpires:HH:mm:ss, dd/MM/yy}");

            user = await _userService.GetByIdAsync(user.UserUUID, ct);
            user.UserAuthTokens = needsTokens ? await _tokenService.GenerateInitialTokensForUserAsync(user.UserUUID) : null;
            return user;
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
