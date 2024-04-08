namespace TizianaTerenzi.Identity.Web.Gateway.Services.Products
{
    using Refit;
    using TizianaTerenzi.Identity.Web.Gateway.Models;

    public interface IProductsService
    {
        [Get("/Wishlist/GetAllProductsFromUsersWishlistPersonalData")]
        Task<IEnumerable<ProductsFromUsersWishlistPersonalDataResponseModel>> GetAllProductsFromUsersWishlistPersonalData();

        [Get("/Votes/GetAllUsersProductVotesPersonalData")]
        Task<IEnumerable<UsersProductVotesPersonalDataResponseModel>> GetAllUsersProductVotesPersonalData();

        [Get("/Comments/GetAllUsersCommentsAndVotesPersonalData")]
        Task<IEnumerable<UsersCommentsPersonalDataResponseModel>> GetAllUsersCommentsAndVotesPersonalData();
    }
}
