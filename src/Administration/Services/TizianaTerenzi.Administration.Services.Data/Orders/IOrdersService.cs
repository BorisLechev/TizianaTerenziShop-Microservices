namespace TizianaTerenzi.Administration.Services.Data.Orders
{
    using TizianaTerenzi.Common.Messages.Orders;

    public interface IOrdersService
    {
        Task<bool> AddOrderStatisticsAsync(OrderAddedInAdminStatisticsMessage model);
    }
}
