namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Services.Data.Notes;

    public class NoteCreatedConsumer : IConsumer<NoteCreatedMessage>
    {
        private readonly INotesService notesService;

        public NoteCreatedConsumer(INotesService notesService)
        {
            this.notesService = notesService;
        }

        public async Task Consume(ConsumeContext<NoteCreatedMessage> context)
        {
            var message = context.Message;

            await this.notesService.CreateNoteAsync(message.Name);

            await Task.CompletedTask;
        }
    }
}
