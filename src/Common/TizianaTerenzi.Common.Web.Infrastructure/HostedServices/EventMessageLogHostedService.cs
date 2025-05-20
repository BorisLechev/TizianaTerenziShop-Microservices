namespace TizianaTerenzi.Common.Web.Infrastructure.HostedServices
{
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TizianaTerenzi.Common.Data.EventualConsistencyMessages;
    using TizianaTerenzi.Common.Data.Models;

    public class EventMessageLogHostedService : IHostedService
    {
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IPublisher publisher;

        public EventMessageLogHostedService(
            IRecurringJobManager recurringJobManager,
            IServiceScopeFactory scopeFactory,
            IPublisher publisher)
        {
            this.recurringJobManager = recurringJobManager;
            this.scopeFactory = scopeFactory;
            this.publisher = publisher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = this.scopeFactory.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            var dbContextIsUp = await dbContext.Database.CanConnectAsync();

            if (!dbContextIsUp)
            {
                await dbContext.Database.MigrateAsync();
            }

            this.recurringJobManager
                .AddOrUpdate(
                    nameof(EventMessageLogHostedService),
                    () => this.ProcessPendingEventMessages(),
                    "*/5 * * * * *"); // every 5 seconds

            await Task.CompletedTask;
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
                    .OrderBy(m => m.Id)
                    .ToList();

            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    this.publisher.Publish(message.Data, message.Type).GetAwaiter().GetResult();

                    message.MarkAsPublished();

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
