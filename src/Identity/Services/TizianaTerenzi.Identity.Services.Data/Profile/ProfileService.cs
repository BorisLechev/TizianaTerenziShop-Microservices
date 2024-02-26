namespace TizianaTerenzi.Identity.Services.Data.Profile
{
    using MassTransit;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Countries;
    using TizianaTerenzi.Identity.Web.Models.Profile;

    public class ProfileService : IProfileService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        private readonly ICountriesService countriesService;

        //private readonly INotificationsService notificationsService;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IBus publisher;

        public ProfileService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository,
            ICountriesService countriesService,
            //INotificationsService notificationsService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IBus publisher)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.countriesService = countriesService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.publisher = publisher;
            //this.notificationsService = notificationsService;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            try
            {
                this.usersRepository.Delete(user);

                await this.usersRepository.SaveChangesAsync();

                await this.signInManager.SignOutAsync();

                await this.publisher.PublishBatch(new object[]
                {
                    new AllProductsInTheUsersWishlistDeletedMessage
                    {
                        UserId = user.Id,
                    },
                    new AllProductsInTheUsersCartDeletedMessage
                    {
                        UserId = user.Id,
                    },
                    new AllUserCommentsDeletedMessage
                    {
                        UserId = user.Id,
                    },
                    new AllUserCommentVotesDeletedMessage
                    {
                        UserId = user.Id,
                    },
                    //new AllUserOrdersDeletedMessage
                    //{
                    //    UserId = user.Id,
                    //},
                    //new AllUserOrderProductsDeletedMessage
                    //{
                    //    UserId = user.Id,
                    //},
                });
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EditUserDetailsAsync(ApplicationUser user, UserEditInputModel inputModel)
        {
            user.FirstName = inputModel.FirstName;
            user.LastName = inputModel.LastName;
            //user.UserName = inputModel.UserName;
            user.CountryId = inputModel.CountryId;
            user.Address = inputModel.Address;
            user.Town = inputModel.Town;
            user.PostalCode = inputModel.PostalCode;
            user.PhoneNumber = inputModel.PhoneNumber;

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<AllUsersListViewModel> GetAllUsersExceptAdminsAsync(int page, int take, int skip = 0)
        {
            var adminRole = await this.rolesRepository
                            .AllAsNoTracking()
                            .Select(r => new
                            {
                                r.Id,
                                r.Name,
                            })
                            .SingleOrDefaultAsync(r => r.Name == GlobalConstants.AdministratorRoleName);

            var users = await this.usersRepository
                            .AllAsNoTracking()
                            .Where(u => !u.Roles.Any(r => r.RoleId == adminRole.Id))
                            .OrderBy(u => u.UserName)
                            .Skip(skip)
                            .Take(take)
                            .To<UserInListViewModel>()
                            .ToListAsync();

            var usersCount = await this.usersRepository.AllAsNoTracking().CountAsync();

            var viewModel = new AllUsersListViewModel
            {
                Users = users,
                CurrentPage = page,
                ItemsCount = usersCount,
                ItemsPerPage = take,
            };

            return viewModel;
        }

        public async Task<EditUserWithDropdownsResponseModel> GetDetailsForUserEditAsync(string userId)
        {
            var viewModel = await this.usersRepository
                                .All()
                                .Where(u => u.Id == userId)
                                .To<EditUserWithDropdownsResponseModel>()
                                .SingleOrDefaultAsync();

            var countries = await this.countriesService.GetAllCountriesAsync();
            viewModel.Countries = countries;

            return viewModel;
        }

        public async Task<DownloadPersonalDataViewModel> GetPersonalDataForUserJsonAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            var personalData = await this.usersRepository
                                    .AllAsNoTracking()
                                    .Where(u => u.Id == userId)
                                    .To<DownloadPersonalDataViewModel>()
                                    .SingleOrDefaultAsync();

            return personalData;
        }

        public async Task<bool> SaveShippingDataAsync(UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedMessage model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var countryId = await this.countriesService.GetCountryIdByNameAsync(model.Country);

            user.CountryId = countryId;
            user.Address = model.ShippingAddress;
            user.Town = model.Town;
            user.PostalCode = model.PostalCode;
            user.PhoneNumber = model.PhoneNumber;

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
