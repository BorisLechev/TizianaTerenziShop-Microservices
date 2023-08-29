namespace TizianaTerenzi.Common.Services.Identity
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        string Username { get; }

        string Email { get; }

        bool IsUserAuthenticated { get; }

        bool IsAdministrator { get; }
    }
}
