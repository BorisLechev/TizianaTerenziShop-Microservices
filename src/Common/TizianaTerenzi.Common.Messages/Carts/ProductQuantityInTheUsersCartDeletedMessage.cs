namespace TizianaTerenzi.Common.Messages.Carts
{
    public class ProductQuantityInTheUsersCartDeletedMessage
    {
        public string UserId { get; set; }

        public int Quantity { get; set; }
    }
}
