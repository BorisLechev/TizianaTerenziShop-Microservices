namespace TizianaTerenzi.Web.ViewModels.Profile
{
    using System.Collections.Generic;

    public class AllUsersListViewModel : PagingViewModel
    {
        public IEnumerable<UserInListViewModel> Users { get; set; }
    }
}
