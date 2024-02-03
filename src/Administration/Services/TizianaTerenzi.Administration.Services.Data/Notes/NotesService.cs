namespace TizianaTerenzi.Administration.Services.Data.Notes
{
    using MassTransit;
    using TizianaTerenzi.Administration.Web.Models.Notes;
    using TizianaTerenzi.Common.Messages.Administration;

    public class NotesService : INotesService
    {
        private readonly IBus publisher;

        public NotesService(IBus publisher)
        {
            this.publisher = publisher;
        }

        public async Task CreateNoteAsync(CreateNoteInputModel inputModel)
        {
            await this.publisher.Publish(new NoteCreatedMessage
            {
                Name = inputModel.Name,
            });
        }
    }
}
