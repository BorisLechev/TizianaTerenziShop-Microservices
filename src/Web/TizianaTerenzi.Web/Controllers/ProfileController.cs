namespace TizianaTerenzi.Web.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Web.ViewModels.Profile;

    [Authorize]
    public class ProfileController : BaseController
    {
        private const string PersonalDataFileName = "{0}_PersonalData_{1}_{2}.json";

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IPersonalDataService personalDataService;

        private readonly IOrdersService ordersService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPersonalDataService personalDataService,
            IOrdersService ordersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.personalDataService = personalDataService;
            this.ordersService = ordersService;
        }

        [Route("/profile")]
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var profileViewModel = new ProfileViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
            };

            return this.View(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPersonalData(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                (password != null &&
                await this.userManager.CheckPasswordAsync(user, password));

            if (!passwordValid)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.RedirectToAction("Index");
            }

            var json = await this.personalDataService.GetPersonalDataForUserJsonAsync(user.Id);

            this.Response.Headers.Add("Content-Disposition",
                "attachment; filename=" + string.Format(PersonalDataFileName, GlobalConstants.SystemName, user.FirstName, user.LastName));

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "text/json");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                                (password != null &&
                                await this.userManager.CheckPasswordAsync(user, password));

            if (!passwordValid)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.RedirectToAction("Index");
            }

            var result = await this.personalDataService.DeleteUserAsync(user.Id);

            if (!result)
            {
                this.Error(NotificationMessages.AccountDeleteError);

                return this.RedirectToAction("Index");
            }

            await this.signInManager.SignOutAsync();

            this.Success(NotificationMessages.AccountDeleted);

            return this.LocalRedirect("/");
        }

        [Route("/profile/orders/my")]
        public async Task<IActionResult> MyOrders()
        {
            var allOrdersByUser = await this.ordersService
                .GetAllOrdersByUserAsync(this.User.Identity.Name);

            return this.View(allOrdersByUser);
        }

        [Route("/profile/order/{orderId}")]
        public async Task<IActionResult> MyOrderProducts(int orderId)
        {
            var allOrderProductsByUser = await this.ordersService
                .GetAllOrderProductsByUserAsync(this.User.Identity.Name, orderId);

            return this.View(allOrderProductsByUser);
        }
    }
}
