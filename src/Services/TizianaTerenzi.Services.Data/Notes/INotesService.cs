namespace TizianaTerenzi.Services.Data.Notes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface INotesService
    {
        Task<bool> CreateNoteAsync(string noteName);

        Task<IEnumerable<SelectListItem>> GetAllNotesAsync();

        Task<bool> FindNoteByNameAsync(string noteName);

        Task<IEnumerable<SelectListItem>> GetAllNotesWithSelectedByProductIdAsync(int productId);

        Task<bool> SoftDeleteAllProductNotesAsync(int productId);

        Task<int> HardDeleteAllProductNotesAsync(int productId);
    }
}
