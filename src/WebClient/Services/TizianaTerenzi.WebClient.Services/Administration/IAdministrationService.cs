namespace TizianaTerenzi.WebClient.Services.Administration
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Dashboard;
    using TizianaTerenzi.WebClient.ViewModels.DiscountCodes;
    using TizianaTerenzi.WebClient.ViewModels.GeneralDiscounts;
    using TizianaTerenzi.WebClient.ViewModels.Notes;
    using TizianaTerenzi.WebClient.ViewModels.Products;

    public interface IAdministrationService
    {
        [Get("/Dashboard/Index")]
        Task<DashboardViewModel> GetDashboardInformationAsync();

        [Multipart]
        [Post("/Products/Create")]
        Task<Result> CreateProductAsync([Query] CreateProductInputModel inputModel, [AliasAs("picture")] StreamPart picture);

        [Multipart]
        [Put("/Products/Edit")]
        Task<Result> EditProductAsync([Query] EditProductInputModel inputmodel, [AliasAs("picture")] StreamPart picture);

        [Delete("/Products/Delete")]
        Task<Result> DeleteProductAsync(int productId);

        [Post("/Notes/Create")]
        Task<Result> CreateNoteAsync(CreateNoteInputModel inputModel);

        [Get("/GeneralDiscounts/Index")]
        Task<GeneralDiscountViewModel> GetGeneralDiscountsAsync();

        [Post("/GeneralDiscounts/Apply")]
        Task<Result> ApplyGeneralDiscountToAllProductsAsync(GeneralDiscountInputModel inputModel);

        [Post("/GeneralDiscounts/Disable")]
        Task<Result> DisableGeneralDiscountToAllProductsAsync();

        [Get("/DiscountCodes/Index")]
        Task<IEnumerable<DiscountCodesListingViewModel>> GetAllDiscountCodesAsync();

        [Post("/DiscountCodes/Create")]
        Task<Result> CreateDiscountCodeAsync(CreateDiscountCodeInputModel discountCodeInputModel);

        [Delete("/DiscountCodes/Delete")]
        Task<Result> DeleteDiscountCodeAsync(int discountCodeId);

        [Post("/Orders/Process")]
        Task<Result> ProcessOrderAsync(int orderId);
    }
}
