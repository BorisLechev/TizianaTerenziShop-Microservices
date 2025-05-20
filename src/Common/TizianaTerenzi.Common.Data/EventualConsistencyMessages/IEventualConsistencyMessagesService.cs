namespace TizianaTerenzi.Common.Data.EventualConsistencyMessages
{
    public interface IEventualConsistencyMessagesService
    {
        Task<bool> IsDuplicated(object eventMessageData, string propertyFilter, object identifier);
    }
}
