namespace TizianaTerenzi.Identity.Services.Location
{
    using TizianaTerenzi.Identity.Services.Models.Location;

    public interface ILocationService
    {
        public Task<CountryTownIpServiceModel> GetLocationAsync();

        public Task<string> GetIpAddressAsync();
    }
}
