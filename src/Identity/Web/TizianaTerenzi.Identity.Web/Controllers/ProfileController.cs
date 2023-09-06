namespace TizianaTerenzi.Identity.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Profile;
    using TizianaTerenzi.Identity.Web.Models.Profile;

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProfileController : ApiController
    {
        private const int UsersPerPage = 6;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IProfileService profileService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IProfileService profileService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileViewModel>> Index(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return this.BadRequest(Result.Failure("User not found."));
            }

            var profileViewModel = new ProfileViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Username = user.UserName,
                Address = user.Address,
                PostalCode = user.PostalCode,
                Town = user.Town,
                Phone = user.PhoneNumber,
            };

            //if (user.Id != currentUserId)
            //{
            //    var chatGroupId = await this.chatsService.GetChatGroupByUserIdsAsync(user.Id, currentUserId);

            //    if (chatGroupId == null)
            //    {
            //        chatGroupId = await this.chatsService.AddUserToGroupAsync(chatGroupId, user.UserName, currentUserName);
            //    }

            //    profileViewModel.GroupId = chatGroupId;
            //}

            return profileViewModel;
        }

        public async Task<Result<DownloadPersonalDataViewModel>> GetUsersPersonalDataForExport(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                                (password != null &&
                                await this.userManager.CheckPasswordAsync(user, password));

            if (!passwordValid)
            {
                return Result<DownloadPersonalDataViewModel>.Failure(NotificationMessages.InvalidPassword);
            }

            var usersCommentsAndVotes = await this.profileService.GetPersonalDataForUserJsonAsync(user.Id);

            return Result<DownloadPersonalDataViewModel>.SuccessWith(usersCommentsAndVotes);
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteAccount(string password)
        //{
        //    var user = await this.userManager.GetUserAsync(this.User);

        //    var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
        //                        (password != null &&
        //                        await this.userManager.CheckPasswordAsync(user, password));

        //    if (passwordValid == false)
        //    {
        //        return Result.Failure(NotificationMessages.InvalidPassword);
        //    }

        //    var result = await this.profileService.DeleteUserAsync(user);

        //    if (result == false)
        //    {
        //        this.Error(NotificationMessages.AccountDeleteError);

        //        return this.RedirectToAction(nameof(this.Index), new { userId = user.Id });
        //    }

        //    await this.signInManager.SignOutAsync();

        //    this.Success(NotificationMessages.AccountDeleted);

        //    return this.RedirectToAction("Index", "Home");
        //}

        [HttpGet]
        public async Task<ActionResult<UserChangePasswordInputModel>> ChangePassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (!hasPassword)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var inputModel = new UserChangePasswordInputModel
            {
                Email = user.Email,
            };

            return this.Ok(inputModel);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> ChangePassword(UserChangePasswordInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, inputModel.OldPassword, inputModel.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                inputModel.Email = user.Email;

                return Result.Failure(changePasswordResult.Errors.Select(e => e.Description).ToList());
            }

            await this.signInManager.RefreshSignInAsync(user);

            return Result.Success();
        }

        [HttpGet]
        public async Task<ActionResult<UserSetPasswordInputModel>> SetPassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var inputModel = new UserSetPasswordInputModel
            {
                Email = user.Email,
            };

            return this.Ok(inputModel);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> SetPassword(UserSetPasswordInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var addPasswordResult = await this.userManager.AddPasswordAsync(user, inputModel.NewPassword);

            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                inputModel.Email = user.Email;

                return Result.Failure(addPasswordResult.Errors.Select(e => e.Description).ToList());
            }

            await this.signInManager.RefreshSignInAsync(user);

            return Result.Success();
        }

        [HttpGet]
        public async Task<ActionResult<UserEditInputModel>> Edit()
        {
            var userId = this.User.GetUserId();

            var inputModel = await this.profileService.GetDetailsForUserEditAsync(userId);

            return this.Ok(inputModel);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> Edit(UserEditInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                inputModel.Email = user.Email;
                inputModel.UserName = user.UserName;

                return this.Ok(inputModel);
            }

            var result = await this.profileService.EditUserDetailsAsync(user, inputModel);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CannotUpdateProfileDetails);
            }

            return Result.Success();
        }

        [HttpGet]
        public async Task<ActionResult<AllUsersListViewModel>> All(int page = 0)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * UsersPerPage;

            var usersViewModel = await this.profileService.GetAllUsersExceptAdminsAsync(page, UsersPerPage, skip);

            return this.Ok(usersViewModel);
        }
    }
}
