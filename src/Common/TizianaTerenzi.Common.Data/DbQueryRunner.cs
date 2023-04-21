namespace TizianaTerenzi.Common.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public class DbQueryRunner<TDbContext> : IDbQueryRunner<TDbContext>
        where TDbContext : DbContext
    {
        public DbQueryRunner(TDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TDbContext Context { get; set; }

        public Task RunQueryAsync(string query, params object[] parameters)
        {
            return this.Context.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
