namespace TizianaTerenzi.Identity.Web.Gateway.Services.Orders
{
    using Refit;
    using TizianaTerenzi.Identity.Web.Gateway.Models;

    public interface IOrdersService
    {
        [Get("/Orders/GetAllUsersOrdersAndProductsPersonalData")]
        Task<IEnumerable<PersonalDataOrdersViewModel>> GetAllUsersOrdersAndProductsPersonalDataAsync();
    }
}
