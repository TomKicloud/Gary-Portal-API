using System;
using System.Security.Claims;

namespace GaryPortalAPI.Utilities
{
    public static class AuthenticationUtilities
    {
        public static bool IsSameUserOrPrivileged(ClaimsPrincipal identity, string uuid)
        {
            return identity.Identity.Name.Equals(uuid) || IsStaff(identity) || IsAdmin(identity);
        }

        public static bool IsSameUserOrStaff(ClaimsPrincipal identity, string uuid)
        {
            return IsSameUser(identity, uuid) || IsStaff(identity);
        }

        public static bool IsSameUserOrAdmin(ClaimsPrincipal identity, string uuid)
        {
            return IsSameUser(identity, uuid) || IsAdmin(identity);
        }

        public static bool IsSameUser(ClaimsPrincipal identity, string uuid)
        {
            return identity.Identity.Name.Equals(uuid);
        }

        public static bool IsPrivilegedUser(ClaimsPrincipal identity)
        {
            return identity.HasClaim(ClaimTypes.Role, "staff") || identity.HasClaim(ClaimTypes.Role, "admin");
        }

        public static bool IsAdmin(ClaimsPrincipal identity)
        {
            return identity.HasClaim(ClaimTypes.Role, "admin");
        }

        public static bool IsStaff(ClaimsPrincipal identity)
        {
            return identity.HasClaim(ClaimTypes.Role, "staff");
        }
    }
}
