namespace TizianaTerenzi.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.Profile;
    using TizianaTerenzi.Web.ViewModels.Profile;

    [Authorize]
    public class ProfileController : BaseController
    {
        private const string PersonalDataFileName = "{0}_PersonalData_{1}_{2}.json";

        private const int UsersPerPage = 6;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IProfileService profileService;

        private readonly IOrdersService ordersService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IProfileService profileService,
            IOrdersService ordersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.profileService = profileService;
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var user = await this.profileService.GetUserByIdAsync(userId);

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
        [ActionName("Download")]
        public async Task<IActionResult> DownloadPersonalData(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                (password != null &&
                await this.userManager.CheckPasswordAsync(user, password));

            if (passwordValid == false)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
            }

            var json = await this.profileService.GetPersonalDataForUserJsonAsync(user.Id);

            this.Response.Headers.Add("Content-Disposition", "attachment; filename=" + string.Format(PersonalDataFileName, GlobalConstants.SystemName, user.FirstName, user.LastName));

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "text/json");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                                (password != null &&
                                await this.userManager.CheckPasswordAsync(user, password));

            if (passwordValid == false)
            {
                this.Error(NotificationMessages.InvalidPassword);

                return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
            }

            var result = await this.profileService.DeleteUserAsync(user);

            if (result == false)
            {
                this.Error(NotificationMessages.AccountDeleteError);

                return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
            }

            await this.signInManager.SignOutAsync();

            this.Success(NotificationMessages.AccountDeleted);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword == false)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var inputModel = new UserChangePasswordInputModel
            {
                Email = user.Email,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePasswordInputModel inputModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, inputModel.OldPassword, inputModel.NewPassword);

            if (changePasswordResult.Succeeded == false)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                inputModel.Email = user.Email;

                return this.View(inputModel);
            }

            await this.signInManager.RefreshSignInAsync(user);

            this.Success(NotificationMessages.PasswordChanged);

            return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound(NotificationMessages.UserNotFound);
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var inputModel = new UserSetPasswordInputModel
            {
                Email = user.Email,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(UserSetPasswordInputModel inputModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (await this.userManager.HasPasswordAsync(user))
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var addPasswordResult = await this.userManager.AddPasswordAsync(user, inputModel.NewPassword);

            if (addPasswordResult.Succeeded == false)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                inputModel.Email = user.Email;

                return this.View(inputModel);
            }

            await this.signInManager.RefreshSignInAsync(user);

            this.Success(NotificationMessages.PasswordSet);

            return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound(NotificationMessages.UserNotFound);
            }

            var inputModel = await this.profileService.GetDetailsForUserEditAsync(user.Id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                inputModel.Email = user.Email;
                inputModel.UserName = user.UserName;

                return this.View(inputModel);
            }

            var result = await this.profileService.EditUserDetailsAsync(user, inputModel);

            if (result)
            {
                this.Success(NotificationMessages.ProfileDetailsUpdated);
            }
            else
            {
                this.Error(NotificationMessages.CannotUpdateProfileDetails);
            }

            return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
        }

        public async Task<IActionResult> All(int page = 0)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * UsersPerPage;

            var usersViewModel = await this.profileService.GetAllUsersExceptAdminsAsync(page, UsersPerPage, skip);

            return this.View(usersViewModel);
        }
    }
}
