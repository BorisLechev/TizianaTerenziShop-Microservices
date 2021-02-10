namespace TizianaTerenzi.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Wishlist;

    [Authorize]
    public class WishlistController : BaseController
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(
            IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var productsInWishlistViewModel = await this.wishlistService.GetAllProductsFromUserWishlistAsync(userId);

            return this.View(productsInWishlistViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isProductAdded = await this.wishlistService.IsTheProductAlreadyAddedInWishlistAsync(productId, userId);

            if (isProductAdded)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var result = await this.wishlistService.AddProductToTheWishlistAsync(productId, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotAddProductInTheWishlist);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.AddProductInTheWishlistSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await this.wishlistService.DeleteProductInTheWishlistAsync(productId, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotDeleteProductInTheWishlist);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.DeleteProductInTheWishlistSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
