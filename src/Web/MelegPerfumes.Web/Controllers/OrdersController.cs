namespace MelegPerfumes.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        private readonly ICartService cartService;

        private readonly IOrdersService ordersService;

        public OrdersController(
            ICartService cartService,
            IOrdersService ordersService)
        {
            this.cartService = cartService;
            this.ordersService = ordersService;
        }

        [HttpGet]
        [Route("/order/cart")]
        public async Task<IActionResult> Cart()
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var productsInTheCart = await this.cartService.GetAllProductsInTheCartByUserId(user);

            return this.View(productsInTheCart);
        }

        [HttpGet]
        [Route("/order/{orderId}/quantity/increase")]
        public async Task<IActionResult> IncreaseQuantity(string orderId)
        {
            bool result = await this.cartService.IncreaseQuantity(orderId);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpGet]
        [Route("/order/{orderId}/quantity/reduce")]
        public async Task<IActionResult> ReduceQuantity(string orderId)
        {
            bool result = await this.cartService.ReduceQuantity(orderId);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        public async Task<IActionResult> DeleteOrder(string id)
        {
            // TODO: Check user if has rights to delete by id
            bool result = await this.cartService.DeleteProductInTheCart(id);

            // TODO: Success/Error Message
            //if (result)
            //{
            //    return this.Ok();
            //}
            //else
            //{
            //    return this.Forbid();
            //}

            return this.RedirectToAction("Cart");
        }

        //[HttpPost]
        [Route("/orders/complete")]
        public async Task<IActionResult> Complete(OrderInputModel inputModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var productsInTheCart = await this.cartService
                 .GetAllProductsInTheCartByUserId(userId);

            var orderProducts = productsInTheCart
                .Select(op => new OrderProduct
                {
                    ProductId = op.ProductId,
                    Price = op.ProductPrice,
                    Quantity = op.Quantity,
                    CreatedOn = DateTime.UtcNow,
                    UserId = userId,
                })
                .ToList();

            await this.ordersService.CreateOrderAsync(userId, orderProducts);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
