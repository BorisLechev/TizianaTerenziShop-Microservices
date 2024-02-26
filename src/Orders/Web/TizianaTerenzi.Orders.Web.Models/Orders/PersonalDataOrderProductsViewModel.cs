namespace TizianaTerenzi.Orders.Web.Models.Orders
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Orders.Data.Models;

    public class PersonalDataOrderProductsViewModel : IMapFrom<OrderProduct>
    {
        public DateTime CreatedOn { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
