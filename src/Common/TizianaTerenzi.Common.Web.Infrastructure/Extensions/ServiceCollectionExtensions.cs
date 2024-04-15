namespace TizianaTerenzi.Common.Web.Infrastructure.Extensions
{
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;

    using Hangfire;
    using MassTransit;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Polly;
    using Refit;
    using TizianaTerenzi.Common.Services.EventualConsistencyMessages;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure.HostedServices;

    public static class ServiceCollectionExtensions
    {
        private static ServiceEndpoints serviceEndpoints;

        // Group all extensions
        public static IServiceCollection AddMicroservice<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            JwtBearerEvents events = null)
            where TDbContext : DbContext
        {
            services
                .AddDatabase<TDbContext>(configuration)
                .AddApplicationSettings(configuration)
                .AddJwtTokenAuthentication(configuration, events)
                .AddHealth(configuration)
                .AddCustomResponseCompression()
                .AddControllers();

            return services;
        }

        public static IServiceCollection AddGateway(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddJwtTokenAuthentication(configuration)
                .AddHealth(configuration, includeSqlServer: false, includeRabbitMq: false)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddCustomResponseCompression()
                .AddControllers();

            return services;
        }

        public static IServiceCollection AddDatabase<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            services
                .AddScoped<DbContext, TDbContext>()
                .AddDbContext<TDbContext>(options => options
                    .UseSqlServer(
                        configuration.GetDefaultConnectionString(),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        }));

            return services;
        }

        // If you need application settings in class library.
        public static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<ApplicationSettings>(
                    configuration.GetSection(nameof(ApplicationSettings)),
                    config => config.BindNonPublicProperties = true);

            return services;
        }

        // Decrypt jwt token e.g. for [Authorize] attribute
        public static IServiceCollection AddJwtTokenAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            JwtBearerEvents events = null)
        {
            var jwtTokenSecret = configuration
                                .GetSection(nameof(ApplicationSettings))
                                .GetValue<string>(nameof(ApplicationSettings.Secret));

            var key = Encoding.ASCII.GetBytes(jwtTokenSecret);

            services
                .AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };

                    if (events != null)
                    {
                        options.Events = events;
                    }
                });

            // To inject IHttpContextAccessor in CurrentUserService contructor
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddCustomResponseCompression(this IServiceCollection services)
        {
            services
                .AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                })
                .Configure<BrotliCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                })
                .Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.SmallestSize;
                });

            return services;
        }

        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services,
            IConfiguration configuration,
            bool usePolling = true,
            params Type[] consumers)
        {
            services
                .AddTransient<IPublisher, Publisher>();

            services
                .AddMassTransit(mt =>
                {
                    consumers.ForEach(consumer => mt.AddConsumer(consumer));

                    mt.UsingRabbitMq((context, rmq) =>
                    {
                        // Consumer.FullName (with namespace) prevents us from having two Consumers with the same name sharing same queue (RabbitMq level).
                        consumers.ForEach(consumer => rmq.ReceiveEndpoint(consumer.FullName, endpoint =>
                        {
                            endpoint.PrefetchCount = Environment.ProcessorCount / 2;
                            endpoint.UseMessageRetry(r => r.Interval(retryCount: 10, interval: 1000));

                            endpoint.ConfigureConsumer(context, consumer);
                        }));

                        rmq.ConfigureEndpoints(context);
                    });
                });

            if (usePolling)
            {
                services
                    .AddBackgroundJob(configuration);

                services
                    .AddHostedService<EventMessageLogHostedService>();
            }

            return services;
        }

        // Ivaylo Kenov:
        // Not working because of https://github.com/reactiveui/refit/issues/717
        public static IServiceCollection AddExternalService<TService>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TService : class
        {
            if (serviceEndpoints == null)
            {
                serviceEndpoints = configuration
                    .GetSection(nameof(ServiceEndpoints))
                    .Get<ServiceEndpoints>(config => config
                        .BindNonPublicProperties = true);
            }

            var serviceName = typeof(TService)
                            .Name.Substring(1)
                            .Replace("Service", string.Empty);

            services
                .AddRefitClient<TService>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri(serviceEndpoints[serviceName]);

                    var requestServices = serviceProvider
                        .GetService<IHttpContextAccessor>()
                        ?.HttpContext
                        .RequestServices;

                    var currentToken = requestServices
                        ?.GetService<ICurrentTokenService>()
                        ?.Get();

                    if (currentToken == null)
                    {
                        return;
                    }

                    var authorizationHeader = new AuthenticationHeaderValue(InfrastructureConstants.AuthorizationHeaderValuePrefix, currentToken);
                    client.DefaultRequestHeaders.Authorization = authorizationHeader;
                })
                .AddTransientHttpErrorPolicy(policy => policy
                    .OrResult(result => result.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(retryCount: 5, sleepDurationProvider: retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
                .AddTransientHttpErrorPolicy(policy => policy
                    .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30)));

            return services;
        }

        public static IServiceCollection AddBackgroundJob(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetDefaultConnectionString()));

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddHealth(
            this IServiceCollection services,
            IConfiguration configuration,
            bool includeSqlServer = true,
            bool includeRabbitMq = true)
        {
            // Health Check for the Web Server
            var healthChecks = services.AddHealthChecks();

            if (includeSqlServer)
            {
                healthChecks
                    .AddSqlServer(configuration.GetDefaultConnectionString());
            }

            if (includeRabbitMq)
            {
                healthChecks
                    .AddRabbitMQ(rabbitConnectionString: "amqp://guest:guest@localhost/");
            }

            return services;
        }
    }
}
