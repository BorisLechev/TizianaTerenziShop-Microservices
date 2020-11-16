namespace MelegPerfumes.Web.Areas.Administration.Models.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class AdminOrdersListingViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserFullName { get; set; }

        public DiscountCode DiscountCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalPrice => this.Products.Sum(p => p.Price * p.Quantity);

        public string StatusName { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, AdminOrdersListingViewModel>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
        }
    }
}
