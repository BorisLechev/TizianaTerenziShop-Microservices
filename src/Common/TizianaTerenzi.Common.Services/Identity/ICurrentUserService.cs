namespace TizianaTerenzi.Common.Services.Identity
{
    using System.Security.Claims;

    public interface ICurrentUserService
    {
        string UserId { get; }

        string Username { get; }

        string Email { get; }

        bool IsUserAuthenticated { get; }

        bool IsAdministrator { get; }

        ClaimsPrincipal GetUser { get; }
    }
}
