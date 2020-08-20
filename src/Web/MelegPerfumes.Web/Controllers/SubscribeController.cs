namespace MelegPerfumes.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Common;
    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Web.ViewModels.Subscribe;
    using Microsoft.AspNetCore.Mvc;

    public class SubscribeController : BaseController
    {
        private readonly IDeletableEntityRepository<Subscriber> subscribersRepository;

        private readonly ISubscribeService subscribeService;

        public SubscribeController(
            IDeletableEntityRepository<Subscriber> subscribersRepository,
            ISubscribeService subscribeService)
        {
            this.subscribersRepository = subscribersRepository;
            this.subscribeService = subscribeService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(SubscribeInputModel subscribeInputModel)
        {
            if ((await this.subscribeService.GetAllEmails()).Any(s => s.Email == subscribeInputModel.Email))
            {
                this.Error(NotificationMessages.SubscriberEmailExists);
            }

            var subscriber = new Subscriber
            {
                Email = subscribeInputModel.Email,
            };

            await this.subscribeService.SubscribeForNewsletterAsync(subscriber);
            this.Success(NotificationMessages.SubsribedSuccessfully);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
