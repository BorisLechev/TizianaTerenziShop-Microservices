using MelegPerfumes.Data.Common.Repositories;
using MelegPerfumes.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelegPerfumes.Services.Data
{
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
            Note note = new Note
            {
                Name = noteName,
            };

            await this.notesRepository.AddAsync(note);
            int result = await this.notesRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task CreateNotesRangeAsync(IEnumerable<Note> notes)
        {
            await this.notesRepository.AddRangeAsync(notes);
            await this.notesRepository.SaveChangesAsync();
        }

        public async Task<Note> FindNoteByName(string noteName)
        {
            var note = await this.notesRepository
                .All()
                .SingleOrDefaultAsync(n => n.Name == noteName);

            return note;
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            var notes = await this.notesRepository
                .All()
                .OrderBy(n => n.Name)
                .ToListAsync();

            return notes;
        }
    }
}
