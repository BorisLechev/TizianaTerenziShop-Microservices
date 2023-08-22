namespace TizianaTerenzi.Common.Services
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUserAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            bool isUserAdministrator = user.IsInRole(GlobalConstants.AdministratorRoleName);

            return isUserAdministrator;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name);
        }
    }
}
