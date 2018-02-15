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
        EmailNotConfirmed,
        IncorrectData,
        EmailAlreadySent,
        UserNotFound,
        RoleNotFound,
        UserHaveNoRole,
        EmailSendingFailed,
        InvalidFileUploaded,
        AddressAlreadyExists
    }
}