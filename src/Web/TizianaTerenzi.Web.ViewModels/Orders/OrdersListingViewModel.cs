namespace TizianaTerenzi.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class OrdersListingViewModel : OrdersChartResponseModel, IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserFullName { get; set; }

        public string DiscountCodeName { get; set; }

        public double? DiscountCodeDiscount { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalPrice => this.Products.Sum(p => p.Price * p.Quantity);

        public string StatusName { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrdersListingViewModel>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
        }
    }
}
