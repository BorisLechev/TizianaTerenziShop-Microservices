namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public async Task<bool> CreateFragranceGroupsRangeAsync(IEnumerable<FragranceGroup> fragranceGroups)
        {
            await this.fragranceGroupsRepository.AddRangeAsync(fragranceGroups);
            var result = await this.fragranceGroupsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<FragranceGroup> FindByNameAsync(string fragranceGroupName)
        {
            var fragranceGroup = await this.fragranceGroupsRepository
                .All()
                .SingleOrDefaultAsync(fg => fg.Name == fragranceGroupName);

            return fragranceGroup;
        }

        public async Task<IEnumerable<FragranceGroup>> GetAllFragranceGroups()
        {
            var fragranceGroups = await this.fragranceGroupsRepository
                .All()
                .ToListAsync();

            return fragranceGroups;
        }
    }
}
