namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Contacts;
    using Xunit;

    public class ContactsServiceTests
    {
        [Fact]
        public async Task GetAllEmailsSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var newContact = new ContactFormEntry
            {
                Name = "John Doe",
                Subject = "Test",
                Content = "Hello",
                Email = "test@abv.bg",
                Ip = "198:11",
            };

            await dbContext.ContactFormEntries.AddAsync(newContact);
            await dbContext.SaveChangesAsync();

            var service = new ContactsService(new EfDeletableEntityRepository<ContactFormEntry>(dbContext));

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(newContact.Name, result.First().Name);
            Assert.Equal(newContact.Subject, result.First().Subject);
            Assert.Equal(newContact.Ip, result.First().Ip);
            Assert.Equal(newContact.Content, result.First().Content);
            Assert.Equal(newContact.Email, result.First().Email);
        }

        [Fact]
        public async Task DeleteEmailSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.ContactFormEntries.AddRangeAsync(
                new ContactFormEntry
                {
                    Name = "John Doe",
                    Subject = "Test",
                    Content = "Hello",
                    Email = "test@abv.bg",
                    Ip = "198:11",
                },
                new ContactFormEntry
                {
                    Name = "John Foe",
                    Subject = "Hello",
                    Content = "Hello",
                    Email = "john@gmail.com",
                    Ip = "198:12",
                });
            await dbContext.SaveChangesAsync();

            var service = new ContactsService(new EfDeletableEntityRepository<ContactFormEntry>(dbContext));

            // Act
            var allEmails = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, allEmails.Count());

            var resultAfterDelete = await service.DeleteAsync(2);
            var allEmailsAfterDeletion = await service.GetAllAsync();

            Assert.True(resultAfterDelete);
            Assert.Single(allEmailsAfterDeletion);
            Assert.Equal("John Doe", allEmailsAfterDeletion.First().Name);
            Assert.Equal("Test", allEmailsAfterDeletion.First().Subject);
            Assert.Equal("198:11", allEmailsAfterDeletion.First().Ip);
            Assert.Equal("Hello", allEmailsAfterDeletion.First().Content);
            Assert.Equal("test@abv.bg", allEmailsAfterDeletion.First().Email);
        }

        [Fact]
        public async Task DeleteEmailShouldReturnFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.ContactFormEntries.AddRangeAsync(
                new ContactFormEntry
                {
                    Name = "John Doe",
                    Subject = "Test",
                    Content = "Hello",
                    Email = "test@abv.bg",
                    Ip = "198:11",
                },
                new ContactFormEntry
                {
                    Name = "John Foe",
                    Subject = "Hello",
                    Content = "Hello",
                    Email = "john@gmail.com",
                    Ip = "198:12",
                });
            await dbContext.SaveChangesAsync();

            var service = new ContactsService(new EfDeletableEntityRepository<ContactFormEntry>(dbContext));

            // Act
            var allEmails = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, allEmails.Count());

            var resultAfterDelete = await service.DeleteAsync(3);
            var allEmailsAfterDeletion = await service.GetAllAsync();

            Assert.False(resultAfterDelete);
            Assert.Equal(2, allEmailsAfterDeletion.Count());
        }
    }
}
