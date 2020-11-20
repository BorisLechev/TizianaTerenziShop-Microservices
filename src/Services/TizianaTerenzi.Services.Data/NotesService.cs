namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class NotesService : INotesService
    {
        private readonly IDeletableEntityRepository<Note> notesRepository;

        public NotesService(
            IDeletableEntityRepository<Note> notesRepository)
        {
            this.notesRepository = notesRepository;
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

        public async Task CreateNotesRangeAsync(IEnumerable<Note> notes)
        {
            await this.notesRepository.AddRangeAsync(notes);
            await this.notesRepository.SaveChangesAsync();
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
    }
}
