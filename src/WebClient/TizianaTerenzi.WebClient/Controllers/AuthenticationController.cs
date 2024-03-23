namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using TizianaTerenzi.Common;

    public class AuthenticationController : BaseController
    {
        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(
            ILogger<AuthenticationController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("/logout")]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            //await this.signInManager.SignOutAsync();
            this.Response.Cookies.Delete(InfrastructureConstants.AuthenticationCookieName);
            this.logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                return this.LocalRedirect(returnUrl);
            }

            this.Success(NotificationMessages.LoggedOut);

            return this.LocalRedirect("/");
        }

        //[HttpPost]
        //public IActionResult GoogleLogin(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    var redirectUrl = this.Url.Action("GoogleRegister", "Authentication", new { returnUrl });
        //    var properties = this.signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        //    return new ChallengeResult("Google", properties);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GoogleRegister(string returnUrl = null, string remoteError = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    if (remoteError != null)
        //    {
        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var info = await this.signInManager.GetExternalLoginInfoAsync();

        //    if (info == null)
        //    {
        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var result = await this.signInManager.ExternalLoginSignInAsync("Google", info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

        //    if (result.Succeeded)
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    this.ViewData["ReturnUrl"] = returnUrl;

        //    var isEmailExisting = await this.usersRepository
        //            .AllAsNoTrackingWithDeleted()
        //            .AnyAsync(u => u.Email == info.Principal.FindFirstValue(ClaimTypes.Email));

        //    if (isEmailExisting)
        //    {
        //        this.Error(NotificationMessages.DuplicateEmail);

        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var inputModel = new GoogleLoginInputModel();

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        //    {
        //        inputModel.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    }

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
        //    {
        //        var name = info.Principal.FindFirstValue(ClaimTypes.Name);

        //        var split = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //        if (split.Length >= 2)
        //        {
        //            inputModel.FirstName = split[0];
        //            inputModel.LastName = split[split.Length - 1];
        //        }
        //        else
        //        {
        //            inputModel.FirstName = name;
        //        }
        //    }

        //    var location = await this.locationService.GetLocationAsync();
        //    var countryId = await this.countriesService.GetCountryIdByNameAsync(location.CountryName);

        //    // extension method
        //    var randomNumber = RandomExtensions.NextIntRange(this.random, 0, 10000);

        //    var username = $"{inputModel.Email.Split("@")[0].Trim()}_{randomNumber}";

        //    var user = new ApplicationUser
        //    {
        //        UserName = username,
        //        Email = inputModel.Email,
        //        FirstName = inputModel.FirstName,
        //        LastName = inputModel.LastName,
        //        CountryId = countryId,
        //        Town = location.Town,
        //    };

        //    var resultAfterCreate = await this.signInManager.UserManager.CreateAsync(user);

        //    if (resultAfterCreate.Succeeded)
        //    {
        //        resultAfterCreate = await this.signInManager.UserManager.AddLoginAsync(user, info);

        //        if (resultAfterCreate.Succeeded)
        //        {
        //            ApplicationRole role = await this.roleManager.FindByNameAsync(GlobalConstants.UserRoleName);

        //            await this.userManager.AddToRoleAsync(user, role.Name);

        //            this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));
        //            this.logger.LogInformation("User created with Google provider.");

        //            await this.signInManager.SignInAsync(user, isPersistent: true);
        //            return this.LocalRedirect(returnUrl);
        //        }
        //    }
        //    else
        //    {
        //        this.ModelState.AddModelError(string.Empty, NotificationMessages.DuplicateEmail);

        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    this.ViewData["ReturnUrl"] = returnUrl;
        //    return this.View();
        //}

        //[HttpPost]
        //public IActionResult FacebookLogin(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    var redirectUrl = this.Url.Action("FacebookRegister", "Authentication", new { returnUrl });
        //    var properties = this.signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
        //    return new ChallengeResult("Facebook", properties);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> FacebookRegister(FacebookLoginInputModel model, string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    var info = await this.signInManager.GetExternalLoginInfoAsync();

        //    if (info == null)
        //    {
        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var result = await this.signInManager.ExternalLoginSignInAsync("Facebook", info.ProviderKey,
        //        isPersistent: true, bypassTwoFactor: true);

        //    if (result.Succeeded)
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    this.ViewData["ReturnUrl"] = returnUrl;

        //    var isEmailExisting = await this.usersRepository
        //           .AllAsNoTrackingWithDeleted()
        //           .AnyAsync(u => u.Email == info.Principal.FindFirstValue(ClaimTypes.Email));

        //    if (isEmailExisting)
        //    {
        //        this.Error(NotificationMessages.DuplicateEmail);

        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var inputModel = new FacebookLoginInputModel();

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        //    {
        //        inputModel.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    }

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
        //    {
        //        var name = info.Principal.FindFirstValue(ClaimTypes.Name);

        //        var split = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //        if (split.Length >= 2)
        //        {
        //            inputModel.FirstName = split[0];
        //            inputModel.LastName = split[split.Length - 1];
        //        }
        //        else
        //        {
        //            inputModel.FirstName = name;
        //        }
        //    }

        //    var location = await this.locationService.GetLocationAsync();
        //    var countryId = await this.countriesService.GetCountryIdByNameAsync(location.CountryName);

        //    // extension method
        //    var randomNumber = RandomExtensions.NextIntRange(this.random, 0, 10000);

        //    var username = $"{inputModel.Email.Split("@")[0].Trim()}_{randomNumber}";

        //    var user = new ApplicationUser
        //    {
        //        UserName = username,
        //        Email = inputModel.Email,
        //        FirstName = inputModel.FirstName,
        //        LastName = inputModel.LastName,
        //        CountryId = countryId,
        //        Town = location.Town,
        //    };

        //    var resultAfterCreate = await this.signInManager.UserManager.CreateAsync(user);

        //    if (resultAfterCreate.Succeeded)
        //    {
        //        resultAfterCreate = await this.signInManager.UserManager.AddLoginAsync(user, info);

        //        if (resultAfterCreate.Succeeded)
        //        {
        //            ApplicationRole role = await this.roleManager.FindByNameAsync(GlobalConstants.UserRoleName);

        //            await this.userManager.AddToRoleAsync(user, role.Name);

        //            this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));
        //            this.logger.LogInformation("User created with Facebook provider.");

        //            await this.signInManager.SignInAsync(user, isPersistent: true);
        //            return this.LocalRedirect(returnUrl);
        //        }
        //    }

        //    this.ViewData["ReturnUrl"] = returnUrl;
        //    return this.View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GitHubLoginCallback(string returnUrl = null, string remoteError = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    if (remoteError != null)
        //    {
        //        // return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var info = await this.signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        // return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var result = await this.signInManager.ExternalLoginSignInAsync("GitHub", info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

        //    if (result.Succeeded)
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    this.ViewData["ReturnUrl"] = returnUrl;

        //    var model = new UserGitHubRegisterInputModel();

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        //    {
        //        model.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    }

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
        //    {
        //        var name = info.Principal.FindFirstValue(ClaimTypes.Name);

        //        var split = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //        if (split.Length >= 2)
        //        {
        //            model.FirstName = split[0];
        //            model.LastName = split[split.Length - 1];
        //        }
        //        else
        //        {
        //            model.FirstName = name;
        //        }
        //    }

        //    return this.View("GitHubRegister", model);
        //}

        //[HttpPost]
        //public IActionResult GitHubLogin(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    var redirectUrl = this.Url.Action("GitHubLoginCallback", "Authentication", new { returnUrl });
        //    var properties = this.signInManager.ConfigureExternalAuthenticationProperties("GitHub", redirectUrl);

        //    return new ChallengeResult("GitHub", properties);
        //}

        //[HttpPost]
        //[Route("/register/github")]
        //public async Task<IActionResult> GitHubRegister(UserGitHubRegisterInputModel inputModel, string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? this.Url.Content("~/");

        //    if (this.User.IsUserAuthenticated())
        //    {
        //        return this.LocalRedirect(returnUrl);
        //    }

        //    var info = await this.signInManager.GetExternalLoginInfoAsync();

        //    if (info == null)
        //    {
        //        // return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    var isEmailExisting = await this.usersRepository
        //           .AllAsNoTrackingWithDeleted()
        //           .AnyAsync(u => u.Email == inputModel.Email);

        //    if (isEmailExisting)
        //    {
        //        this.Error(NotificationMessages.DuplicateEmail);

        //        return this.RedirectToPage("/Account/Login", new { area = "Identity" });
        //    }

        //    if (this.ModelState.IsValid)
        //    {
        //        var location = await this.locationService.GetLocationAsync();
        //        var countryId = await this.countriesService.GetCountryIdByNameAsync(location.CountryName);

        //        // extension method
        //        var randomNumber = RandomExtensions.NextIntRange(this.random, 0, 10000);

        //        var username = $"{inputModel.Email.Split("@")[0].Trim()}_{randomNumber}";

        //        var user = new ApplicationUser()
        //        {
        //            UserName = username,
        //            Email = inputModel.Email,
        //            FirstName = inputModel.FirstName,
        //            LastName = inputModel.LastName,
        //            CountryId = countryId,
        //            Town = location.Town,
        //        };

        //        var result = await this.signInManager.UserManager.CreateAsync(user);

        //        if (result.Succeeded)
        //        {
        //            result = await this.signInManager.UserManager.AddLoginAsync(user, info);

        //            if (result.Succeeded)
        //            {
        //                ApplicationRole role = await this.roleManager.FindByNameAsync(GlobalConstants.UserRoleName);

        //                await this.userManager.AddToRoleAsync(user, role.Name);

        //                await this.signInManager.SignInAsync(user, isPersistent: true);

        //                this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));
        //                this.logger.LogInformation("User created with GitHub provider.");

        //                return this.LocalRedirect(returnUrl);
        //            }
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            this.ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    this.ViewData["ReturnUrl"] = returnUrl;

        //    return this.View();
        //}
    }
}
