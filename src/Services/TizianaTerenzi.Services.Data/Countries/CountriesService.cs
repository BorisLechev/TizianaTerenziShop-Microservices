namespace TizianaTerenzi.Services.Data.Countries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class CountriesService : ICountriesService
    {
        private readonly IRepository<Country> countriesRepository;

        public CountriesService(
            IRepository<Country> countriesRepository)
        {
            this.countriesRepository = countriesRepository;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllCountriesAsync()
        {
            var countries = await this.countriesRepository
                .AllAsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                })
                .ToListAsync();

            return countries;
        }

        public async Task<int> GetCountryIdByNameAsync(string countryName)
        {
            var countryId = await this.countriesRepository
                .AllAsNoTracking()
                .Where(c => c.Name == countryName)
                .Select(c => c.Id)
                .SingleOrDefaultAsync();

            return countryId;
        }
    }
}
