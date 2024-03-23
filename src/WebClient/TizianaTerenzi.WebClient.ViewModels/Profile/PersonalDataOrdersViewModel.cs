namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System.Collections.Generic;

    public class PersonalDataOrdersViewModel
    {
        public int Id { get; set; }

        public virtual ICollection<PersonalDataOrderProductsViewModel> Products { get; set; }
    }
}
