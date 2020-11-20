namespace TizianaTerenzi.Services
{
    public interface IUrlGenerator
    {
        string GenerateUrl(int id, string name);

        string ToUrl(string str);
    }
}
