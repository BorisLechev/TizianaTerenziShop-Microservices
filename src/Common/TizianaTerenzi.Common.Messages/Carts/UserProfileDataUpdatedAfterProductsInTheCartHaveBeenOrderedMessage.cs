namespace TizianaTerenzi.Common.Messages.Carts
{
    public class UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedMessage
    {
        public string UserId { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string ShippingAddress { get; set; }
    }
}
