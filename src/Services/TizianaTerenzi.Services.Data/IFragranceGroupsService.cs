namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Data.Models;

    public interface IFragranceGroupsService
    {
        Task<bool> CreateFragranceGroupAsync(FragranceGroup fragranceGroup);

        Task<IEnumerable<SelectListItem>> GetAllFragranceGroupsAsync();
    }
}
