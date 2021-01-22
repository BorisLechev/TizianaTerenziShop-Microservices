namespace TizianaTerenzi.Web.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Messaging;
    using TizianaTerenzi.Web.ViewModels.Contacts;

    public class ContactsController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactsController(
            IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactMessageInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            //https://nikolay.it/Blog/2018/03/Generate-PDF-file-from-Razor-view-using-ASP-NET-Core-and-PhantomJS/37
            var html = new StringBuilder();
            html.AppendLine($"<h1>{inputModel.Name}</h1>");
            html.AppendLine($"<h3>{inputModel.Email}</h3>");
            html.AppendLine($"<h3>{inputModel.Subject}</h3>");
            html.AppendLine($"<p>{inputModel.Content}</p>");
            await this.emailSender.SendEmailAsync(inputModel.Email, inputModel.Name, GlobalConstants.SystemEmail, inputModel.Subject, html.ToString());

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
