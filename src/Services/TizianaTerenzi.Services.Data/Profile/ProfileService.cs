namespace TizianaTerenzi.Services.Data.Profile
{
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Data.Notifications;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Services.Data.Wishlist;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    public class ProfileService : IProfileService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        private readonly ICountriesService countriesService;

        private readonly IWishlistService wishlistService;

        private readonly IOrdersService ordersService;

        private readonly ICommentsService commentsService;

        private readonly ICommentVotesService commentVotesService;

        private readonly INotificationsService notificationsService;

        private readonly UserManager<ApplicationUser> userManager;

        public ProfileService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository,
            ICountriesService countriesService,
            IWishlistService wishlistService,
            IOrdersService ordersService,
            ICommentsService commentsService,
            ICommentVotesService commentVotesService,
            INotificationsService notificationsService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.countriesService = countriesService;
            this.wishlistService = wishlistService;
            this.userManager = userManager;
            this.ordersService = ordersService;
            this.commentsService = commentsService;
            this.commentVotesService = commentVotesService;
            this.notificationsService = notificationsService;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            try
            {
                await this.ordersService.DeleteAllOrdersByUserIdAsync(user.Id);
                await this.ordersService.DeleteAllOrderProductsByUserIdAsync(user.Id);
                await this.commentsService.DeleteRangeByUserIdAsync(user.Id);
                await this.commentVotesService.DeleteRangeByUserIdAsync(user.Id);
                await this.wishlistService.DeleteAllProductsInTheWishlistAsync(user.Id);
                await this.notificationsService.DeleteAllNotificationsByUserIdAsync(user.Id, user.UserName);

                this.usersRepository.Delete(user);

                await this.usersRepository.SaveChangesAsync();
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
                .Where(u => u.Roles.Any(r => r.RoleId == adminRole.Id) == false)
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

        public async Task<UserEditInputModel> GetDetailsForUserEditAsync(string userId)
        {
            var viewModel = await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .To<UserEditInputModel>()
                .SingleOrDefaultAsync();

            var countries = await this.countriesService.GetAllCountriesAsync();
            viewModel.Countries = countries;

            return viewModel;
        }

        public async Task<string> GetPersonalDataForUserJsonAsync(string userId)
        {
            if (userId == null)
            {
                return null;
            }

            var user = await this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.Id == userId)
                .To<DownloadPersonalDataViewModel>()
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var personalData = new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.CreatedOn,
                user.CountryName,
                user.Town,
                user.PostalCode,
                user.Address,
                Orders = user.Orders.Select(o => new
                {
                    o.Id,
                    OrderProducts = o.Products.Select(op => new
                    {
                        op.CreatedOn,
                        op.ProductName,
                        op.Quantity,
                        op.ProductPrice,
                    })
                    .ToArray(),
                })
                .ToArray(),
                Votes = user.ProductVotes.Select(pv => new
                {
                    pv.CreatedOn,
                    pv.ProductName,
                    pv.Value,
                })
                .ToArray(),
                Comments = user.Comments.Select(c => new
                {
                    c.CreatedOn,
                    c.Content,
                    Votes = c.Votes.Select(v => new
                    {
                        v.CreatedOn,
                        v.Type,
                    })
                    .ToArray(),
                })
                .ToArray(),
                FavoriteProduct = user.FavoriteProducts.Select(fp => new
                {
                    fp.Id,
                    fp.CreatedOn,
                    fp.ProductName,
                })
                .ToArray(),
                ChatUserGroups = user.ChatUserGroups.Select(ug => new
                {
                    ug.ChatGroupName,
                    ug.ChatGroupCreatedOn,
                })
                .ToArray(),
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize(personalData, options);

            return json;
        }
    }
}
