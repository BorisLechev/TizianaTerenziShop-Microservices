namespace TizianaTerenzi.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.PersonalData;
    using TizianaTerenzi.Services.PDF;
    using TizianaTerenzi.Web.ViewModels.PDF;
    using TizianaTerenzi.Web.ViewModels.Profile;

    [Authorize]
    public class ProfileController : BaseController
    {
        private const string PersonalDataFileName = "{0}_PersonalData_{1}_{2}.json";

        private const int UsersPerPage = 6;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IPersonalDataService personalDataService;

        private readonly IOrdersService ordersService;

        private readonly IViewRenderService viewRenderService;

        private readonly IHtmlToPdfConverter htmlToPdfConverter;

        private readonly IWebHostEnvironment environment;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPersonalDataService personalDataService,
            IOrdersService ordersService,
            IViewRenderService viewRenderService,
            IHtmlToPdfConverter htmlToPdfConverter,
            IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.personalDataService = personalDataService;
            this.ordersService = ordersService;
            this.viewRenderService = viewRenderService;
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.environment = environment;
        }

        [Route("/profile/{userId}")]
        public async Task<IActionResult> Index(string userId)
        {
            var user = await this.personalDataService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var group = new List<string>() { currentUser.UserName, user.UserName };
            var groupName = string.Join(GlobalConstants.ChatGroupNameSeparator, group.OrderBy(x => x));

            var profileViewModel = new ProfileViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Username = user.UserName,
                Address = user.Address,
                PostalCode = user.PostalCode,
                Town = user.Town,
                Phone = user.PhoneNumber,
                GroupName = groupName,
            };

            return this.View(profileViewModel);
        }

        [HttpPost]
        [Route("/profile/download")]
        public async Task<IActionResult> DownloadPersonalData(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                (password != null &&
                await this.userManager.CheckPasswordAsync(user, password));

            if (passwordValid == false)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.RedirectToAction(nameof(this.Index));
            }

            var json = await this.personalDataService.GetPersonalDataForUserJsonAsync(user.Id);

            this.Response.Headers.Add("Content-Disposition", "attachment; filename=" + string.Format(PersonalDataFileName, GlobalConstants.SystemName, user.FirstName, user.LastName));

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "text/json");
        }

        [HttpPost]
        [Route("/profile/delete")]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                                (password != null &&
                                await this.userManager.CheckPasswordAsync(user, password));

            if (passwordValid == false)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.LocalRedirect($"/profile/{user.Id}");
            }

            var result = await this.personalDataService.DeleteUserAsync(user.Id);

            if (result == false)
            {
                this.Error(NotificationMessages.AccountDeleteError);

                return this.LocalRedirect($"/profile/{user.Id}");
            }

            await this.signInManager.SignOutAsync();

            this.Success(NotificationMessages.AccountDeleted);

            return this.LocalRedirect("/");
        }

        [Route("/profile/orders/my")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = this.userManager.GetUserId(this.User);

            var allOrdersByUser = await this.ordersService.GetAllOrdersByUserIdAsync(userId);

            return this.View(allOrdersByUser);
        }

        [Route("/profile/order/{orderId}")]
        public async Task<IActionResult> MyOrderProducts(int orderId)
        {
            var allOrderProductsByUser = await this.ordersService.GetAllOrderProductsByOrderIdAsync(orderId);

            return this.View(allOrderProductsByUser);
        }

        public async Task<IActionResult> GeneratePdf(int orderId)
        {
            var orderProducts = await this.ordersService.GetAllOrderProductsByOrderIdAsync(orderId);
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new ExportPdfUserOrderProductsViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Products = orderProducts,
            };

            var htmlData = await this.RenderViewAsync("GeneratePdf", viewModel);

            this.Response.Headers.Add("Content-Disposition", "attachment; filename=" + string.Format("{0}_Order.pdf", user.UserName));

            var fileContents = this.htmlToPdfConverter.Convert($"{this.environment.WebRootPath}/pdf", htmlData, FormatType.A4, OrientationType.Portrait);

            return this.File(fileContents, "application/pdf");
        }

        [Route("/profile/all")]
        public async Task<IActionResult> All(int page = 0)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * UsersPerPage;

            var usersViewModel = await this.personalDataService.GetAllUsersExceptCurrentLoggedInUserAsync(page, UsersPerPage, skip);

            return this.View(usersViewModel);
        }
    }
}
