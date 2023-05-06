namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Location;
    using TizianaTerenzi.Services.Messaging;
    using TizianaTerenzi.WebClient.ViewModels.Contacts;

    public class ContactsController : BaseController
    {
        private readonly IRepository<ContactFormEntry> contactsRepository;

        private readonly ILocationService locationService;

        private readonly IEmailSender emailSender;

        public ContactsController(
            IRepository<ContactFormEntry> contactsRepository,
            ILocationService locationService,
            IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.locationService = locationService;
            this.emailSender = emailSender;
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

            var ip = await this.locationService.GetIpAddressAsync();

            var contactFormEntry = new ContactFormEntry
            {
                Name = inputModel.Name,
                Email = inputModel.Email,
                Subject = inputModel.Subject,
                Content = inputModel.Content,
                Ip = ip,
            };

            await this.contactsRepository.AddAsync(contactFormEntry);
            await this.contactsRepository.SaveChangesAsync();

            // https://nikolay.it/Blog/2018/03/Generate-PDF-file-from-Razor-view-using-ASP-NET-Core-and-PhantomJS/37
            var html = new StringBuilder();
            html.AppendLine($"<h1>{inputModel.Name}</h1>");
            html.AppendLine($"<h3>{inputModel.Email}</h3>");
            html.AppendLine($"<h3>{inputModel.Subject}</h3>");
            html.AppendLine($"<p>{inputModel.Content}</p>");

            await this.emailSender.SendEmailAsync(inputModel.Email, inputModel.Name, GlobalConstants.SystemEmail, inputModel.Subject, html.ToString());

            this.Success(NotificationMessages.ContactFormSendMessageSuccessfully);

            return this.RedirectToAction(nameof(this.ThankYou));
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
