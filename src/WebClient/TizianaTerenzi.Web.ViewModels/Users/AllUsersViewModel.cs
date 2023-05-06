namespace TizianaTerenzi.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public ICollection<ApplicationUserViewModel> ApplicationUsers = new HashSet<ApplicationUserViewModel>();
    }
}
