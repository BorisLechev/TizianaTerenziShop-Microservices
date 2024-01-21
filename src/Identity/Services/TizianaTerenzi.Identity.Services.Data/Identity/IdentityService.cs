namespace TizianaTerenzi.Identity.Services.Data.Identity
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using MassTransit;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Countries;
    using TizianaTerenzi.Identity.Services.Location;
    using TizianaTerenzi.Identity.Web.Models.Identity;

    public class IdentityService : IIdentityService
    {
        private const string InvalidLoginAttemptErrorMessage = "Invalid login attempt.";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILocationService locationService;
        private readonly ICountriesService countriesService;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly ITokenGeneratorService tokenGeneratorService;
        private readonly ILogger<IdentityService> logger;
        private readonly IBus publisher;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILocationService locationService,
            ICountriesService countriesService,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            ITokenGeneratorService tokenGeneratorService,
            ILogger<IdentityService> logger,
            IBus publisher)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.locationService = locationService;
            this.countriesService = countriesService;
            this.usersRepository = usersRepository;
            this.tokenGeneratorService = tokenGeneratorService;
            this.logger = logger;
            this.publisher = publisher;
        }

        public async Task<Result<UserResponseModel>> Login(LoginUserInputModel userInput)
        {
            if (userInput.EmailOrUserName.IndexOf('@') > -1)
            {
                // Validate email format
                const string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);

                if (!re.IsMatch(userInput.EmailOrUserName))
                {
                    return InvalidLoginAttemptErrorMessage;
                }
            }

            var userName = userInput.EmailOrUserName;
            ApplicationUser? user = new ApplicationUser();

            if (userName.IndexOf('@') > -1)
            {
                user = await this.userManager.FindByEmailAsync(userName);

                if (user == null)
                {
                    return InvalidLoginAttemptErrorMessage;
                }
                else
                {
                    userName = user.UserName;
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await this.signInManager.PasswordSignInAsync(userName, userInput.Password, userInput.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                this.logger.LogInformation("User logged in.");

                var userRoles = await this.userManager.GetRolesAsync(user);

                var token = this.tokenGeneratorService.GenerateJwtBearerToken(user, userRoles);

                //var response = new UserResponseModel(token);
                //return Result<UserResponseModel>.SuccessWith(response);

                return new UserResponseModel(token);
            }
            else
            {
                return InvalidLoginAttemptErrorMessage;
            }
        }

        public async Task<Result<ApplicationUser>> Register(RegisterUserInputModel userInput)
        {
            var isUserNameExisting = await this.usersRepository
                                            .AllAsNoTrackingWithDeleted()
                                            .AnyAsync(u => u.UserName == userInput.UserName);

            if (isUserNameExisting == false)
            {
                var location = await this.locationService.GetLocationAsync();
                var countryId = await this.countriesService.GetCountryIdByNameAsync(location.CountryName);

                var user = new ApplicationUser
                {
                    UserName = userInput.UserName,
                    Email = userInput.Email,
                    FirstName = userInput.FirstName,
                    LastName = userInput.LastName,
                    CountryId = countryId,
                    Town = location.Town,
                };

                var result = await this.userManager.CreateAsync(user, userInput.Password);

                if (result.Succeeded)
                {
                    ApplicationRole? role = await this.roleManager.FindByNameAsync(GlobalConstants.UserRoleName);

                    await this.userManager.AddToRoleAsync(user, role.Name);

                    this.logger.LogInformation("User created a new account with password.");

                    //await this.signInManager.SignInAsync(user, isPersistent: false);

                    await this.publisher.Publish(new UserAddedInAdminStatisticsMessage
                    {
                        UserId = user.Id,
                        RoleName = role.Name,
                        IsBlocked = false,
                    });

                    return Result<ApplicationUser>.SuccessWith(user);
                }

                var errors = result.Errors.Select(e => e.Description);

                return Result<ApplicationUser>.Failure(errors);
            }
            else
            {
                return Result<ApplicationUser>.Failure(NotificationMessages.UsernameIsTaken);
            }
        }
    }
}
