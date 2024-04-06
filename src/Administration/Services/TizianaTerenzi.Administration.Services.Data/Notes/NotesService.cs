namespace TizianaTerenzi.Administration.Services.Data.Notes
{
    using MassTransit;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.Notes;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;

    public class NotesService : INotesService
    {
        private readonly IBus publisher;
        private readonly IDeletableEntityRepository<OrderProductStatistics> orderProductStatisticsRepository;

        public NotesService(
            IBus publisher,
            IDeletableEntityRepository<OrderProductStatistics> orderProductStatisticsRepository)
        {
            this.publisher = publisher;
            this.orderProductStatisticsRepository = orderProductStatisticsRepository;
        }

        public async Task CreateNoteAsync(CreateNoteInputModel inputModel)
        {
            var messageData = new NoteCreatedMessage
            {
                Name = inputModel.Name,
            };

            var message = new EventMessageLog(messageData);

            await this.orderProductStatisticsRepository.CreateEventMessageLog(message);

            var result = await this.orderProductStatisticsRepository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.orderProductStatisticsRepository.MarkEventMessageLogAsPublished(message.Id);
        }
    }
}
