namespace TizianaTerenzi.Services.Location
{
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;

    using IpInfo;
    using Microsoft.Extensions.Configuration;

    public class LocationService : ILocationService
    {
        private readonly IConfiguration configuration;

        public LocationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<(string CountryName, string Town, string Ip)> GetLocationAsync()
        {
            using var client = new HttpClient();
            var key = this.configuration["IpInfo:ApiKey"];
            var api = new IpInfoApi(key, client); // Some methods work without a token, for this case there is a constructor without a token.

            var response = await api.GetCurrentInformationAsync();
            RegionInfo countryInfo = new RegionInfo(response.Country);
            var countryName = countryInfo.EnglishName;
            var town = response.City;
            var ip = response.Ip;

            return (countryName, town, ip);
        }
    }
}
