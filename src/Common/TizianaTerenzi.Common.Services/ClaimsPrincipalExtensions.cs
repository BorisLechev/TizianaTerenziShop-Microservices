namespace TizianaTerenzi.Common.Services
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            bool isUserAdministrator = user.IsInRole(GlobalConstants.AdministratorRoleName);

            return isUserAdministrator;
        }
    }
}
