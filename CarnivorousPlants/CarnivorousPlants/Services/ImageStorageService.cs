using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IConfiguration _configuration;

        CloudBlobClient blobClient;
        string baseBlobUri;
        string blobAccountName;
        string blobKeyValue;

        public ImageStorageService(IConfiguration configuration)
        {
            _configuration = configuration;

            baseBlobUri = configuration["baseBlobUri"];
            blobAccountName = configuration["blobAccountName"];
            blobKeyValue = configuration["blobKeyValue"];
            var credential = new StorageCredentials(blobAccountName, blobKeyValue);
            blobClient = new CloudBlobClient(new Uri(baseBlobUri), credential);
        }

        public async Task<string> SaveImageAsync(Stream CvStream, string fileName)
        {
            var photoId = Guid.NewGuid().ToString() + "." + Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName);

            var container = blobClient.GetContainerReference("imagestorage");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(photoId);
            blob.Properties.ContentType = "image/jpg";

            try
            {
                await blob.UploadFromStreamAsync(CvStream);
                return photoId;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }


        public string UriFor(string photoId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1)
            };
            var container = blobClient.GetContainerReference("imagestorage");
            var blob = container.GetBlockBlobReference(photoId);
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return $"{baseBlobUri}imagestorage/{photoId}{sas}";
        }
    }
}
