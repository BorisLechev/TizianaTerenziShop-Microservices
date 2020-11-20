namespace TizianaTerenzi.Services.Data
{
    using System.Threading.Tasks;

    public interface IPersonalDataService
    {
        Task<string> GetPersonalDataForUserJsonAsync(string userId);

        Task<bool> DeleteUserAsync(string userId);
    }
}
