namespace TizianaTerenzi.Services.Data.Contacts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Contacts;

    public interface IContactsService
    {
        public Task<IEnumerable<ContactMessagesViewModel>> GetAllAsync();

        public Task<bool> DeleteAsync(int id);
    }
}
