namespace TizianaTerenzi.Watchdog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services);

            var app = builder.Build();

            app
                .UseRouting()
                .UseEndpoints(endpoints => endpoints
                    .MapHealthChecksUI());

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();
        }
    }
}
