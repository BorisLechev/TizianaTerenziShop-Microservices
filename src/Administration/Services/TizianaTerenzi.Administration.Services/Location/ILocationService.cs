namespace TizianaTerenzi.Administration.Services.Location
{
    using TizianaTerenzi.Administration.Services.Models.Location;

    public interface ILocationService
    {
        public Task<CountryTownIpServiceModel> GetLocationAsync();

        public Task<string> GetIpAddressAsync();
    }
}
