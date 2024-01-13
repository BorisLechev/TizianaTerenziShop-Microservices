namespace TizianaTerenzi.Orders.Services.Data.Orders
{
    using System.Threading.Tasks;

    public interface IOrderStatusesService
    {
        Task<int> FindByNameAsync(string orderStatusName);
    }
}
