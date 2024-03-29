namespace TizianaTerenzi.Products.Services.Data.Notes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Products.Data.Models;

    public class NotesService : INotesService
    {
        private readonly IRepository<Note> notesRepository;

        private readonly IDeletableEntityRepository<ProductNote> productNotesRepository;

        public NotesService(
            IRepository<Note> notesRepository,
            IDeletableEntityRepository<ProductNote> productNotesRepository)
        {
            this.notesRepository = notesRepository;
            this.productNotesRepository = productNotesRepository;
        }

        public async Task<bool> CreateNoteAsync(string noteName)
        {
            var isExisting = await this.FindNoteByNameAsync(noteName);

            if (isExisting == false)
            {
                Note note = new Note
                {
                    Name = noteName,
                };

                await this.notesRepository.AddAsync(note);
                int result = await this.notesRepository.SaveChangesAsync();

                return result > 0;
            }

            return false;
        }

        public async Task<bool> FindNoteByNameAsync(string noteName)
        {
            var isExisting = await this.notesRepository
                .AllAsNoTracking()
                .AnyAsync(n => n.Name == noteName);

            return isExisting;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllNotesAsync()
        {
            var notes = await this.notesRepository
                .AllAsNoTracking()
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                })
                .ToListAsync();

            return notes;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllNotesWithSelectedByProductIdAsync(int productId)
        {
            var allProductNoteIds = await this.GetAllProductNoteIdsAsync(productId);
            var notes = await this.notesRepository
                .AllAsNoTracking()
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                    Selected = allProductNoteIds.Any(id => n.Id == id),
                })
                .ToListAsync();

            return notes;
        }

        public async Task<bool> SoftDeleteAllProductNotesAsync(int productId)
        {
            var affectedRows = await this.productNotesRepository
                .All()
                .Where(pn => pn.ProductId == productId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(pn => pn.IsDeleted, true)
                    .SetProperty(pn => pn.DeletedOn, DateTime.UtcNow));

            return affectedRows > 0;
        }

        public async Task<bool> HardDeleteAllProductNotesAsync(int productId)
        {
            var productNotesCount = await this.productNotesRepository
                                        .All()
                                        .Where(pn => pn.ProductId == productId)
                                        .ExecuteDeleteAsync();

            return productNotesCount > 0;
        }

        private async Task<IEnumerable<int>> GetAllProductNoteIdsAsync(int productId)
        {
            var productNoteIds = await this.productNotesRepository
                        .AllAsNoTracking()
                        .Where(pn => pn.ProductId == productId)
                        .Select(n => n.NoteId)
                        .ToListAsync();

            return productNoteIds;
        }
    }
}
