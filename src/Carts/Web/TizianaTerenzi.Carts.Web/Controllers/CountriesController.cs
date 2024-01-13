namespace TizianaTerenzi.Carts.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Carts.Services.Data.Countries;
    using TizianaTerenzi.Common.Web.Controllers;

    public class CountriesController : ApiController
    {
        private readonly ICountriesService countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SelectListItem>>> GetAll()
        {
            var countries = await this.countriesService.GetAllCountriesAsync();

            return this.Ok(countries);
        }
    }
}
