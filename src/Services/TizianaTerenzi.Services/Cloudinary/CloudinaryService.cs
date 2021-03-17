namespace TizianaTerenzi.Services.Cloudinary
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task DeletePictureAsync(string url)
        {
            var publicId = this.GetCloudinaryPublicIdFromUrl(url);

            var deletionParams = new DeletionParams(publicId);
            await this.cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await pictureFile.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "product_images",
                    File = new FileDescription(fileName, ms),
                };

                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult?.SecureUrl.AbsoluteUri;
        }

        private string GetCloudinaryPublicIdFromUrl(string url)
        {
            var startIndex = url.IndexOf('/') + 1;
            var length = url.LastIndexOf('.') - startIndex;
            var publicId = url.Substring(startIndex, length);

            return publicId;
        }
    }
}
