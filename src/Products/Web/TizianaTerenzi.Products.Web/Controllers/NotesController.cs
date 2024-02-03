namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Services.Data.Notes;

    public class NotesController : ApiController
    {
        private readonly INotesService notesService;

        public NotesController(INotesService notesService)
        {
            this.notesService = notesService;
        }

        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> Index()
        {
            var notes = await this.notesService.GetAllNotesAsync();

            return notes;
        }

        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> GetAllNotesWithSelectedByProductId(int productId)
        {
            var notes = await this.notesService.GetAllNotesWithSelectedByProductIdAsync(productId);

            return notes;
        }
    }
}
