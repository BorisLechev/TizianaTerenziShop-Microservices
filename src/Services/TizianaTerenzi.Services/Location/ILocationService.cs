namespace TizianaTerenzi.Services.Location
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Services.Models.Location;

    public interface ILocationService
    {
        public Task<CountryTownIpServiceModel> GetLocationAsync();

        public Task<string> GetIpAddressAsync();
    }
}
