namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Web.ViewModels.Notes;

    public class NotesController : AdministrationController
    {
        private readonly INotesService notesService;

        public NotesController(INotesService notesService)
        {
            this.notesService = notesService;
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

            var result = await this.notesService.CreateNoteAsync(inputModel.Name);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateNoteError);

                return this.RedirectToAction("Index", "Home");
            }

            this.Success(NotificationMessages.CreateNoteSuccessfully);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
