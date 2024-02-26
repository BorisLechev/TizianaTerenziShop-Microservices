namespace TizianaTerenzi.Products.Web.Gateway.Controllers
{
    using System.Text;
    using System.Text.Json;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Web.Gateway.Models;
    using TizianaTerenzi.Products.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Products.Web.Gateway.Services.Orders;
    using TizianaTerenzi.Products.Web.Gateway.Services.Products;

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
            var usersPersonalData = await this.identityService.GetUsersPersonalDataForExport(password);

            if (!usersPersonalData.Succeeded)
            {
                return Result<DownloadPersonalDataFileResponseModel>.Failure(NotificationMessages.InvalidPassword);
            }

            var favoriteProducts = await this.productsService.GetAllProductsFromUsersWishlistPersonalData();
            var productVotes = await this.productsService.GetAllUsersProductVotesPersonalData();
            var commentsAndCommentVotes = await this.productsService.GetAllUsersCommentsAndVotesPersonalData();
            var ordersAndProducts = await this.ordersService.GetAllUsersOrdersAndProductsPersonalDataAsync();

            // TODO: Add Orders and ChatUserGroups
            var personalData = new DownloadPersonalDataResponseModel
            {
                FirstName = usersPersonalData.Data.FirstName,
                LastName = usersPersonalData.Data.LastName,
                Email = usersPersonalData.Data.Email,
                CreatedOn = usersPersonalData.Data.CreatedOn,
                Town = usersPersonalData.Data.Town,
                PostalCode = usersPersonalData.Data.PostalCode,
                CountryName = usersPersonalData.Data.CountryName,
                Address = usersPersonalData.Data.Address,
                FavoriteProducts = favoriteProducts,
                ProductVotes = productVotes,
                Comments = commentsAndCommentVotes,
                Orders = ordersAndProducts,
                //ChatUserGroups = ,
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

        // TODO: Delete user's Data in Orders and Notifications tables.
        [Authorize]
        [HttpDelete]
        public async Task<Result> DeleteAccount(string password)
        {
            var deleteUserAccount = await this.identityService.DeleteAccount(password);

            if (!deleteUserAccount.Succeeded)
            {
                return Result.Failure(deleteUserAccount.Errors);
            }

            var resultOfDeletingTheProductsInTheUsersWishlist = await this.productsService.DeleteAllProductsInTheUsersWishlist();
            var resultOfDeletingTheUsersComments = await this.productsService.DeleteAllUserComments();
            var resultOfDeletingTheUsersCommentVotes = await this.productsService.DeleteAllUserCommentVotes();
            //await this.ordersService.DeleteAllOrdersByUserIdAsync(user.Id);
            //await this.ordersService.DeleteAllOrderProductsByUserIdAsync(user.Id);
            //await this.notificationsService.DeleteAllNotificationsByUserIdAsync(user.Id, user.UserName);

            if (resultOfDeletingTheProductsInTheUsersWishlist &&
                resultOfDeletingTheUsersComments &&
                resultOfDeletingTheUsersCommentVotes)
            {
                return Result.Success();
            }

            return Result.Failure(NotificationMessages.SomethingWentWrong);
        }
    }
}
