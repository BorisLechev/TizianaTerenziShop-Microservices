namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Products;

    [Authorize]
    public class WishlistController : BaseController
    {
        private readonly IProductsService productsService;

        public WishlistController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> Index()
        {
            var wishlistViewModel = await this.productsService.GetAllProductsFromUsersWishlist();

            return this.View(wishlistViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var result = await this.productsService.AddProductToTheWishlist(productId);

            if (!result.Succeeded)
            {
                if (result.Errors.Contains(NotificationMessages.TheProductHasAlreadyBeenAddedToTheWishlist))
                {
                    this.Error(NotificationMessages.TheProductHasAlreadyBeenAddedToTheWishlist);

                    return this.RedirectToAction(nameof(this.Index));
                }

                this.Error(NotificationMessages.CannotAddProductToTheWishlist);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.AddProductToTheWishlistSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var result = await this.productsService.DeleteProductFromTheWishlist(productId);

            if (!result.Succeeded)
            {
                this.Error(NotificationMessages.CannotDeleteProductFromTheWishlist);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.DeleteProductFromTheWishlistSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
