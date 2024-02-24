namespace TizianaTerenzi.Common.Messages.Administration
{
    public class ProductInTheCartsEditedMessage
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
