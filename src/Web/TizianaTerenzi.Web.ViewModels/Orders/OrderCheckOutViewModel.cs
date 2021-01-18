namespace TizianaTerenzi.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class OrderCheckOutViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Street, apartment, suite, etc.")]
        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        [Required]
        public int? CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        public IEnumerable<ProductsInTheCartViewModel> Products { get; set; }
    }
}
