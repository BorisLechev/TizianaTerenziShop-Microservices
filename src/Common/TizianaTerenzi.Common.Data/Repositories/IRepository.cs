namespace TizianaTerenzi.Common.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Common.Data.Models;

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        Task AddAsync(TEntity entity, params EventMessageLog[] messages);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task CreateEventMessageLog(params EventMessageLog[] messages);

        Task UpdateAsync(TEntity entity, params EventMessageLog[] messages);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();

        Task MarkEventMessageLogAsPublished(int id);
    }
}
