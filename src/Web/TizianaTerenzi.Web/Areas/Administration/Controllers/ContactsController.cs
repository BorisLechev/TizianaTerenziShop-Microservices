namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Contacts;

    public class ContactsController : AdministrationController
    {
        private readonly IContactsService contactsService;

        public ContactsController(IContactsService contactsService)
        {
            this.contactsService = contactsService;
        }

        public async Task<IActionResult> Index()
        {
            var emails = await this.contactsService.GetAllAsync();

            return this.View(emails);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.contactsService.DeleteAsync(id);

            if (result)
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
