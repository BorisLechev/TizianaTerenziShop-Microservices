namespace TizianaTerenzi.Products.Web.Services
{
    using Grpc.Core;
    using gRPCWishlistServer;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Products.Services.Data.Wishlist;
    using TizianaTerenzi.Products.Web.Models.Wishlist;

    public class WishlistGrpcService : Wishlist.WishlistBase
    {
        private readonly IWishlistService wishlistService;
        private readonly ICurrentUserService currentUserService;

        public WishlistGrpcService(IWishlistService wishlistService, ICurrentUserService currentUserService)
        {
            this.wishlistService = wishlistService;
            this.currentUserService = currentUserService;
        }

        public override async Task<WishlistResponse> GetAllProductsFromUsersWishlistAsync(WishlistRequest request, ServerCallContext context)
        {
            var userId = this.currentUserService.UserId;
            var wishlistViewModel = await this.wishlistService.GetAllProductsFromUsersWishlistAsync<ProductsFromUsersWishlistPersonalDataResponseModel>(request.UserId);

            var result = new WishlistResponse();

            result.Items.AddRange(wishlistViewModel.Select(w => new WishlistItemResponse { ProductName = w.ProductName, }));

            return result;
            //return wishlistViewModel.Select(w => new WishlistResponse { ProductName = w.ProductName,  });
        }
    }
}
