namespace TizianaTerenzi.Common.Web.Infrastructure.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using TizianaTerenzi.Common.Services.Identity;

    public class JwtHeaderAuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public JwtHeaderAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentTokenService currentTokenService)
        {
            var token = context.Request.Headers[InfrastructureConstants.AuthorizationHeaderName].ToString();

            if (!string.IsNullOrWhiteSpace(token))
            {
                currentTokenService.Set(token.Split().Last());
            }

            await this.next(context);
        }
    }

    public static class JwtHeaderAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtHeaderAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder
                    .UseMiddleware<JwtHeaderAuthenticationMiddleware>()
                    .UseAuthentication();
        }
    }
}
