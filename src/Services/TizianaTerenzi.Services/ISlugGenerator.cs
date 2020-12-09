namespace TizianaTerenzi.Services
{
    public interface ISlugGenerator
    {
        string GenerateUrl(int id, string name);

        string ToUrl(string str);
    }
}
