namespace TizianaTerenzi.Carts.Web.Gateway.Services.Carts
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Refit;
    using TizianaTerenzi.Carts.Web.Gateway.Models;

    public interface ICartsService
    {
        [Get("/Carts/Index")]
        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCart();

        [Get("/Countries/GetAll")]
        Task<IEnumerable<SelectListItem>> GetAllCountries();
    }
}
