namespace TizianaTerenzi.Orders.Web.Models.Orders
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Orders.Data.Models;

    public class PersonalDataOrdersViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public virtual ICollection<PersonalDataOrderProductsViewModel> Products { get; set; }
    }
}
