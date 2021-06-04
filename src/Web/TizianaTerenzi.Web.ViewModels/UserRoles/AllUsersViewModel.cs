namespace TizianaTerenzi.Web.ViewModels.UserRoles
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public ICollection<ApplicationUserViewModel> ApplicationUsers = new HashSet<ApplicationUserViewModel>();
    }
}
