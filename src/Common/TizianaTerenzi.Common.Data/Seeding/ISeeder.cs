namespace TizianaTerenzi.Common.Data.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface ISeeder<TDbContext>
        where TDbContext : DbContext
    {
        Task SeedAsync(TDbContext dbContext, IServiceProvider serviceProvider);
    }
}
