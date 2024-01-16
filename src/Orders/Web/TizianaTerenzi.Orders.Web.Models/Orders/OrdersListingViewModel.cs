namespace TizianaTerenzi.Orders.Web.Models.Orders
{
    using AutoMapper;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Orders.Data.Models;

    public class OrdersListingViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public OrdersListingViewModel()
        {
            this.Products = new List<OrderProductsListingViewModel>();
        }

        public int Id { get; set; }

        public string UserFullName { get; set; }

        public string DiscountCodeName { get; set; }

        public byte? DiscountCodeDiscount { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<OrderProductsListingViewModel> Products { get; set; }

        public decimal TotalPrice => this.Products.Sum(p => p.Price * p.Quantity);

        public string StatusName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderProductsListingViewModel, Order>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));

            configuration.CreateMap<Order, OrdersListingViewModel>()
                .ForMember(dest => dest.DiscountCodeName, opt => opt.MapFrom(src => src.CartDiscountCodeName))
                .ForMember(dest => dest.DiscountCodeDiscount, opt => opt.MapFrom(src => src.CartDiscountCodeValue));
        }
    }
}
