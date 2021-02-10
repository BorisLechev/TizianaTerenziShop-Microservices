namespace TizianaTerenzi.Services.Data.PersonalData
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Services.Data.Wishlist;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Account;
    using TizianaTerenzi.Web.ViewModels.Profile;

    public class PersonalDataService : IPersonalDataService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly ICountriesService countriesService;

        private readonly IWishlistService wishlistService;

        private readonly IOrdersService ordersService;

        private readonly ICommentsService commentsService;

        private readonly ICommentVotesService commentVotesService;

        private readonly UserManager<ApplicationUser> userManager;

        public PersonalDataService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            ICountriesService countriesService,
            IWishlistService wishlistService,
            IOrdersService ordersService,
            ICommentsService commentsService,
            ICommentVotesService commentVotesService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.countriesService = countriesService;
            this.wishlistService = wishlistService;
            this.userManager = userManager;
            this.ordersService = ordersService;
            this.commentsService = commentsService;
            this.commentVotesService = commentVotesService;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (userId == null)
            {
                return false;
            }

            var user = await this.usersRepository
                .All()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            try
            {
                await this.ordersService.DeleteAllOrdersByUserIdAsync(userId);
                await this.ordersService.DeleteAllOrderProductsByUserIdAsync(userId);
                await this.commentsService.DeleteRangeByUserIdAsync(userId);
                await this.commentVotesService.DeleteRangeByUserIdAsync(userId);
                await this.wishlistService.DeleteAllProductsInTheWishlistAsync(userId);

                this.usersRepository.Delete(user);

                await this.usersRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task EditUserDetailsAsync(ApplicationUser user, UserEditInputModel inputModel)
        {
            user.FirstName = inputModel.FirstName;
            user.LastName = inputModel.LastName;
            user.UserName = inputModel.UserName;
            user.CountryId = inputModel.CountryId;
            user.Address = inputModel.Address;
            user.Town = inputModel.Town;
            user.PostalCode = inputModel.PostalCode;
            user.PhoneNumber = inputModel.PhoneNumber;

            await this.userManager.UpdateAsync(user);
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
            };

            var json = JsonConvert.SerializeObject(personalData, Formatting.Indented);

            return json;
        }
    }
}
