namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.WebClient.Services.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IConfiguration configuration;

        public ProductsController(
            IProductsService productsService,
            IConfiguration configuration)
        {
            this.productsService = productsService;
            this.configuration = configuration;
        }

        public async Task<IActionResult> All(string search, ProductSorting sorting, int page = 1)
        {
            var stripe = this.configuration.GetSection("Stripe:SecretKey");
            var recaptcha = this.configuration.GetSection("GoogleReCaptcha:Secret").Value;
            var recaptchaKey = this.configuration["GoogleReCaptcha:Key"];
            var recaptchaSecret = this.configuration["GoogleReCaptcha:Secret"];

            var productsViewModel = await this.productsService.All(search, sorting, page);

            return this.View(productsViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsService.Details(id);

            if (!productDetailsViewModel.Succeeded)
            {
                return this.NotFound();
            }

            return this.View(productDetailsViewModel.Data);
        }
    }
}
