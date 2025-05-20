namespace TizianaTerenzi.Common.Data.EventualConsistencyMessages
{
    using Microsoft.EntityFrameworkCore;

    public class EventualConsistencyMessagesService : IEventualConsistencyMessagesService
    {
        private readonly EventMessageLogDbContext dbContext;

        public EventualConsistencyMessagesService(DbContext dbContext)
        {
            this.dbContext = dbContext as EventMessageLogDbContext
                ?? throw new InvalidOperationException($"Eventual Consistency Messages can only be used with a {nameof(EventMessageLogDbContext)}.");
        }

        public async Task<bool> IsDuplicated(object eventMessageData, string propertyFilter, object identifier)
        {
            var eventMessageType = eventMessageData.GetType();

            var test = await this.dbContext.EventMessageLogs
                .FromSqlInterpolated($"SELECT JSON_VALUE(serializedData, '$.{propertyFilter}') FROM EventMessageLogs WHERE Type = '{eventMessageType.AssemblyQualifiedName}'")
                .FirstOrDefaultAsync();

            return await this.dbContext.EventMessageLogs
                .FromSqlInterpolated($"SELECT * FROM EventMessageLogs WHERE Type = '{eventMessageType.AssemblyQualifiedName}' AND JSON_VALUE(serializedData, '$.{propertyFilter}') = {identifier}")
                .AnyAsync();
        }
    }
}
