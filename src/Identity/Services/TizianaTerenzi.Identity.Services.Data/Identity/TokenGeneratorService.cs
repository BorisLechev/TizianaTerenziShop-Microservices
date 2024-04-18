namespace TizianaTerenzi.Identity.Services.Data.Identity
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Identity.Data.Models;

    [TransientRegistration]
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly ApplicationSettings applicationSettings;

        public TokenGeneratorService(IOptions<ApplicationSettings> applicationSettings)
        {
            this.applicationSettings = applicationSettings.Value;
        }

        public string GenerateJwtBearerToken(ApplicationUser user, IEnumerable<string> userRoles = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.applicationSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.LastName),
                new Claim(ClaimTypes.Surname, user.LastName),
            };

            if (userRoles != null)
            {
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}
