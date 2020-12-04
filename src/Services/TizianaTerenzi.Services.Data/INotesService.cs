namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface INotesService
    {
        Task<bool> CreateNoteAsync(string noteName);

        Task<bool> CreateNotesRangeAsync(IEnumerable<Note> notes);

        Task<IEnumerable<Note>> GetAllNotesAsync();

        Task<Note> FindNoteByNameAsync(string noteName);

        Task<IEnumerable<int>> GetAllNoteIdsByProductAsync(int? productId);

        Task<bool> DeleteProductNotesAsync(int? productId);
    }
}
