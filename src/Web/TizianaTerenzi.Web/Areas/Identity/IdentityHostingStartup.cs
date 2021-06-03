using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TizianaTerenzi.Web.Areas.Identity.IdentityHostingStartup))]
namespace TizianaTerenzi.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}