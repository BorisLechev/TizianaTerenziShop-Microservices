namespace TizianaTerenzi.WebClient.Services.Administration
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Dashboard;
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
    }
}
