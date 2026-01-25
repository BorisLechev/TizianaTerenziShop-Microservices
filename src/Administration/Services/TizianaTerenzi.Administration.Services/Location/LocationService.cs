namespace TizianaTerenzi.Administration.Services.Location
{
    using System.Threading.Tasks;

    using IPinfo;
    using Microsoft.Extensions.Configuration;
    using TizianaTerenzi.Administration.Services.Models.Location;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;

    [TransientRegistration]
    public class LocationService : ILocationService
    {
        private readonly IConfiguration configuration;

        public LocationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> GetIpAddressAsync()
        {
            var token = this.configuration["IpInfo:ApiKey"];

            var client = new IPinfoClient.Builder()
                            .AccessToken(token)
                            .Build();

            var response = await client.IPApi.GetDetailsAsync();

            return response.IP;
        }

        public async Task<CountryTownIpServiceModel> GetLocationAsync()
        {
            var token = this.configuration["IpInfo:ApiKey"];

            var client = new IPinfoClient.Builder()
                            .AccessToken(token)
                            .Build();

            var response = await client.IPApi.GetDetailsAsync();

            var serviceModel = new CountryTownIpServiceModel
            {
                CountryName = response.CountryName,
                Town = response.City,
                Ip = response.IP,
            };

            return serviceModel;
        }
    }
}
