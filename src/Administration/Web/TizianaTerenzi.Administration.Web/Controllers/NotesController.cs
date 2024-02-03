namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.Notes;
    using TizianaTerenzi.Administration.Web.Models.Notes;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministrator]
    public class NotesController : ApiController
    {
        private readonly INotesService notesService;

        public NotesController(INotesService notesService)
        {
            this.notesService = notesService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create(CreateNoteInputModel inputModel)
        {
            await this.notesService.CreateNoteAsync(inputModel);

            return Result.Success(NotificationMessages.CreateNoteSuccessfully);
        }
    }
}
