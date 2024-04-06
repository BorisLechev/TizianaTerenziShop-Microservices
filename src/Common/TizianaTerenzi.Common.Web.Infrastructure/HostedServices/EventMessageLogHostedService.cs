namespace TizianaTerenzi.Common.Web.Infrastructure.HostedServices
{
    using Hangfire;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TizianaTerenzi.Common.Data.Models;

    public class EventMessageLogHostedService : IHostedService
    {
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IBus publisher;

        public EventMessageLogHostedService(
            IRecurringJobManager recurringJobManager,
            IServiceScopeFactory scopeFactory,
            IBus publisher)
        {
            this.recurringJobManager = recurringJobManager;
            this.scopeFactory = scopeFactory;
            this.publisher = publisher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = this.scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            var dbContextIsUp = dbContext.Database.CanConnect();

            if (!dbContextIsUp)
            {
                dbContext.Database.Migrate();
            }

            this.recurringJobManager
                .AddOrUpdate(
                    nameof(EventMessageLogHostedService),
                    () => this.ProcessPendingEventMessages(),
                    "*/5 * * * * *"); // every 5 seconds

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void ProcessPendingEventMessages()
        {
            using var scope = this.scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            var messages = dbContext
                    .Set<EventMessageLog>()
                    .Where(m => !m.Published)
                    .OrderBy(m => m.CreatedOn)
                    .ToList();

            foreach (var message in messages)
            {
                this.publisher.Publish(message.Data, message.Type).GetAwaiter().GetResult();

                message.MarkAsPublished();

                dbContext.SaveChanges();
            }
        }
    }
}
