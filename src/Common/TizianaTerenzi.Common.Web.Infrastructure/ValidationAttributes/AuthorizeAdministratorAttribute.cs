namespace TizianaTerenzi.Common.Web.Infrastructure.ValidationAttributes
{
    using Microsoft.AspNetCore.Authorization;

    // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AuthorizeAdministratorAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorAttribute()
        {
            this.Roles = GlobalConstants.AdministratorRoleName;
        }
    }
}
