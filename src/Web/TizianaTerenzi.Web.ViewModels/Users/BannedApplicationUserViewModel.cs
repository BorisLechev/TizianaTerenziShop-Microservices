namespace TizianaTerenzi.Web.ViewModels.Users
{
    using System;

    using AutoMapper;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class BannedApplicationUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string Role { get; set; }

        public DateTime BannedOn { get; set; }

        public string Town { get; set; }

        public string CountryName { get; set; }

        public string Address { get; set; }

        public string ReasonToBeBlocked { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, BannedApplicationUserViewModel>()
               .ForMember(dest => dest.BannedOn, opt => opt.MapFrom(src => src.DeletedOn));
        }
    }
}
