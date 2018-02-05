namespace SmartValley.Application.Exceptions
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
        UserIsNotExist
    }
}