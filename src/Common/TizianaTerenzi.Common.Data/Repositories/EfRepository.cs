namespace TizianaTerenzi.Common.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    public class EfRepository<TEntity> : IRepository<TEntity>
         where TEntity : class
    {
        public EfRepository(DbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected DbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => this.DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

        public virtual Task AddAsync(TEntity entity, params EventMessageLog[] messages)
        {
            foreach (var message in messages)
            {
                this.Context.AddAsync(message).AsTask();
            }

            return this.DbSet.AddAsync(entity).AsTask();
        }

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities) => this.DbSet.AddRangeAsync(entities);

        public virtual Task UpdateAsync(TEntity entity, params EventMessageLog[] messages)
        {
            foreach (var message in messages)
            {
                this.Context.AddAsync(message).AsTask();
            }

            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Task CreateEventMessageLog(params EventMessageLog[] messages)
        {
            foreach (var message in messages)
            {
                this.Context.AddAsync(message).AsTask();
            }

            return Task.CompletedTask;
        }

        public async Task MarkEventMessageLogAsPublished(params int[] ids)
        {
            foreach (var id in ids)
            {
                var eventMessageLog = await this.Context.FindAsync<EventMessageLog>(id);

                if (eventMessageLog == null)
                {
                    throw new ArgumentNullException(nameof(eventMessageLog));
                }

                eventMessageLog.MarkAsPublished();

                await this.Context.SaveChangesAsync();
            }
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
