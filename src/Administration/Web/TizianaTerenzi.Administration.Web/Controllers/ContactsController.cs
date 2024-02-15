namespace TizianaTerenzi.Administration.Web.Controllers
{
    using System.Text;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.Contacts;
    using TizianaTerenzi.Administration.Web.Models.Contacts;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Services.Messaging;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministrator]
    public class ContactsController : ApiController
    {
        private readonly IContactsService contactsService;
        private readonly IEmailSender emailSender;

        public ContactsController(
            IContactsService contactsService,
            IEmailSender emailSender)
        {
            this.contactsService = contactsService;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMessagesViewModel>>> Index()
        {
            var contactEntries = await this.contactsService.GetAllAsync();

            return this.Ok(contactEntries);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> Create(ContactMessageInputModel inputModel)
        {
            var result = await this.contactsService.CreateAsync(inputModel);

            // https://nikolay.it/Blog/2018/03/Generate-PDF-file-from-Razor-view-using-ASP-NET-Core-and-PhantomJS/37
            var html = new StringBuilder();
            html.AppendLine($"<h1>{inputModel.Name}</h1>");
            html.AppendLine($"<h3>{inputModel.Email}</h3>");
            html.AppendLine($"<h3>{inputModel.Subject}</h3>");
            html.AppendLine($"<p>{inputModel.Content}</p>");

            await this.emailSender.SendEmailAsync(inputModel.Email, inputModel.Name, GlobalConstants.SystemEmail, inputModel.Subject, html.ToString());

            return Result.Success(NotificationMessages.ContactFormSendMessageSuccessfully);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var result = await this.contactsService.DeleteAsync(id);

            if (result)
            {
                return Result.Success(NotificationMessages.SuccessfullyDeletedContactFormEntry);
            }

            return Result.Failure(NotificationMessages.DeleteContactFormEntryError);
        }
    }
}
