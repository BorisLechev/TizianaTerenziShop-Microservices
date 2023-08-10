namespace TizianaTerenzi.WebClient.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services.Identity;

    public class JwtCookieAuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public JwtCookieAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentTokenService currentTokenService)
        {
            var token = context.Request.Cookies[InfrastructureConstants.AuthenticationCookieName];

            if (!string.IsNullOrWhiteSpace(token))
            {
                currentTokenService.Set(token);

                // Authorization: Bearer .....
                context.Request.Headers.Append(InfrastructureConstants.AuthorizationHeaderName, $"{InfrastructureConstants.AuthorizationHeaderValuePrefix} {token}");
            }

            await this.next(context);
        }
    }

    public static class JwtCookieAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtCookieAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder
                    .UseMiddleware<JwtCookieAuthenticationMiddleware>()
                    .UseAuthentication();
        }
    }
}
