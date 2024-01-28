namespace TizianaTerenzi.Administration.Services.Data.Products
{
    using TizianaTerenzi.Administration.Web.Models.Products;

    public interface IProductsService
    {
        Task CreateProductAsync(CreateProductInputModel inputModel, byte[] picture);
    }
}
