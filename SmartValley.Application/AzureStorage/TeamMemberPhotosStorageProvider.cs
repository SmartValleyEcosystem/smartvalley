namespace SmartValley.Application.AzureStorage
{
    public class TeamMemberPhotosStorageProvider : AzureStorageProvider
    {
        public TeamMemberPhotosStorageProvider(AzureStorageOptions azureStorageOptions)
            : base(azureStorageOptions, "teamMember-photos")
        {
        }
    }
}