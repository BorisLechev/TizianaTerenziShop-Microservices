namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.ViewModels.Contacts;

    public class ContactsController : BaseController
    {
        private readonly IAdministrationService administrationService;

        public ContactsController(
            IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactMessageInputModel inputModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(inputModel);
            }

            var result = await this.administrationService.SendContactMessageAsync(inputModel);

            this.Success(result.SucceessMessage);

            return this.RedirectToAction(nameof(this.ThankYou));
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
