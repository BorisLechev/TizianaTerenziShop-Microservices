namespace TizianaTerenzi.Common.Messages.Carts
{
    public class NotificationsUpdatedWhenProductAddedInTheCartMessage
    {
        public string UserId { get; set; }

        public int NumberOfProductsInTheUsersCart { get; set; }
    }
}
