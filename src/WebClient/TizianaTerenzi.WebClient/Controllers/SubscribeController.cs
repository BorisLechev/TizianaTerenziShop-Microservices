namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.ViewModels.Subscribe;

    public class SubscribeController : BaseController
    {
        //private readonly ISubscribeService subscribeService;

        //public SubscribeController(
        //    ISubscribeService subscribeService)
        //{
        //    this.subscribeService = subscribeService;
        //}

        [HttpPost]
        public async Task<IActionResult> Index(SubscribeInputModel inputModel)
        {
            //var result = await this.subscribeService.SubscribeForNewsletterAsync(inputModel.Email);

            //if (result == true)
            //{
            //    this.Success(NotificationMessages.SubsribedSuccessfully);
            //}
            //else
            //{
            //    this.Error(NotificationMessages.SubscriberEmailExists);
            //}

            return this.RedirectToAction("Index", "Home");
        }
    }
}
