namespace SmartValley.Application.AzureStorage
{
    public class ExpertApplicationsStorageProvider : AzureStorageProvider
    {
        public ExpertApplicationsStorageProvider(AzureStorageOptions azureStorageOptions)
            : base(azureStorageOptions, "expert-applications")
        {
        }
    }
}