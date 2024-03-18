namespace TizianaTerenzi.Notifications.Web.Infrastructure
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Common.Services.Identity;

    public class JwtConfiguration
    {
        public static JwtBearerEvents BearerEvents
            => new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/numberOfProductsInTheUsersCartHub") ||
                        path.StartsWithSegments("/userStatusHub") ||
                        path.StartsWithSegments("/chatHub") ||
                        path.StartsWithSegments("/notificationHub")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                        context.HttpContext.RequestServices?.GetService<ICurrentTokenService>()?.Set(accessToken);
                    }

                    return Task.CompletedTask;
                },
            };
    }
}
