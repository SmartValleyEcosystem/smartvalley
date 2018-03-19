using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SmartValley.Application.AzureStorage
{
    public abstract class AzureStorageProvider
    {
        private readonly CloudBlobContainer _container;

        protected AzureStorageProvider(AzureStorageOptions azureStorageOptions, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(azureStorageOptions.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExistsAsync();
        }

        public async Task<AzureFile> DowndloadAsync(string fileName)
        {
            var blockBlob = _container.GetBlockBlobReference(fileName);
            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                return new AzureFile(fileName, stream.ToArray());
            }
        }

        public async Task UploadAsync(string fileName, AzureFile file)
        {
            var blockBlob = _container.GetBlockBlobReference(fileName);
            await blockBlob.DeleteIfExistsAsync();
            await blockBlob.UploadFromByteArrayAsync(file.Data, 0, file.Data.Length);
        }
    }
}