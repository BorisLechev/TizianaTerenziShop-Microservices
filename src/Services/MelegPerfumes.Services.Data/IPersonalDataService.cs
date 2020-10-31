namespace MelegPerfumes.Services.Data
{
    using System.Threading.Tasks;

    public interface IPersonalDataService
    {
        Task<string> GetPersonalDataForUserJson(string userId);
    }
}
