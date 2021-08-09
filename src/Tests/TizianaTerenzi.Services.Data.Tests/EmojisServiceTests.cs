namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Chat;
    using Xunit;

    public class EmojisServiceTests
    {
        [Fact]
        public async Task GetAllEmojisSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var emoji = new Emoji
            {
                Image = "smile",
            };

            await dbContext.Emojis.AddAsync(emoji);
            await dbContext.SaveChangesAsync();

            var service = new EmojisService(new EfDeletableEntityRepository<Emoji>(dbContext));

            // Act
            var emojis = await service.GetAllEmojisAsync();

            // Assert
            Assert.Single(emojis);
            Assert.Equal(emoji.Image, emoji.Image);
        }
    }
}
