namespace TizianaTerenzi.Administration.Services.Data.Contacts
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Services.Location;
    using TizianaTerenzi.Administration.Web.Models.Contacts;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.Mapping;

    public class ContactsService : IContactsService
    {
        private readonly ILocationService locationService;
        private readonly IRepository<ContactFormEntry> contactsRepository;

        public ContactsService(
            ILocationService locationService,
            IRepository<ContactFormEntry> contactsRepository)
        {
            this.locationService = locationService;
            this.contactsRepository = contactsRepository;
        }

        public async Task<IEnumerable<ContactMessagesViewModel>> GetAllAsync()
        {
            var emails = await this.contactsRepository
                                .All()
                                .To<ContactMessagesViewModel>()
                                .ToListAsync();

            return emails;
        }

        public async Task<bool> CreateAsync(ContactMessageInputModel inputModel)
        {
            var ip = await this.locationService.GetIpAddressAsync();

            var contactFormEntry = new ContactFormEntry
            {
                Name = inputModel.Name,
                Email = inputModel.Email,
                Subject = inputModel.Subject,
                Content = inputModel.Content,
                Ip = ip,
            };

            await this.contactsRepository.AddAsync(contactFormEntry);
            var result = await this.contactsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var email = await this.contactsRepository
                            .All()
                            .SingleOrDefaultAsync(c => c.Id == id);

            if (email == null)
            {
                return false;
            }

            this.contactsRepository.Delete(email);
            var result = await this.contactsRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
