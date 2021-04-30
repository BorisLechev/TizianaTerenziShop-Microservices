namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

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
