namespace TizianaTerenzi.Services.PDF
{
    public interface IHtmlToPdfConverter
    {
        byte[] Convert(string basePath, string htmlCode, FormatType formatType, OrientationType orientationType);
    }
}
