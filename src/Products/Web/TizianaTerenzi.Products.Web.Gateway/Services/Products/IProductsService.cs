namespace TizianaTerenzi.Products.Web.Gateway.Services.Products
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Products.Web.Gateway.Models;

    public interface IProductsService
    {
        [Get("/Wishlist/GetAllProductsFromUsersWishlistPersonalData")]
        Task<IEnumerable<ProductsFromUsersWishlistPersonalDataResponseModel>> GetAllProductsFromUsersWishlistPersonalData();

        [Get("/Votes/GetAllUsersProductVotesPersonalData")]
        Task<IEnumerable<UsersProductVotesPersonalDataResponseModel>> GetAllUsersProductVotesPersonalData();

        [Get("/Comments/GetAllUsersCommentsAndVotesPersonalData")]
        Task<IEnumerable<UsersCommentsPersonalDataResponseModel>> GetAllUsersCommentsAndVotesPersonalData();

        [Delete("/Wishlist/DeleteAllProductsInTheUsersWishlist")]
        Task<Result> DeleteAllProductsInTheUsersWishlist();

        [Delete("/Comments/DeleteAllUserComments")]
        Task<Result> DeleteAllUserComments();

        [Delete("/Votes/DeleteAllUserCommentVotes")]
        Task<Result> DeleteAllUserCommentVotes();

        [Get("/Notes/Index")]
        Task<IEnumerable<SelectListItem>> GetAllNotesAsync();

        [Get("/ProductTypes/Index")]
        Task<IEnumerable<SelectListItem>> GetAllProductTypesAsync();

        [Get("/FragranceGroups/Index")]
        Task<IEnumerable<SelectListItem>> GetAllGetAllFragranceGroupsAsyncNotesAsync();
    }
}
