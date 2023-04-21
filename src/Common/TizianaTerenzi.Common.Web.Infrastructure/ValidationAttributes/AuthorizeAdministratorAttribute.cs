namespace TizianaTerenzi.Common.Web.Infrastructure.ValidationAttributes
{
    using Microsoft.AspNetCore.Authorization;

    public class AuthorizeAdministratorAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorAttribute()
        {
            this.Roles = GlobalConstants.AdministratorRoleName;
        }
    }
}
