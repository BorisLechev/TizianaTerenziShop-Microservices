namespace TizianaTerenzi.WebClient.ViewModels.Users
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public ICollection<ApplicationUserViewModel> ApplicationUsers = new HashSet<ApplicationUserViewModel>();
    }
}
