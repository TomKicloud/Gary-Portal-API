using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GaryPortalAPI.Services.Authentication
{
    public interface IHashingService : IDisposable
    {
        bool VerifyHash(string UserPassHash, string UserSalt, string VerifyingPass);
        Tuple<string, string> NewHashAndSalt(string password);
        string RandomHash();
    }

    public class HashingService : IHashingService
    {
        public void Dispose()
        {
        }

        public string RandomHash()
        {
            var bytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }


        public Tuple<string, string> NewHashAndSalt(string password)
        {
            string salt = RandomHash();
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations: 4096, HashAlgorithmName.SHA256);
            return new Tuple<string, string>(Convert.ToBase64String(pbkdf2.GetBytes(64)), salt);
        }

        public bool VerifyHash(string UserPassHash, string UserSalt, string VerifyingPass)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(VerifyingPass, Convert.FromBase64String(UserSalt), iterations: 4096, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(pbkdf2.GetBytes(64)).Equals(UserPassHash);
        }
    }
}
