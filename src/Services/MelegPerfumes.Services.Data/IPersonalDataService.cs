namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Web.ViewModels.Orders;

    public interface IPersonalDataService
    {
        Task<string> GetPersonalDataForUserJsonAsync(string userId);

        Task<bool> DeleteUserAsync(string userId);

        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUser(string userName);
    }
}
