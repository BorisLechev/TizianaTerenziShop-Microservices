namespace TizianaTerenzi.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Web.ViewModels.Subscribe;

    public class SubscribeController : BaseController
    {
        private readonly ISubscribeService subscribeService;

        public SubscribeController(
            ISubscribeService subscribeService)
        {
            this.subscribeService = subscribeService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(SubscribeInputModel inputModel)
        {
            if ((await this.subscribeService.GetAllEmailsAsync()).Any(s => s.Email == inputModel.Email))
            {
                this.Error(NotificationMessages.SubscriberEmailExists);

                return this.RedirectToAction("Index", "Home");
            }

            var subscriber = new Subscriber
            {
                Email = inputModel.Email,
            };

            var result = await this.subscribeService.SubscribeForNewsletterAsync(subscriber);

            if (result == true)
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
