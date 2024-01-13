
namespace TizianaTerenzi.Orders.Services.Data.Orders
{
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Orders.Data.Models;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IOrderStatusesService orderStatusesService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IOrderStatusesService orderStatusesService)
        {
            this.ordersRepository = ordersRepository;
            this.orderStatusesService = orderStatusesService;
        }

        public async Task<bool> OrderAsync(ProductsInTheUserCartHaveBeenOrderedMessage model)
        {
            var orderProducts = model.Products
                .Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.PriceWithDiscountCode,
                    Quantity = p.Quantity,
                    CreatedOn = DateTime.UtcNow,
                })
                .ToList();

            var discountCodeId = model.Products.FirstOrDefault()?.DiscountCodeId;
            var discountCodeDiscount = model.Products.FirstOrDefault()?.DiscountCodeDiscount;

            var pendingStatusId = await this.orderStatusesService.FindByNameAsync("Pending");

            var order = new Order
            {
                UserId = model.UserId,
                UserFullName = model.FullName,
                UserEmail = model.Email,
                UserPhoneNumber = model.PhoneNumber,
                UserShippingAddress = model.ShippingAddress,
                UserTown = model.Town,
                UserPostalCode = model.PostalCode,
                UserCountry = model.Country,
                StatusId = pendingStatusId,
                Products = orderProducts,
                DiscountCodeId = discountCodeId,
                CartDiscountCodeValue = discountCodeDiscount,
            };

            await this.ordersRepository.AddAsync(order);
            var result = await this.ordersRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
