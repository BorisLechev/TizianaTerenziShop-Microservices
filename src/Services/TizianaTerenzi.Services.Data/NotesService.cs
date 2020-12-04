namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class NotesService : INotesService
    {
        private readonly IDeletableEntityRepository<Note> notesRepository;

        private readonly IDeletableEntityRepository<ProductNotes> productNotesRepository;

        public NotesService(
            IDeletableEntityRepository<Note> notesRepository,
            IDeletableEntityRepository<ProductNotes> productNotesRepository)
        {
            this.notesRepository = notesRepository;
            this.productNotesRepository = productNotesRepository;
        }

        public async Task<bool> CreateNoteAsync(string noteName)
        {
            var check = await this.FindNoteByNameAsync(noteName);

            if (check == null)
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

        public async Task<bool> CreateNotesRangeAsync(IEnumerable<Note> notes)
        {
            await this.notesRepository.AddRangeAsync(notes);
            var result = await this.notesRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Note> FindNoteByNameAsync(string noteName)
        {
            var note = await this.notesRepository
                .All()
                .SingleOrDefaultAsync(n => n.Name == noteName);

            return note;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            var notes = await this.notesRepository
                .All()
                .OrderBy(n => n.Name)
                .ToListAsync();

            return notes;
        }

        public async Task<IEnumerable<int>> GetAllNoteIdsByProductAsync(int? productId)
        {
            var notes = await this.productNotesRepository
                .All()
                .Where(pn => pn.ProductId == productId)
                .Select(pn => pn.NoteId)
                .ToListAsync();

            return notes;
        }

        public async Task<bool> DeleteProductNotesAsync(int? productId)
        {
            var productNotes = await this.productNotesRepository
                .All()
                .Where(pn => pn.ProductId == productId)
                .ToListAsync();

            if (productNotes.Any())
            {
                this.productNotesRepository.DeleteRange(productNotes);
                var result = await this.productNotesRepository.SaveChangesAsync();

                return result > 0;
            }

            return false;
        }
    }
}
