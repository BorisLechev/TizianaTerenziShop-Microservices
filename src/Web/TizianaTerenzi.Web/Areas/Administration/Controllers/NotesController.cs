namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Web.Areas.Administration.Models.Notes;

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

                return this.LocalRedirect("/home/index");
            }

            this.Success(NotificationMessages.CreateNoteSuccessfully);

            return this.LocalRedirect("/home/index");
        }
    }
}
