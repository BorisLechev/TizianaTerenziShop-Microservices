namespace TizianaTerenzi.Web.ViewModels.GeneralDiscounts
{
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class GeneralDiscountViewModel : IMapFrom<GeneralDiscount>, IHaveCustomMappings
    {
        public int PercentId { get; set; }

        public IEnumerable<SelectListItem> Percents { get; set; }

        public int IsActive { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<GeneralDiscount, GeneralDiscountViewModel>()
                .ForMember(dest => dest.PercentId, opt => opt.MapFrom(src => src.Percent));
        }
    }
}
