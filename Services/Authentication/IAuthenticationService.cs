using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GaryPortalAPI.Services.Authentication
{
    public interface IAuthenticationService : IDisposable
    {
        Task<User> Authenticate(AuthenticatingUser authUser, bool needsTokens = false, CancellationToken ct = default);
        Task<string> AddEmailConfirmation(User user, CancellationToken ct = default);
        Task<bool> ConfirmUser(string hash, CancellationToken ct = default);
        Task<string> AddResetHash(User user, CancellationToken ct = default);
        Task<bool> ResetPassword(string hash, string newPassword, CancellationToken ct = default);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppDbContext _context;
        private readonly IHashingService _hashingService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthenticationService(AppDbContext context, IHashingService hashingService, IUserService userService, ITokenService tokenService, IEmailService emailService)
        {
            _context = context;
            _hashingService = hashingService;
            _userService = userService;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<User> Authenticate(AuthenticatingUser authUser, bool needsTokens = false, CancellationToken ct = default)
        {
            User user = await _context.Users
                .Include(u => u.UserAuthentication)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == authUser.AuthenticatorString.ToLower() || u.UserAuthentication.UserEmail.ToLower() == authUser.AuthenticatorString.ToLower());
            if (user == null) return null;

            if (!_hashingService.VerifyHash(user.UserAuthentication.UserPassHash, user.UserAuthentication.UserPassSalt, authUser.Password))
            {
                return null;
            }

            if (await _userService.GetFirstBanOfTypeIfAnyAsnc(user.UserUUID, 1, ct) is UserBan ub && ub != null)
                throw new AuthenticationException($"User has received a global ban, ban expires: {ub.BanExpires:HH:mm:ss, dd/MM/yy}");

            user = await _userService.GetByIdAsync(user.UserUUID, ct);
            user.UserAuthTokens = needsTokens ? await _tokenService.GenerateInitialTokensForUserAsync(user.UserUUID) : null;
            return user;
        }

        public async Task<string> AddEmailConfirmation(User user, CancellationToken ct = default)
        {
            UserAuthenticationConfirmation conf = new UserAuthenticationConfirmation
            {
                UserUUID = user.UserUUID,
                UserConfirmationHash = _hashingService.RandomHash(),
                ConfirmationExpiry = DateTime.UtcNow.AddHours(1),
                ConfirmationIsActive = true
            };
            await _context.UserAuthConfirmations.AddAsync(conf, ct);
            await _context.SaveChangesAsync();
            return conf.UserConfirmationHash;
        }

        public async Task<string> AddResetHash(User user, CancellationToken ct = default)
        {
            string resetHash = Base64UrlEncoder.Encode(_hashingService.RandomHash());
            UserPassResetToken resetToken = new UserPassResetToken
            {
                UserUUID = user.UserUUID,
                UserResetHash = resetHash,
                HashExpiry = DateTime.UtcNow.AddMinutes(30),
                HashIsActive = true
            };
            await _context.UserPassResetTokens.AddAsync(resetToken, ct);
            await _context.SaveChangesAsync();
            return resetToken.UserResetHash;
        }

        public Task<bool> ConfirmUser(string hash, CancellationToken ct = default)
        {
            //TODO: Implement
            return Task.FromResult(false);
        }

        public async Task<bool> ResetPassword(string hash, string newPassword, CancellationToken ct = default)
        {
            UserPassResetToken token = await _context.UserPassResetTokens
                .FirstOrDefaultAsync(urt => urt.UserResetHash == hash && urt.HashExpiry > DateTime.UtcNow);
            if (token == null)
            {
                return false;
            }

            Tuple<string, string> hashedPassValues = _hashingService.NewHashAndSalt(newPassword);
            UserAuthentication auth = await _context.UserAuthentications.FirstOrDefaultAsync(ua => ua.UserUUID == token.UserUUID);
            if (auth != null)
            {
                auth.UserPassHash = hashedPassValues.Item1;
                auth.UserPassSalt = hashedPassValues.Item2;
            }
            await _tokenService.InvalidateAllRefreshTokensAsync(token.UserUUID);
            token.HashIsActive = false;
            _context.Update(token);
            await _context.SaveChangesAsync();
            return true;
            throw new NotImplementedException();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
