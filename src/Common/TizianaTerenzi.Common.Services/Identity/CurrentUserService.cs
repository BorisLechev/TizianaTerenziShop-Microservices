namespace TizianaTerenzi.Common.Services.Identity
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.user = httpContextAccessor.HttpContext?.User;

            if (this.user == null)
            {
                throw new InvalidOperationException("This request does not have an authenticated user.");
            }

            this.UserId = this.user.GetUserId();
        }

        public string UserId { get; }

        public string Username => this.user.GetUserName();

        public string Email => this.user.GetUserEmail();

        public bool IsUserAuthenticated => this.user.IsUserAuthenticated();

        public bool IsAdministrator => this.user.IsAdministrator();
    }
}