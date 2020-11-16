namespace MelegPerfumes.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Common;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Web.Areas.Administration.Models.Orders;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : AdministrationController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await this.ordersService.GetAllOrdersAsync();

            var model = orders
                .Select(o => new AdminOrdersListingViewModel
                {
                    Id = o.Id,
                    CreatedOn = o.CreatedOn,
                    DiscountCode = o.DiscountCode,
                    Products = o.Products,
                    StatusName = o.StatusName,
                    UserFullName = o.UserFullName,
                })
                .ToList();

            return this.View(model);
        }

        public async Task<IActionResult> Pending()
        {
            var orders = await this.ordersService.GetAllPendingOrdersAsync();

            var model = orders
                .Select(o => new AdminOrdersListingViewModel
                {
                    Id = o.Id,
                    CreatedOn = o.CreatedOn,
                    DiscountCode = o.DiscountCode,
                    Products = o.Products,
                    StatusName = o.StatusName,
                    UserFullName = o.UserFullName,
                })
                .ToList();

            return this.View(model);
        }

        public async Task<IActionResult> Processed()
        {
            var orders = await this.ordersService.GetAllProcessedOrdersAsync();

            var model = orders
                .Select(o => new AdminOrdersListingViewModel
                {
                    Id = o.Id,
                    CreatedOn = o.CreatedOn,
                    DiscountCode = o.DiscountCode,
                    Products = o.Products,
                    StatusName = o.StatusName,
                    UserFullName = o.UserFullName,
                })
                .ToList();

            return this.View(model);
        }

        public async Task<IActionResult> Products(int orderId)
        {
            var orderProducts = await this.ordersService.GetAllOrderProductsAsync(orderId);

            var model = orderProducts
               .Select(op => new AdminOrderProductsListingViewModel
               {
                   Product = op.Product,
                   CreatedOn = op.CreatedOn,
                   DiscountCode = op.DiscountCode,
                   Quantity = op.Quantity,
                   Price = op.Price,
               })
               .ToList();

            return this.View(model);
        }

        public async Task<IActionResult> Process(int orderId)
        {
            var result = await this.ordersService.ProcessOrderAsync(orderId);

            if (result == false)
            {
                this.Error(NotificationMessages.ProcessOrderError);

                return this.RedirectToAction("Index", "Orders");
            }

            this.Success(NotificationMessages.ProcessOrderSuccessfully);

            return this.RedirectToAction("Index", "Orders");
        }
    }
}
