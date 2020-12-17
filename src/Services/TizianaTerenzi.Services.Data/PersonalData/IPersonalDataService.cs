namespace TizianaTerenzi.Services.Data.PersonalData
{
    using System.Threading.Tasks;

    public interface IPersonalDataService
    {
        Task<string> GetPersonalDataForUserJsonAsync(string userId);

        Task<bool> DeleteUserAsync(string userId);
    }
}
