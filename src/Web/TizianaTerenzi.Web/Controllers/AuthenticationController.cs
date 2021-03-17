namespace TizianaTerenzi.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Location;
    using TizianaTerenzi.Web.ViewModels.Authentication;

    public class AuthenticationController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<AuthenticationController> logger;

        private readonly ICartService cartService;

        private readonly ILocationService locationService;

        private readonly ICountriesService countriesService;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticationController> logger,
            ICartService cartService,
            ILocationService locationService,
            ICountriesService countriesService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.cartService = cartService;
            this.locationService = locationService;
            this.countriesService = countriesService;
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

            if (inputModel.EmailOrUserName.IndexOf('@') > -1)
            {
                // Validate email format
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);

                if (!re.IsMatch(inputModel.EmailOrUserName))
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            else
            {
                //validate Username format
                string emailRegex = @"^[a-zA-Z0-9]*$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(inputModel.EmailOrUserName))
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            if (this.ModelState.IsValid)
            {
                var userName = inputModel.EmailOrUserName;

                // This if the user would like to loging with username or email
                if (userName.IndexOf('@') > -1)
                {
                    var user = await this.userManager.FindByEmailAsync(inputModel.EmailOrUserName);

                    if (user == null)
                    {
                        this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        //return this.View(this.Input);
                    }
                    else
                    {
                        userName = user.UserName;
                    }
                }

                var result = await this.signInManager.PasswordSignInAsync(userName, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: true);

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
                (var country, var town, var ip) = await this.locationService.GetLocationAsync();
                var countryId = await this.countriesService.GetCountryIdByNameAsync(country);

                var user = new ApplicationUser
                {
                    UserName = inputModel.UserName,
                    Email = inputModel.Email,
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                    CountryId = countryId,
                    Town = town,
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

            await this.cartService.DeleteAllProductsInTheCartByUserId(this.userManager.GetUserId(this.User));

            if (returnUrl != null)
            {
                return this.LocalRedirect(returnUrl);
            }

            this.Success(NotificationMessages.LoggedOut);

            return this.LocalRedirect("/");
        }

        [HttpPost]
        public IActionResult GoogleLogin(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            var redirectUrl = this.Url.Action("GoogleRegister", "Authentication", new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            return new ChallengeResult("Google", properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleRegister(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            if (remoteError != null)
            {
                return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var result = await this.signInManager.ExternalLoginSignInAsync("Google", info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return this.LocalRedirect(returnUrl);
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            var inputModel = new GoogleLoginInputModel();

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                inputModel.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                var split = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (split.Length >= 2)
                {
                    inputModel.FirstName = split[0];
                    inputModel.LastName = split[split.Length - 1];
                }
                else
                {
                    inputModel.FirstName = name;
                }
            }

            (var country, var town, var ip) = await this.locationService.GetLocationAsync();
            var countryId = await this.countriesService.GetCountryIdByNameAsync(country);

            var user = new ApplicationUser
            {
                UserName = $"{inputModel.Email}_Google",
                Email = inputModel.Email,
                FirstName = inputModel.FirstName,
                LastName = inputModel.LastName,
                CountryId = countryId,
                Town = town,
            };

            var resultAfterCreate = await this.signInManager.UserManager.CreateAsync(user);

            if (resultAfterCreate.Succeeded)
            {
                resultAfterCreate = await this.signInManager.UserManager.AddLoginAsync(user, info);

                if (resultAfterCreate.Succeeded)
                {
                    this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));

                    await this.signInManager.SignInAsync(user, isPersistent: true);
                    return this.LocalRedirect(returnUrl);
                }
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        public IActionResult FacebookLogin(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            var redirectUrl = this.Url.Action("FacebookRegister", "Authentication", new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookRegister(FacebookLoginInputModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.LocalRedirect(returnUrl);
            }

            //if (remoteError != null)
            //{
            //    return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
            //}

            var info = await this.signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var result = await this.signInManager.ExternalLoginSignInAsync("Facebook", info.ProviderKey,
                isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return this.LocalRedirect(returnUrl);
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            var inputModel = new FacebookLoginInputModel();

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                inputModel.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                var split = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (split.Length >= 2)
                {
                    inputModel.FirstName = split[0];
                    inputModel.LastName = split[split.Length - 1];
                }
                else
                {
                    inputModel.FirstName = name;
                }
            }

            (var country, var town, var ip) = await this.locationService.GetLocationAsync();
            var countryId = await this.countriesService.GetCountryIdByNameAsync(country);

            var user = new ApplicationUser
            {
                UserName = $"{inputModel.Email}_Facebook",
                Email = inputModel.Email,
                FirstName = inputModel.FirstName,
                LastName = inputModel.LastName,
                CountryId = countryId,
                Town = town,
            };

            var resultAfterCreate = await this.signInManager.UserManager.CreateAsync(user);

            if (resultAfterCreate.Succeeded)
            {
                resultAfterCreate = await this.signInManager.UserManager.AddLoginAsync(user, info);

                if (resultAfterCreate.Succeeded)
                {
                    this.Success(string.Format(NotificationMessages.RegistrationWelcome, inputModel.FirstName));

                    await this.signInManager.SignInAsync(user, isPersistent: true);
                    return this.LocalRedirect(returnUrl);
                }
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
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
                // return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
                return this.RedirectToPage("Login");
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
                return this.RedirectToPage("Login");
            }

            var result = await this.signInManager.ExternalLoginSignInAsync("GitHub", info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

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
                // return this.RedirectToAction("Login", new { ReturnUrl = returnUrl });
                return this.RedirectToPage("Login");
            }

            if (this.ModelState.IsValid)
            {
                (var country, var town, var ip) = await this.locationService.GetLocationAsync();
                var countryId = await this.countriesService.GetCountryIdByNameAsync(country);

                var user = new ApplicationUser()
                {
                    UserName = $"{inputModel.Email}_GitHub",
                    Email = inputModel.Email,
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                    CountryId = countryId,
                    Town = town,
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
