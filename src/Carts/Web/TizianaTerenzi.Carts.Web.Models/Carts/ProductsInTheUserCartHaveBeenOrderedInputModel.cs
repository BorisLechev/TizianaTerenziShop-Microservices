namespace TizianaTerenzi.Carts.Web.Models.Carts
{
    public class ProductsInTheUserCartHaveBeenOrderedInputModel
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string ShippingAddress { get; set; }
    }
}
