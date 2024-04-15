namespace TizianaTerenzi.Common.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task<int> SaveAndPublishEventMessageAsync(params object[] messages);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
