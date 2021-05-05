namespace TizianaTerenzi.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IProductsService productsService;

        private readonly IProductVotesService productVotesService;

        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsController(
            IDeletableEntityRepository<Product> productsRepository,
            IProductsService productsService,
            IProductVotesService productVotesService)
        {
            this.productsService = productsService;
            this.productVotesService = productVotesService;
            this.productsRepository = productsRepository;
        }

        public async Task<IActionResult> All(string search, string criteria, int page = 1)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * ItemsPerPage;

            var query = this.productsRepository.AllAsNoTracking();
            var words = search?
                        .Split(' ')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2)
                        .ToList();

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

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsService.GetProductByIdAsync<ProductDetailsViewModel>(id);

            if (productDetailsViewModel == null)
            {
                return this.NotFound();
            }

            var allValues = await this.productVotesService.GetAllValuesByProductIdAsync(id);
            var numberOfVoters = allValues.Count();

            productDetailsViewModel.NumberOfVoters = numberOfVoters;

            var countOfVotesWithValueFive = allValues.Where(pv => pv == 5).Count();
            var countOfVotesWithValueFour = allValues.Where(pv => pv == 4).Count();
            var countOfVotesWithValueThree = allValues.Where(pv => pv == 3).Count();
            var countOfVotesWithValueTwo = allValues.Where(pv => pv == 2).Count();
            var countOfVotesWithValueOne = allValues.Where(pv => pv == 1).Count();

            productDetailsViewModel.ShareOfVotesWithValueOfFive = countOfVotesWithValueFive > 0 ? (double)countOfVotesWithValueFive / numberOfVoters * 100 : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfFour = countOfVotesWithValueFour > 0 ? (double)countOfVotesWithValueFour / numberOfVoters * 100 : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfThree = countOfVotesWithValueThree > 0 ? (double)countOfVotesWithValueThree / numberOfVoters * 100 : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfTwo = countOfVotesWithValueTwo > 0 ? (double)countOfVotesWithValueTwo / numberOfVoters * 100 : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfOne = countOfVotesWithValueOne > 0 ? (double)countOfVotesWithValueOne / numberOfVoters * 100 : 0;

            var relatedProducts = await this.productsService.GetRandomRelatedProductsAsync(id);
            productDetailsViewModel.RelatedProducts = relatedProducts;

            return this.View(productDetailsViewModel);
        }
    }
}
