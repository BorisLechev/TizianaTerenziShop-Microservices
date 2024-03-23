namespace TizianaTerenzi.WebClient.ViewModels.Products
{
    using System.Collections.Generic;

    using Ganss.Xss;
    using TizianaTerenzi.WebClient.ViewModels.Votes;

    public class ProductDetailsViewModel : ProductVoteResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public decimal Price { get; set; }

        public decimal PriceWithGeneralDiscount { get; set; }

        public string Picture { get; set; }

        public IEnumerable<string> Notes { get; set; }

        public string FragranceGroupName { get; set; }

        public int YearOfManufacture { get; set; }

        public double PercentFillStars => this.AverageVote * 20;

        public IEnumerable<ProductCommentViewModel> Comments { get; set; }

        public IEnumerable<RelatedProductsViewModel> RelatedProducts { get; set; }
    }
}
