namespace TizianaTerenzi.Services.Data.UsersInformation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.UsersInformation;

    public interface IUsersInformationService
    {
        Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync();
    }
}
