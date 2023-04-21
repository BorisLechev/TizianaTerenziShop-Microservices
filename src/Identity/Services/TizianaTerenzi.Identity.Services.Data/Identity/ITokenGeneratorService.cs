namespace TizianaTerenzi.Identity.Services.Data.Identity
{
    using TizianaTerenzi.Identity.Data.Models;

    public interface ITokenGeneratorService
    {
        string GenerateJwtBearerToken(ApplicationUser user, IEnumerable<string> userRoles = null);
    }
}
