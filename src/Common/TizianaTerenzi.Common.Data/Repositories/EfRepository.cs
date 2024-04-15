namespace TizianaTerenzi.Common.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Services.EventualConsistencyMessages;

    public class EfRepository<TEntity> : IRepository<TEntity>
         where TEntity : class
    {
        public EfRepository(
            DbContext context,
            IPublisher publisher)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
            this.Publisher = publisher;
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected DbContext Context { get; set; }

        protected IPublisher Publisher { get; }

        public virtual IQueryable<TEntity> All() => this.DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

        public virtual Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities) => this.DbSet.AddRangeAsync(entities);

        public virtual void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual async Task<int> SaveAndPublishEventMessageAsync(params object[] messages)
        {
            var dataMessages = messages
                                .ToDictionary(message => message, message => new EventMessageLog(message));

            if (this.Context is EventMessageLogDbContext)
            {
                foreach (var (_, eventMessageLog) in dataMessages)
                {
                    await this.Context.AddAsync(eventMessageLog);
                }
            }

            var result = await this.Context.SaveChangesAsync();

            if (this.Context is EventMessageLogDbContext)
            {
                foreach (var (message, eventMessageLog) in dataMessages)
                {
                    await this.Publisher.Publish(message);

                    eventMessageLog.MarkAsPublished();

                    await this.Context.SaveChangesAsync();
                }
            }

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }
        }
    }
}
