namespace TizianaTerenzi.WebClient.ViewModels.Products
{
    public class ProductInListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithGeneralDiscount { get; set; }

        public int Discount => this.Price == this.PriceWithGeneralDiscount ? 0 : (int)((this.Price - this.PriceWithGeneralDiscount) / this.Price * 100);

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public double AverageVote { get; set; }

        public double PercentFillStars => this.AverageVote * 20;

        public string Url { get; set; }
    }
}
