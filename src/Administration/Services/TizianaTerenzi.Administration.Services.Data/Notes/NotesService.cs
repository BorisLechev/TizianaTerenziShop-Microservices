namespace TizianaTerenzi.Administration.Services.Data.Notes
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.Notes;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;

    public class NotesService : INotesService
    {
        private readonly IDeletableEntityRepository<OrderProductStatistics> orderProductStatisticsRepository;

        public NotesService(
            IDeletableEntityRepository<OrderProductStatistics> orderProductStatisticsRepository)
        {
            this.orderProductStatisticsRepository = orderProductStatisticsRepository;
        }

        public async Task CreateNoteAsync(CreateNoteInputModel inputModel)
        {
            var message = new NoteCreatedMessage
            {
                Name = inputModel.Name,
            };

            await this.orderProductStatisticsRepository.SaveAndPublishEventMessageAsync(message);
        }
    }
}
