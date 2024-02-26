namespace TizianaTerenzi.Identity.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Identity.Services.Data.Identity;
    using TizianaTerenzi.Identity.Web.Models.Identity;

    public class IdentityController : ApiController
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost]
        public async Task<ActionResult<Result<UserResponseModel>>> Register(RegisterUserInputModel inputModel)
        {
            var result = await this.identityService.Register(inputModel);

            if (!result.Succeeded)
            {
                return Result<UserResponseModel>.Failure(result.Errors);
            }

            var loginUserInputModel = new LoginUserInputModel
            {
                EmailOrUserName = inputModel.Email,
                Password = inputModel.Password,
                RememberMe = true,
            };

            var loginResult = await this.Login(loginUserInputModel);

            return Result<UserResponseModel>.SuccessWith(loginResult.Value);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> Login(LoginUserInputModel inputModel)
        {
            var result = await this.identityService.Login(inputModel);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
                //return new UserResponseModel(null);
            }

            return new UserResponseModel(result.Data.Token);
        }
    }
}
