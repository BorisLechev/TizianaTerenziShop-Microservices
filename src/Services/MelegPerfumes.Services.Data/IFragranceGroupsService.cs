namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface IFragranceGroupsService
    {
        Task<bool> CreateFragranceGroupAsync(FragranceGroup fragranceGroup);

        Task CreateFragranceGroupsRangeAsync(IEnumerable<FragranceGroup> fragranceGroups);

        Task<IEnumerable<FragranceGroup>> GetAllFragranceGroups();

        Task<FragranceGroup> FindByNameAsync(string fragranceGroupName);
    }
}
