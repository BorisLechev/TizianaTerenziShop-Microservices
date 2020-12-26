namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class Country : BaseModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } // many-to-one
    }
}
