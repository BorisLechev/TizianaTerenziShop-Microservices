namespace TizianaTerenzi.WebClient.Services.Products
{
    using Refit;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Comments;
    using TizianaTerenzi.WebClient.ViewModels.Products;
    using TizianaTerenzi.WebClient.ViewModels.Votes;

    public interface IProductsService
    {
        [Get("/Products/All")]
        Task<ProductsListViewModel> All([Query] string search, ProductSorting sorting, int page = 1);

        [Get("/Products/Details")]
        Task<Result<ProductDetailsViewModel>> Details(int id);

        [Post("/Comments/Create")]
        Task<Result> CreateProductComment(CreateCommentInputModel inputModel);

        [Post("/Votes/VoteForComment")]
        Task<Result<CommentVoteResponseModel>> VoteForComment(PostCommentVoteInputModel inputModel);

        [Post("/Votes/VoteForProduct")]
        Task<Result<ProductVoteResponseModel>> VoteForProduct(PostProductVoteInputModel inputModel);

        [Get("/Wishlist/Index")]
        Task<IEnumerable<WishlistViewModel>> GetAllProductsFromUsersWishlist();

        [Post("/Wishlist/Add")]
        Task<Result> AddProductToTheWishlist(int productId);

        [Delete("/Wishlist/Delete")]
        Task<Result> DeleteProductFromTheWishlist(int productId);

        [Post("/Products/AddProductInTheCart")]
        Task AddProductInTheCart(int productId);
    }
}
