namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.Subscribers;
    using TizianaTerenzi.Administration.Web.Models.Subscribers;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministratorAttribute]
    public class SubscribeController : ApiController
    {
        private readonly ISubscribeService subscribeService;

        public SubscribeController(ISubscribeService subscribeService)
        {
            this.subscribeService = subscribeService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Index(SubscribeInputModel inputModel)
        {
            var result = await this.subscribeService.SubscribeForNewsletterAsync(inputModel.Email);

            if (!result)
            {
                return Result.Failure(NotificationMessages.SubscriberEmailExists);
            }

            return Result.Success(NotificationMessages.SubsribedSuccessfully);
        }
    }
}
