namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Notes;
    using Xunit;

    public class NotesServiceTests : BaseServiceTests
    {
        private INotesService Service => this.ServiceProvider.GetRequiredService<INotesService>();

        [Fact]
        public async Task FindNoteByNameShouldReturnTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var note = new Note
            {
                Name = "Test",
            };

            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            var service = new NotesService(
                new EfRepository<Note>(dbContext),
                null);

            // Act
            var result = await service.FindNoteByNameAsync(note.Name);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllFragranceGroupsShouldReturnSelectListItemArray()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.Notes.AddRangeAsync(
                new Note
                {
                    Name = "Test1",
                },
                new Note
                {
                    Name = "Test2",
                },
                new Note
                {
                    Name = "Test3",
                });
            await dbContext.SaveChangesAsync();

            var service = new NotesService(
                new EfRepository<Note>(dbContext),
                null);

            // Act
            var notes = await service.GetAllNotesAsync();
            var isExistingNote1 = await service.FindNoteByNameAsync("Test1");
            var isExistingNote2 = await service.FindNoteByNameAsync("Test2");
            var isExistingNote3 = await service.FindNoteByNameAsync("Test3");
            var note1 = notes.First(n => n.Text == "Test1");
            var note2 = notes.First(n => n.Text == "Test2");
            var note3 = notes.First(n => n.Text == "Test3");

            Assert.Equal(3, notes.Count());
            Assert.True(isExistingNote1);
            Assert.True(isExistingNote2);
            Assert.True(isExistingNote3);
            Assert.Equal("Test1", note1.Text);
            Assert.Equal("Test2", note2.Text);
            Assert.Equal("Test3", note3.Text);
        }

        [Fact]
        public async Task CreateNoteShouldReturnTrue()
        {
            // Act
            var result = await this.Service.CreateNoteAsync("Test");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreateNoteShouldReturnFalseWhenTryToCreateTwoNotesWithSameName()
        {
            // Act
            var result = await this.Service.CreateNoteAsync("Test");
            var secondResult = await this.Service.CreateNoteAsync("Test");

            // Assert
            Assert.True(result);
            Assert.False(secondResult);
        }

        [Fact]
        public async Task GetAllSelectedFragranceGroupsShouldReturnSelectListItemArray()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.Notes.AddRangeAsync(new List<Note>
            {
                new Note
                {
                    Name = "Test1",
                },
                new Note
                {
                    Name = "Test2",
                },
            });
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            await dbContext.ProductNotes.AddRangeAsync(new List<ProductNote>
            {
                new ProductNote
                {
                    NoteId = 1,
                    ProductId = 1,
                },
                new ProductNote
                {
                    NoteId = 2,
                    ProductId = 1,
                },
            });
            await dbContext.SaveChangesAsync();

            var service = new NotesService(
                new EfRepository<Note>(dbContext),
                new EfDeletableEntityRepository<ProductNote>(dbContext));

            // Act
            var productNotes = await service.GetAllNotesWithSelectedByProductIdAsync(product.Id);
            var isExistingNote1 = await service.FindNoteByNameAsync("Test1");
            var isExistingNote2 = await service.FindNoteByNameAsync("Test2");
            var note1 = productNotes.First(n => n.Text == "Test1");
            var note2 = productNotes.First(n => n.Text == "Test2");

            // Assert
            Assert.Equal(2, productNotes.Count());
            Assert.True(isExistingNote1);
            Assert.True(isExistingNote2);
            Assert.Equal("Test1", note1.Text);
            Assert.Equal("Test2", note2.Text);
        }

        [Fact]
        public async Task SoftDeleteAllProductNotesShouldReturnTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.Notes.AddRangeAsync(new List<Note>
            {
                new Note
                {
                    Name = "Test1",
                },
                new Note
                {
                    Name = "Test2",
                },
            });
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            await dbContext.ProductNotes.AddRangeAsync(new List<ProductNote>
            {
                new ProductNote
                {
                    NoteId = 1,
                    ProductId = 1,
                },
                new ProductNote
                {
                    NoteId = 2,
                    ProductId = 1,
                },
            });
            await dbContext.SaveChangesAsync();

            var service = new NotesService(
                new EfRepository<Note>(dbContext),
                new EfDeletableEntityRepository<ProductNote>(dbContext));

            // Act
            var softDelete = await service.SoftDeleteAllProductNotesAsync(product.Id);

            // Assert
            Assert.True(softDelete);
        }

        [Fact]
        public async Task HardDeleteAllProductNotesShouldReturnTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.Notes.AddRangeAsync(new List<Note>
            {
                new Note
                {
                    Name = "Test1",
                },
                new Note
                {
                    Name = "Test2",
                },
            });
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            await dbContext.ProductNotes.AddRangeAsync(new List<ProductNote>
            {
                new ProductNote
                {
                    NoteId = 1,
                    ProductId = 1,
                },
                new ProductNote
                {
                    NoteId = 2,
                    ProductId = 1,
                },
            });
            await dbContext.SaveChangesAsync();

            var service = new NotesService(
                new EfRepository<Note>(dbContext),
                new EfDeletableEntityRepository<ProductNote>(dbContext));

            // Act
            var hardDelete = await service.HardDeleteAllProductNotesAsync(product.Id);

            // Assert
            Assert.True(hardDelete);
        }
    }
}
