namespace SmartValley.Application.AzureStorage
{
    public class ProjectStorageProvider : AzureStorageProvider
    {
        public ProjectStorageProvider(AzureStorageOptions azureStorageOptions)
            : base(azureStorageOptions, "projects")
        {
        }
    }
}