namespace TizianaTerenzi.Administration.Services.Data.Contacts
{
    using TizianaTerenzi.Administration.Web.Models.Contacts;

    public interface IContactsService
    {
        Task<IEnumerable<ContactMessagesViewModel>> GetAllAsync();

        Task<bool> CreateAsync(ContactMessageInputModel inputModel);

        Task<bool> DeleteAsync(int id);
    }
}
