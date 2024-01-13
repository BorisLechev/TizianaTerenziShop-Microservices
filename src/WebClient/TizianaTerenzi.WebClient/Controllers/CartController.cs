namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Carts;
    using TizianaTerenzi.WebClient.Services.Products;

    [Authorize]
    public class CartController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICartsService cartsService;
        private readonly ICartsGatewayService cartsGatewayService;

        public CartController(
            IProductsService productsService,
            ICartsService cartsService,
            ICartsGatewayService cartsGatewayService)
        {
            this.productsService = productsService;
            this.cartsService = cartsService;
            this.cartsGatewayService = cartsGatewayService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productsInTheCart = await this.cartsService.GetAllProductsInTheUsersCart();

            return this.View(productsInTheCart);
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(string productId)
        {
            var result = await this.cartsService.IncreaseQuantity(productId);

            if (result.Succeeded)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReduceQuantity(string productId)
        {
            var result = await this.cartsService.ReduceQuantity(productId);

            if (result.Succeeded)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductInTheCart(int productId)
        {
            await this.productsService.AddProductInTheCart(productId);

            return this.NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var result = await this.cartsService.DeleteProductInTheCart(id);

            if (!result.Succeeded)
            {
                this.Error(NotificationMessages.CannotDeleteThisProductInTheCartError);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Checkout()
        {
            var result = await this.cartsGatewayService.Checkout();

            if (!result.Succeeded || result.Data.Products.Count == 0)
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View("~/Views/Payment/Index.cshtml", result.Data);
        }
    }
}
