namespace TizianaTerenzi.Services.Data.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICountriesService
    {
        Task<IEnumerable<SelectListItem>> GetAllCountriesAsync();
    }
}
