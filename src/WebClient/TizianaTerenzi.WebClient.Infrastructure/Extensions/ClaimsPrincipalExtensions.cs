namespace TizianaTerenzi.WebClient.Infrastructure.Extensions
{
    using System.Security.Claims;

    using TizianaTerenzi.Common;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUserAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(GlobalConstants.AdministratorRoleName);
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}
