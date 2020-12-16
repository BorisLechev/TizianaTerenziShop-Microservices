namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Data.Models;

    public interface INotesService
    {
        Task<bool> CreateNoteAsync(string noteName);

        Task<IEnumerable<SelectListItem>> GetAllNotesAsync();

        Task<Note> FindNoteByNameAsync(string noteName);

        Task<IEnumerable<SelectListItem>> GetAllNotesWithSelectedByProductIdAsync(int productId);

        Task<bool> DeleteProductNotesAsync(int productId);
    }
}
