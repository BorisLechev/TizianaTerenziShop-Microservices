namespace TizianaTerenzi.Carts.Services.Data.Countries
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICountriesService
    {
        Task<IEnumerable<SelectListItem>> GetAllCountriesAsync();

        Task<int> GetCountryIdByNameAsync(string countryName);
    }
}
