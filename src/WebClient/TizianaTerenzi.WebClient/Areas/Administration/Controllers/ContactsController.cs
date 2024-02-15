namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;

    public class ContactsController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public ContactsController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            var emails = await this.administrationService.GetAllContactMessagesAsync();

            return this.View(emails);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.administrationService.DeleteContactMessageAsync(id);

            if (result.Succeeded)
            {
                this.Success(NotificationMessages.SuccessfullyDeletedContactFormEntry);
            }
            else
            {
                this.Error(NotificationMessages.DeleteContactFormEntryError);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
