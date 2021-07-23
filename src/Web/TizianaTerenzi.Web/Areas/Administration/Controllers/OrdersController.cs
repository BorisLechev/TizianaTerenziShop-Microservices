namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Orders;

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

            return this.View(orders);
        }

        public async Task<IActionResult> Pending()
        {
            var orders = await this.ordersService.GetAllPendingOrdersAsync();

            return this.View(orders);
        }

        public async Task<IActionResult> Processed()
        {
            var orders = await this.ordersService.GetAllProcessedOrdersAsync();

            return this.View(orders);
        }

        public async Task<IActionResult> Products(int id)
        {
            var orderProducts = await this.ordersService.GetAllOrderProductsByOrderIdAsync(id);

            return this.View(orderProducts);
        }

        public async Task<IActionResult> Process(int orderId)
        {
            var result = await this.ordersService.ProcessOrderAsync(orderId);

            if (result == false)
            {
                this.Error(NotificationMessages.ProcessOrderError);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.ProcessOrderSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
