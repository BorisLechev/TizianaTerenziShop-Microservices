namespace MelegPerfumes.Web.ViewModels.Orders
{
    using System;

    using AutoMapper;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class OrdersListingViewModel : IMapFrom<OrderProduct>, IHaveCustomMappings
    {
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => this.Quantity * this.Price;

        public string UserFullName { get; set; }

        public DiscountCode DiscountCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderProduct, OrdersListingViewModel>()
             .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
        }
    }
}
