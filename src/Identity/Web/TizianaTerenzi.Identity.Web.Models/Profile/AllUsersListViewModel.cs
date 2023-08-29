namespace TizianaTerenzi.Identity.Web.Models.Profile
{
    using System.Collections.Generic;

    using TizianaTerenzi.Common.Web.WebModels;

    public class AllUsersListViewModel : BasePagingViewModel
    {
        public IEnumerable<UserInListViewModel> Users { get; set; }
    }
}
