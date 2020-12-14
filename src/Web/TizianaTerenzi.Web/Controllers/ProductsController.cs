namespace TizianaTerenzi.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data;

    public class ProductsController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IProductsService productsService;

        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsController(
            IProductsService productsService,
            IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsService = productsService;
            this.productsRepository = productsRepository;
        }

        public async Task<IActionResult> All(string search, string criteria, int page = 1)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * ItemsPerPage;

            var query = this.productsRepository.AllAsNoTracking();
            var words = search?.Split(' ').Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2).ToList();

            if (words != null)
            {
                foreach (var word in words)
                {
                    query = query.Where(c => EF.Functions.FreeText(c.SearchText, word));
                }
            }

            var productsViewModel = await this.productsService.GetAllProductsAsync(query, search, criteria, page, ItemsPerPage, skip);

            this.ViewData["criteria"] = criteria;

            return this.View(productsViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsService.GetProductDetailsByIdAsync(id);

            if (productDetailsViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(productDetailsViewModel);
        }
    }
}
