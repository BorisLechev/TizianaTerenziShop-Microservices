namespace TizianaTerenzi.Identity.Web.Gateway.Controllers
{
    using System.Text;
    using System.Text.Json;
    using Grpc.Net.Client;
    using gRPCProfileServer;
    using gRPCWishlistServer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Identity.Web.Gateway.Models;
    using TizianaTerenzi.Identity.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Identity.Web.Gateway.Services.Orders;
    using TizianaTerenzi.Identity.Web.Gateway.Services.Products;

    public class ProfileController : ApiController
    {
        private readonly IProductsService productsService;

        private readonly IIdentityService identityService;

        private readonly IOrdersService ordersService;

        public ProfileController(
            IProductsService productsService,
            IIdentityService identityService,
            IOrdersService ordersService)
        {
            this.productsService = productsService;
            this.identityService = identityService;
            this.ordersService = ordersService;
        }

        // TODO: Use gRPC
        [Authorize]
        [HttpPost]
        public async Task<Result<DownloadPersonalDataFileResponseModel>> DownloadPersonalData(string password)
        {
            // The port number must match the port of the gRPC server.
            var channelProfile = GrpcChannel.ForAddress("https://localhost:5003");
            var clientProfile = new Profile.ProfileClient(channelProfile);

            var profileRequest = new PersonalDataForExportRequest { Password = password, UserId = this.User.GetUserId() };
            var usersPersonalData = await clientProfile.GetUsersPersonalDataForExportAsync(profileRequest);
            var usersPersonalData2 = await this.identityService.GetUsersPersonalDataForExport(password);

            if (!usersPersonalData.Succeeded)
            {
                return Result<DownloadPersonalDataFileResponseModel>.Failure(NotificationMessages.InvalidPassword);
            }

            // The port number must match the port of the gRPC server.
            var channelFavoriteProducts = GrpcChannel.ForAddress("https://localhost:5005");
            var clientFavoriteProducts = new Wishlist.WishlistClient(channelFavoriteProducts);

            var favoriteProducts = await clientFavoriteProducts.GetAllProductsFromUsersWishlistAsyncAsync(new WishlistRequest { UserId = this.User.GetUserId() });
            var favoriteProducts2 = await this.productsService.GetAllProductsFromUsersWishlistPersonalData();
            var productVotes = await this.productsService.GetAllUsersProductVotesPersonalData();
            var commentsAndCommentVotes = await this.productsService.GetAllUsersCommentsAndVotesPersonalData();
            var ordersAndProducts = await this.ordersService.GetAllUsersOrdersAndProductsPersonalDataAsync();

            // TODO: Add Orders and ChatUserGroups
            var personalData = new DownloadPersonalDataResponseModel
            {
                FirstName = usersPersonalData.Data.FirstName,
                LastName = usersPersonalData.Data.LastName,
                Email = usersPersonalData.Data.Email,
                CreatedOn = DateTime.Parse(usersPersonalData.Data.CreatedOn),
                Town = usersPersonalData.Data.Town,
                PostalCode = usersPersonalData.Data.PostalCode,
                CountryName = usersPersonalData.Data.CountryName,
                Address = usersPersonalData.Data.Address,
                //FavoriteProducts = favoriteProducts,
                ProductVotes = productVotes,
                Comments = commentsAndCommentVotes,
                Orders = ordersAndProducts,
                ChatUserGroups = usersPersonalData.Data.ChatUserGroups,
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize(personalData, options);

            var byteArray = Encoding.UTF8.GetBytes(json);

            var result = new DownloadPersonalDataFileResponseModel
            {
                UserId = this.User.GetUserId(),
                UserFirstName = personalData.FirstName,
                UserLastName = personalData.LastName,
                File = byteArray,
            };

            return Result<DownloadPersonalDataFileResponseModel>.SuccessWith(result);
        }
    }
}
