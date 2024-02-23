namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.ViewModels.Subscribe;

    public class SubscribeController : BaseController
    {
        private readonly IAdministrationService administrationService;

        public SubscribeController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(SubscribeInputModel inputModel)
        {
            var result = await this.administrationService.SubscribeForNewsletterAsync(inputModel);

            if (result)
            {
                this.Success(NotificationMessages.SubsribedSuccessfully);
            }
            else
            {
                this.Error(NotificationMessages.SubscriberEmailExists);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}
