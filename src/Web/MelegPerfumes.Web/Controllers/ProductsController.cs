namespace MelegPerfumes.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> All()
        {
            var products = (await this.productsService.GetAllProductsAsync()).ToList();

            return this.View(products);
        }
    }
}
