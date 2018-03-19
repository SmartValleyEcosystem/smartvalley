namespace SmartValley.Application.AzureStorage
{
    public class ProjectTeamMembersStorageProvider : AzureStorageProvider
    {
        public ProjectTeamMembersStorageProvider(AzureStorageOptions azureStorageOptions)
            : base(azureStorageOptions, "project-team-members")
        {
        }
    }
}