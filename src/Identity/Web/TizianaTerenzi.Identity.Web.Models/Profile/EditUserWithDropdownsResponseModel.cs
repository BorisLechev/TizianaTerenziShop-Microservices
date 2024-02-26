namespace TizianaTerenzi.Identity.Web.Models.Profile
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;

    public class EditUserWithDropdownsResponseModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Town { get; set; }

        public int? CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
