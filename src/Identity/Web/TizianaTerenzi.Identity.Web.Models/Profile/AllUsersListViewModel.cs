namespace TizianaTerenzi.Identity.Web.Models.Profile
{
    using System.Collections.Generic;

    using TizianaTerenzi.Common.Web.WebModels;

    public class AllUsersListViewModel : BasePagingBaseModel
    {
        public IEnumerable<UserInListViewModel> Users { get; set; }
    }
}
