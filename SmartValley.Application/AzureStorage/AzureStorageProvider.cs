using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SmartValley.Application.AzureStorage
{
    public abstract class AzureStorageProvider
    {
        private readonly string _containerName;
        private readonly CloudBlobClient _client;

        protected AzureStorageProvider(AzureStorageOptions azureStorageOptions, string containerName)
        {
            _containerName = containerName;
            var storageAccount = CloudStorageAccount.Parse(azureStorageOptions.StorageConnectionString);
            _client = storageAccount.CreateCloudBlobClient();
        }

        public async Task InitializeAsync()
        {
            var container = _client.GetContainerReference(_containerName);
            await container.CreateIfNotExistsAsync();

            await SetPermissions(container);
        }

        protected virtual async Task SetPermissions(CloudBlobContainer container)
        {
            var permissions = await container.GetPermissionsAsync();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);
        }

        public async Task<AzureFile> DowndloadAsync(string fileName)
        {
            var blockBlob = _client.GetContainerReference(_containerName).GetBlockBlobReference(fileName);
            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                return new AzureFile(fileName, stream.ToArray());
            }
        }

        public async Task<string> UploadAndGetUriAsync(string fileName, AzureFile file)
        {
            var blockBlob = _client.GetContainerReference(_containerName).GetBlockBlobReference(fileName);
            await blockBlob.DeleteIfExistsAsync();
            await blockBlob.UploadFromByteArrayAsync(file.Data, 0, file.Data.Length);
            return blockBlob.Uri.AbsoluteUri;
        }

        public Task DeleteAsync(string fileName)
        {
            var blockBlob = _client.GetContainerReference(_containerName).GetBlockBlobReference(fileName);
            return blockBlob.DeleteIfExistsAsync();
        }
    }
}