namespace TizianaTerenzi.WebClient.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Web.ValidationAttributes;
    using TizianaTerenzi.WebClient.Services.Identity;
    using TizianaTerenzi.WebClient.ViewModels.Identity;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IIdentityService identityService;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(
            ILogger<LoginModel> logger,
            IIdentityService identityService)
        {
            this.identityService = identityService;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Email or Username field is required.")]
            [Display(Name = "Email or Username")]
            public string EmailOrUserName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            //[GoogleReCaptchaValidation]
            public string RecaptchaValue { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            returnUrl ??= this.Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {

                var loginInputModel = new LoginUserInputModel
                {
                    EmailOrUserName = this.Input.EmailOrUserName,
                    Password = this.Input.Password,
                    RememberMe = this.Input.RememberMe,
                };

                var result = await this.identityService.Login(loginInputModel);

                if (result.Succeeded && !string.IsNullOrWhiteSpace(result.Data.Token))
                {
                    this.Response.Cookies.Append(
                        InfrastructureConstants.AuthenticationCookieName,
                        result.Data.Token,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            MaxAge = TimeSpan.FromDays(7),
                        });

                    return this.LocalRedirect(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return this.Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
