namespace TizianaTerenzi.Identity.Web.Gateway.Models
{
    public class PersonalDataOrdersViewModel
    {
        public int Id { get; set; }

        public ICollection<PersonalDataOrderProductsViewModel> Products { get; set; }
    }
}
