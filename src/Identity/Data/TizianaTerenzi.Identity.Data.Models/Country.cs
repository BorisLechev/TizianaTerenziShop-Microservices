namespace TizianaTerenzi.Identity.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class Country : BaseModel<int>
    {
        public Country()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } // many-to-one
    }
}
