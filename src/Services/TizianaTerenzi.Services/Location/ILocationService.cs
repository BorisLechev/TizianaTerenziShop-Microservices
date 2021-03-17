namespace TizianaTerenzi.Services.Location
{
    using System.Threading.Tasks;

    public interface ILocationService
    {
        public Task<string> GetLocationAsync();
    }
}
