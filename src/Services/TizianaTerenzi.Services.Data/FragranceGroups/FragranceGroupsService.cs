namespace TizianaTerenzi.Services.Data.FragranceGroups
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class FragranceGroupsService : IFragranceGroupsService
    {
        private readonly IDeletableEntityRepository<FragranceGroup> fragranceGroupsRepository;

        public FragranceGroupsService(
            IDeletableEntityRepository<FragranceGroup> fragranceGroupsRepository)
        {
            this.fragranceGroupsRepository = fragranceGroupsRepository;
        }

        public async Task<bool> CreateFragranceGroupAsync(FragranceGroup fragranceGroup)
        {
            await this.fragranceGroupsRepository.AddAsync(fragranceGroup);
            int result = await this.fragranceGroupsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllFragranceGroupsAsync()
        {
            var fragranceGroups = await this.fragranceGroupsRepository
                .AllAsNoTracking()
                .OrderBy(fg => fg.Name)
                .Select(fg => new SelectListItem
                {
                    Value = fg.Id.ToString(),
                    Text = fg.Name,
                })
                .ToListAsync();

            return fragranceGroups;
        }

        public async Task<FragranceGroup> GetFragranceGroupById(int id)
        {
            var fragranceGroup = await this.fragranceGroupsRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(fg => fg.Id == id);

            return fragranceGroup;
        }
    }
}
