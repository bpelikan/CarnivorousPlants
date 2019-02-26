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
    public class PhotoStorageService : IPhotoStorageService
    {
        private readonly IConfiguration _configuration;

        CloudBlobClient blobClient;
        string baseBlobUri;
        string blobAccountName;
        string blobKeyValue;

        public PhotoStorageService(IConfiguration configuration)
        {
            _configuration = configuration;

            baseBlobUri = configuration["baseBlobUri"];
            blobAccountName = configuration["blobAccountName"];
            blobKeyValue = configuration["blobKeyValue"];
            var credential = new StorageCredentials(blobAccountName, blobKeyValue);
            blobClient = new CloudBlobClient(new Uri(baseBlobUri), credential);
        }

        public async Task<string> SavePhotoAsync(Stream CvStream, string fileName)
        {
            var photoId = Guid.NewGuid().ToString() + "." + Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName);

            var container = blobClient.GetContainerReference("photostorage");
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
            var container = blobClient.GetContainerReference("photostorage");
            var blob = container.GetBlockBlobReference(photoId);
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return $"{baseBlobUri}photostorage/{photoId}{sas}";
        }
    }
}
