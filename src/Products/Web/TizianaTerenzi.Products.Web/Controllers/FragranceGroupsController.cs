namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Services.Data.FragranceGroups;

    public class FragranceGroupsController : ApiController
    {
        private readonly IFragranceGroupsService fragranceGroupsService;

        public FragranceGroupsController(IFragranceGroupsService fragranceGroupsService)
        {
            this.fragranceGroupsService = fragranceGroupsService;
        }

        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> Index()
        {
            var fragranceGroups = await this.fragranceGroupsService.GetAllFragranceGroupsAsync();

            return fragranceGroups;
        }
    }
}
