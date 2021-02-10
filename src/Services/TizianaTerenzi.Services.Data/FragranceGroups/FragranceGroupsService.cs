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
        private readonly IRepository<FragranceGroup> fragranceGroupsRepository;

        public FragranceGroupsService(
            IRepository<FragranceGroup> fragranceGroupsRepository)
        {
            this.fragranceGroupsRepository = fragranceGroupsRepository;
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
    }
}
