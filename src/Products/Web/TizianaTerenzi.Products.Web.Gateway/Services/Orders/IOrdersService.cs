namespace TizianaTerenzi.Products.Web.Gateway.Services.Orders
{
    using Refit;
    using TizianaTerenzi.Products.Web.Gateway.Models;

    public interface IOrdersService
    {
        [Get("/Orders/GetAllUsersOrdersAndProductsPersonalData")]
        Task<IEnumerable<PersonalDataOrdersViewModel>> GetAllUsersOrdersAndProductsPersonalDataAsync();
    }
}
