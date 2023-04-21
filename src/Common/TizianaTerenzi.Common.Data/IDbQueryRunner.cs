namespace TizianaTerenzi.Common.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public interface IDbQueryRunner<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        Task RunQueryAsync(string query, params object[] parameters);
    }
}
