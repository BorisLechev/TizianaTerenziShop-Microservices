namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Services.Orders;

    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.GetUserId();

            var allOrdersByUser = await this.ordersService.GetAllOrdersByUserIdAsync(userId);

            return this.View(allOrdersByUser);
        }

        public async Task<IActionResult> Products(int id)
        {
            var allOrderProductsByUser = await this.ordersService.GetAllOrderProductsByOrderIdAsync(id);

            return this.View(allOrderProductsByUser);
        }

        //public async Task<IActionResult> GeneratePdf(int orderId)
        //{
        //    var orderProducts = await this.ordersService.GetAllOrderProductsByOrderIdAsync(orderId);
        //    var user = await this.userManager.GetUserAsync(this.User);

        //    var viewModel = new ExportPdfUserOrderProductsViewModel
        //    {
        //        FullName = $"{user.FirstName} {user.LastName}",
        //        Email = user.Email,
        //        Products = orderProducts,
        //    };

        //    var htmlData = await this.RenderViewAsync("GeneratePdf", viewModel);

        //    this.Response.Headers.Add("Content-Disposition", "attachment; filename=" + string.Format("{0}_Order.pdf", user.UserName));

        //    var fileContents = this.htmlToPdfConverter.Convert($"{this.environment.WebRootPath}/pdf", htmlData, FormatType.A4, OrientationType.Portrait);

        //    return this.File(fileContents, "application/pdf");
        //}
    }
}
