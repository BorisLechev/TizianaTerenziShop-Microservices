namespace TizianaTerenzi.Identity.Services.Data.Identity
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Web.Models.Identity;

    public interface IIdentityService
    {
        Task<Result<ApplicationUser>> Register(RegisterUserInputModel userInput);

        Task<Result<UserResponseModel>> Login(LoginUserInputModel userInput);
    }
}
