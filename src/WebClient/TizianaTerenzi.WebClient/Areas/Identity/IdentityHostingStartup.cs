using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TizianaTerenzi.WebClient.Areas.Identity.IdentityHostingStartup))]

namespace TizianaTerenzi.WebClient.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}