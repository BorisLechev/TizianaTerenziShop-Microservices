namespace TizianaTerenzi.Products.Services.SlugGenerator
{
    public interface ISlugGeneratorService
    {
        string GenerateUrl(int id, string name);

        string ToUrl(string str);
    }
}
