namespace SmartValley.Domain.Exceptions
{
    public enum ErrorCode
    {
        EtherAlreadySent,
        ServerError,
        ValidatationError,
        ProjectNotFound,
        VotingSprintAlreadyInProgress,
        NotEnoughProjectsForSprintStart,
        EmailAlreadyExists,
        InvalidSignature,
        EmailNotConfrimed,
        UserNotFound,
        RoleNotFound,
        UserHaveNoRole
    }
}