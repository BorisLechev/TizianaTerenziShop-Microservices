namespace TizianaTerenzi.Products.Services.Cloudinary
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName);

        Task DeletePictureAsync(string url);

        Task<string> UploadPictureAsByteArrayAsync(byte[] pictureAsByteArray, string fileName);
    }
}
