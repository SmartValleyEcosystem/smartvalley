namespace SmartValley.WebApi.Projects.Requests
{
    public class AddProjectTeamMemberPhotoRequest
    {
        public long ProjectId { get; set; }

        public long ProjectTeamMemberId { get; set; }
    }
}