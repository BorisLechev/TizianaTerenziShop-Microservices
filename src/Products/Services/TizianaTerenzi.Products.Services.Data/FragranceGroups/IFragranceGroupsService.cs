namespace TizianaTerenzi.Products.Services.Data.FragranceGroups
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IFragranceGroupsService
    {
        Task<IEnumerable<SelectListItem>> GetAllFragranceGroupsAsync();
    }
}
