namespace TizianaTerenzi.Administration.Services.Data.Notes
{
    using TizianaTerenzi.Administration.Web.Models.Notes;

    public interface INotesService
    {
        Task CreateNoteAsync(CreateNoteInputModel inputModel);
    }
}
