namespace SmartValley.Application.AzureStorage
{
    public class ApplicationTeamMembersStorageProvider : AzureStorageProvider
    {
        public ApplicationTeamMembersStorageProvider(AzureStorageOptions azureStorageOptions)
            : base(azureStorageOptions, "application-team-members")
        {
        }
    }
}