namespace TizianaTerenzi.Services.Data.Contacts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Contacts;

    public class ContactsService : IContactsService
    {
        private readonly IRepository<ContactFormEntry> contactFormEntriesRepository;

        public ContactsService(IRepository<ContactFormEntry> contactFormEntriesRepository)
        {
            this.contactFormEntriesRepository = contactFormEntriesRepository;
        }

        public async Task<IEnumerable<ContactMessagesViewModel>> GetAllAsync()
        {
            var emails = await this.contactFormEntriesRepository
                .AllAsNoTracking()
                .To<ContactMessagesViewModel>()
                .ToListAsync();

            return emails;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var email = await this.contactFormEntriesRepository
                        .All()
                        .SingleOrDefaultAsync(c => c.Id == id);

            if (email == null)
            {
                return false;
            }

            this.contactFormEntriesRepository.Delete(email);
            await this.contactFormEntriesRepository.SaveChangesAsync();

            return true;
        }
    }
}
