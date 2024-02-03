namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.ViewModels.Notes;

    public class NotesController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public NotesController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var result = await this.administrationService.CreateNoteAsync(inputModel);

            if (!result.Succeeded)
            {
                this.Error(NotificationMessages.CreateNoteError);

                return this.RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }

            this.Success(NotificationMessages.CreateNoteSuccessfully);

            return this.RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
    }
}
