namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface INotesService
    {
        Task<bool> CreateNoteAsync(string noteName);

        Task CreateNotesRangeAsync(IEnumerable<Note> notes);

        Task<IEnumerable<Note>> GetAllNotesAsync();

        Task<Note> FindNoteByNameAsync(string noteName);
    }
}
