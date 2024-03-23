namespace TizianaTerenzi.WebClient.ViewModels.Products
{
    public class WishlistViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductPriceWithGeneralDiscount { get; set; }

        public int Discount { get; set; }

        public string ProductPicture { get; set; }

        public int ProductYearOfManufacture { get; set; }

        public string Url { get; set; }
    }
}
