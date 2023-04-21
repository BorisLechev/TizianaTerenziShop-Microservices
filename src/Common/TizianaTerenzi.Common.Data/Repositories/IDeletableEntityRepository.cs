namespace TizianaTerenzi.Common.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Common.Data.Models;

    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        void DeleteRange(IEnumerable<TEntity> entities);

        void HardDelete(TEntity entity);

        void HardDeleteRange(IEnumerable<TEntity> entities);

        void Undelete(TEntity entity);
    }
}
