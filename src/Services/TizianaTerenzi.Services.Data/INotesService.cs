namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface INotesService
    {
        Task<bool> CreateNoteAsync(string noteName);

        Task CreateNotesRangeAsync(IEnumerable<Note> notes);

        Task<IEnumerable<Note>> GetAllNotesAsync();

        Task<Note> FindNoteByNameAsync(string noteName);
    }
}
