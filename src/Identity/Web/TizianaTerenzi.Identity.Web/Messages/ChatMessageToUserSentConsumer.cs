namespace TizianaTerenzi.Identity.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Notifications;
    using TizianaTerenzi.Identity.Services.Data.Chat;

    public class ChatMessageToUserSentConsumer : IConsumer<ChatMessageToUserSentMessage>
    {
        private readonly IChatService chatService;

        public ChatMessageToUserSentConsumer(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task Consume(ConsumeContext<ChatMessageToUserSentMessage> context)
        {
            var message = context.Message;

            await this.chatService.SendMessageToUserAsync(message);

            await Task.CompletedTask;
        }
    }
}
