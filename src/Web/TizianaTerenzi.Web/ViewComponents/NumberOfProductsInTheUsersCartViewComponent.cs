namespace TizianaTerenzi.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Web.ViewModels.Cart;

    public class NumberOfProductsInTheUsersCartViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<Cart> productsInTheCartRepository;

        public NumberOfProductsInTheUsersCartViewComponent(
            IDeletableEntityRepository<Cart> productsInTheCartRepository)
        {
            this.productsInTheCartRepository = productsInTheCartRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.HttpContext.User.GetUserId();

                var count = await this.productsInTheCartRepository
                                        .AllAsNoTracking()
                                        .Where(p => p.UserId == userId)
                                        .Select(p => p.Quantity)
                                        .SumAsync();

                var viewModel = new NumberOfProductsInTheUsersCartViewModel
                {
                    Count = count,
                };

                return this.View(viewModel);
            }

            return this.View(new NumberOfProductsInTheUsersCartViewModel { Count = 0 });
        }
    }
}
