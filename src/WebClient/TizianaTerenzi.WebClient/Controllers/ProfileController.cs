namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Services.Identity;
    using TizianaTerenzi.WebClient.Services.Products;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    [Authorize]
    public class ProfileController : BaseController
    {
        private const string PersonalDataFileName = "{0}_PersonalData_{1}_{2}.json";

        //private readonly IChatService chatsService;

        private readonly IIdentityService identityService;

        private readonly IProductsGatewayService productsGatewayService;

        public ProfileController(
            IIdentityService identityService,
            IProductsGatewayService productsGatewayService)
        //IChatService chatsService,
        {
            //this.chatsService = chatsService;
            this.identityService = identityService;
            this.productsGatewayService = productsGatewayService;
        }

        public async Task<IActionResult> Index(string id)
        {
            var profileViewModel = await this.identityService.Profile(id);

            //if (user == null)
            //{
            //    return this.NotFound();
            //}

            return this.View(profileViewModel);
        }

        [HttpPost]
        [ActionName("Download")]
        public async Task<IActionResult> DownloadPersonalData(string password)
        {
            var userId = this.User.GetUserId();

            var result = await this.productsGatewayService.DownloadPersonalData(password);

            if (!result.Succeeded)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.RedirectToAction(nameof(this.Index), new { Id = result.Data.UserId });
            }

            this.Response.Headers.Add(
                                    "Content-Disposition",
                                    "attachment; filename=" + string.Format(PersonalDataFileName, GlobalConstants.SystemName, result.Data.UserFirstName, result.Data.UserLastName));

            return new FileContentResult(result.Data.File, "text/json");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var userId = this.User.GetUserId();
            var result = await this.productsGatewayService.DeleteAccount(password);

            if (!result.Succeeded)
            {
                //this.Error(NotificationMessages.InvalidPassword);
                this.Error(NotificationMessages.AccountDeleteError);

                return this.RedirectToAction(nameof(this.Index), new { userId = userId });
            }

            this.Success(NotificationMessages.AccountDeleted);

            return this.RedirectToAction("Logout", "Authentication");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var changePasswordGetEmail = await this.identityService.GetUserEmailWhenChangePassword();

            if (changePasswordGetEmail == null)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var inputModel = new UserChangePasswordInputModel
            {
                Email = changePasswordGetEmail.Email,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePasswordInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var changePasswordResult = await this.identityService.ChangePassword(inputModel);

            if (changePasswordResult.Errors.Any())
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error);
                }

                return this.View(inputModel);
            }

            this.Success(NotificationMessages.PasswordChanged);

            return this.RedirectToAction(nameof(this.Index), new { id = this.User.GetUserId() });
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var setPasswordGetEmail = await this.identityService.GetUserEmailWhenSetPassword();

            if (setPasswordGetEmail == null)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var inputModel = new UserSetPasswordInputModel
            {
                Email = setPasswordGetEmail.Email,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(UserSetPasswordInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var setPasswordResult = await this.identityService.SetPassword(inputModel);

            if (setPasswordResult.Errors.Any())
            {
                foreach (var error in setPasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error);
                }

                return this.View(inputModel);
            }

            //if (hasPassword)
            //{
            //    return this.RedirectToAction(nameof(this.ChangePassword));
            //}

            this.Success(NotificationMessages.PasswordSet);

            return this.RedirectToAction(nameof(this.Index), new { id = this.User.GetUserId() });
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var inputModel = await this.identityService.GetDetailsForUserEdit();

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Email = this.User.GetUserEmail();
                inputModel.UserName = this.User.GetUserName();

                return this.View(inputModel);
            }

            var result = await this.identityService.EditUserDetails(inputModel);

            if (result)
            {
                this.Success(NotificationMessages.ProfileDetailsUpdated);
            }
            else
            {
                this.Error(NotificationMessages.CannotUpdateProfileDetails);
            }

            return this.RedirectToAction(nameof(this.Index), new { id = this.User.GetUserId() });
        }

        public async Task<IActionResult> All(int page = 0)
        {
            var usersViewModel = await this.identityService.GetAllUsersExceptAdmins(page);

            return this.View(usersViewModel);
        }
    }
}
