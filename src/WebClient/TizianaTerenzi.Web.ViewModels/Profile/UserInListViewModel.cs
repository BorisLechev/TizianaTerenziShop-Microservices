namespace TizianaTerenzi.Web.ViewModels.Profile
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class UserInListViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}
