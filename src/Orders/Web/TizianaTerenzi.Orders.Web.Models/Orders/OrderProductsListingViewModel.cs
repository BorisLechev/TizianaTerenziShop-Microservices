namespace TizianaTerenzi.Orders.Web.Models.Orders
{
    using AutoMapper;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Orders.Data.Models;

    public class OrderProductsListingViewModel : IMapFrom<OrderProduct>, IHaveCustomMappings
    {
        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => this.Quantity * this.Price;

        public byte? OrderDiscountCodeDiscount { get; set; }

        public string? OrderDiscountCodeName { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderProduct, OrderProductsListingViewModel>()
                .ForMember(dest => dest.OrderDiscountCodeDiscount, opt => opt.MapFrom(src => src.Order.CartDiscountCodeValue));
        }
    }
}
