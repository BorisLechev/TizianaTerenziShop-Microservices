namespace TizianaTerenzi.Carts.Web.Gateway.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductsInTheCartCheckoutViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Town { get; set; }

        public int? CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public IEnumerable<ProductsInTheCartViewModel> Products { get; set; }

        public int? DiscountCodeId { get; set; }

        public byte? DiscountCodeDiscount { get; set; }

        public int BulgariaId { get; set; }

        public string ShippingFeeRow { get; set; }
    }
}
