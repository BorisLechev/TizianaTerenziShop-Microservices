namespace MelegPerfumes.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderProductInputModel
    {
        private const double MinPrice = 0.1;

        private const int MinQuantity = 1;

        private const string PriceErrorMessage = "Price should be a positive number.";

        private const string QuantityErrorMessage = "Quantity should be a positive number.";

        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(MinPrice, double.MaxValue, ErrorMessage = PriceErrorMessage)]
        public decimal Price { get; set; }

        [Range(MinQuantity, int.MaxValue, ErrorMessage = QuantityErrorMessage)]
        public int Quantity { get; set; }
    }
}
