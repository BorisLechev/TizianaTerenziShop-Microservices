namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Account;

    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<AccountController> logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
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

            if (!hasPassword)
            {
                return this.RedirectToAction("SetPassword");
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
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("ChangePassword");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, inputModel.OldPassword, inputModel.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                inputModel.Email = user.Email;

                return this.View(inputModel);
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.logger.LogInformation("User changed their password successfully.");

            this.Success(NotificationMessages.PasswordChanged);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound();
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToAction("ChangePassword");
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
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("SetPassword");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (await this.userManager.HasPasswordAsync(user))
            {
                return this.RedirectToAction("ChangePassword");
            }

            var addPasswordResult = await this.userManager.AddPasswordAsync(user, inputModel.NewPassword);

            if (!addPasswordResult.Succeeded)
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

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound();
            }

            var inputModel = new UserEditInputModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

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

            if (!this.ModelState.IsValid)
            {
                inputModel.Email = user.Email;

                return this.View(inputModel);
            }

            user.FirstName = inputModel.FirstName;
            user.LastName = inputModel.LastName;

            await this.userManager.UpdateAsync(user);

            this.Success(NotificationMessages.ProfileDetailsUpdated);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
