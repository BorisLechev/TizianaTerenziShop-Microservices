namespace TizianaTerenzi.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Cart;

    public class NumberOfProductsInTheUsersCartViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository;

        private readonly UserManager<ApplicationUser> userManager;

        public NumberOfProductsInTheUsersCartViewComponent(
            IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.productsInTheCartRepository = productsInTheCartRepository;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(this.HttpContext.User);

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
