namespace TizianaTerenzi.Services.Location
{
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;

    using IpInfo;
    using Microsoft.Extensions.Configuration;
    using TizianaTerenzi.Services.Models.Location;

    public class LocationService : ILocationService
    {
        private readonly IConfiguration configuration;

        public LocationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> GetIpAddress()
        {
            using var client = new HttpClient();
            var key = this.configuration["IpInfo:ApiKey"];
            var api = new IpInfoApi(key, client); // Some methods work without a token, for this case there is a constructor without a token.

            var ip = await api.GetCurrentIpAsync();

            return ip;
        }

        public async Task<CountryTownIpServiceModel> GetLocationAsync()
        {
            using var client = new HttpClient();
            var key = this.configuration["IpInfo:ApiKey"];
            var api = new IpInfoApi(key, client); // Some methods work without a token, for this case there is a constructor without a token.

            var response = await api.GetCurrentInformationAsync();
            RegionInfo countryInfo = new RegionInfo(response.Country);

            var serviceModel = new CountryTownIpServiceModel
            {
                CountryName = countryInfo.EnglishName,
                Town = response.City,
                Ip = response.Ip,
            };

            return serviceModel;
        }
    }
}
