namespace TizianaTerenzi.Administration.Web.Models.GeneralDiscounts
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Services.Mapping;

    public class GeneralDiscountViewModel : IMapFrom<GeneralDiscount>, IHaveCustomMappings
    {
        public byte PercentId { get; set; }

        public IEnumerable<SelectListItem> Percents { get; set; }

        public int IsActive { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<GeneralDiscount, GeneralDiscountViewModel>()
                .ForMember(dest => dest.PercentId, opt => opt.MapFrom(src => src.Percent));
        }
    }
}
