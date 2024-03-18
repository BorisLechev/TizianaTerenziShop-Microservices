namespace TizianaTerenzi.Identity.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Chat;
    using TizianaTerenzi.Identity.Services.Data.Profile;
    using TizianaTerenzi.Identity.Web.Models.Profile;

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProfileController : ApiController
    {
        private const int UsersPerPage = 6;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IProfileService profileService;

        private readonly IChatService chatsService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IProfileService profileService,
            IChatService chatsService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.profileService = profileService;
            this.chatsService = chatsService;
        }

        [HttpGet]
        public async Task<Result<ProfileViewModel>> Index(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Result<ProfileViewModel>.Failure(NotificationMessages.UserNotFound);
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

            var currentUserId = this.User.GetUserId();
            var currentUserName = this.User.GetUserName();

            if (user.Id != currentUserId)
            {
                var chatGroupId = await this.chatsService.GetChatGroupByUserIdsAsync(user.Id, currentUserId);

                if (chatGroupId == null)
                {
                    chatGroupId = await this.chatsService.AddUserToGroupAsync(chatGroupId, user.UserName, currentUserName);
                }

                profileViewModel.GroupId = chatGroupId;
            }

            return Result<ProfileViewModel>.SuccessWith(profileViewModel);
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

        // TODO: Delete user's Data in Notifications tables.
        [HttpDelete]
        public async Task<ActionResult<Result>> DeleteAccount(string password)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                                (password != null &&
                                await this.userManager.CheckPasswordAsync(user, password));

            if (passwordValid == false)
            {
                return Result.Failure(NotificationMessages.InvalidPassword);
            }

            var result = await this.profileService.DeleteUserAsync(user);

            if (result == false)
            {
                return Result.Failure(NotificationMessages.AccountDeleteError);
            }

            return Result.Success();
        }

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
        public async Task<ActionResult<EditUserWithDropdownsResponseModel>> Edit()
        {
            var userId = this.User.GetUserId();

            var inputModel = await this.profileService.GetDetailsForUserEditAsync(userId);

            return this.Ok(inputModel);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> Edit([FromQuery] UserEditInputModel inputModel, IFormFile avatarPicture)
        {
            byte[]? avatarPictureAsByteArray = null;

            if (avatarPicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await avatarPicture.CopyToAsync(memoryStream);

                    avatarPictureAsByteArray = memoryStream.ToArray();
                }

                avatarPictureAsByteArray = avatarPictureAsByteArray.Length == 0 ? null : avatarPictureAsByteArray;
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var result = await this.profileService.EditUserDetailsAsync(user, inputModel);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CannotUpdateProfileDetails);
            }

            return Result.Success(NotificationMessages.ProfileDetailsUpdated);
        }

        [HttpGet]
        public async Task<ActionResult<AllUsersListViewModel>> All(int page = 0)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * UsersPerPage;

            var usersViewModel = await this.profileService.GetAllUsersExceptAdminsAsync(page, UsersPerPage, skip);

            return this.Ok(usersViewModel);
        }

        [HttpGet]
        public async Task<Result<ProfileResponseModel>> GetProfileData(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Result<ProfileResponseModel>.Failure("User not found.");
            }

            var profileViewModel = new ProfileResponseModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PostalCode = user.PostalCode,
                Town = user.Town,
                Phone = user.PhoneNumber,
                Country = user.Country?.Name,
            };

            return Result<ProfileResponseModel>.SuccessWith(profileViewModel);
        }
    }
}
