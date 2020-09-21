namespace MelegPerfumes.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using MelegPerfumes.Common;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Web.ViewModels.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class AuthenticationController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticationController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [Route("/login")]
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            return this.View();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(UserLoginInputModel inputModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(inputModel.Email, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    return this.LocalRedirect(returnUrl);
                }

                this.ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return this.View();
        }

        [Route("/register")]
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            return this.View();
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(UserRegisterInputModel inputModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = inputModel.Email,
                    Email = inputModel.Email,
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                };

                var result = await this.userManager.CreateAsync(user, inputModel.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");
                    this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));

                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    return this.LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        [Route("/logout")]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                return this.LocalRedirect(returnUrl);
            }

            this.Success(NotificationMessages.LoggedOut);

            return this.RedirectToRoute("/");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GitHubLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            if (remoteError != null)
            {
                //return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
                return this.RedirectToPage("Login");
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
                return this.RedirectToPage("Login");
            }

            var result = await this.signInManager.ExternalLoginSignInAsync("GitHub", info.ProviderKey,
                isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return this.LocalRedirect(returnUrl);
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            var model = new UserGitHubRegisterInputModel();

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                model.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                var split = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (split.Length >= 2)
                {
                    model.FirstName = split[0];
                    model.LastName = split[split.Length - 1];
                }
                else
                {
                    model.FirstName = name;
                }
            }

            return this.View("GitHubRegister", model);
        }

        [HttpPost]
        public IActionResult GitHubLogin(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            var redirectUrl = this.Url.Action("GitHubLoginCallback", "Authentication", new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties("GitHub", redirectUrl);

            return new ChallengeResult("GitHub", properties);
        }

        [HttpPost]
        [Route("/register/github")]
        public async Task<IActionResult> GitHubRegister(UserGitHubRegisterInputModel inputModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                //return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
                return this.RedirectToPage("Login");
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = inputModel.Email,
                    Email = inputModel.Email,
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                };

                var result = await this.signInManager.UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await this.signInManager.UserManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: true);
                        this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));

                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            return this.View();
        }
    }
}
