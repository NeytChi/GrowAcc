namespace GrowAcc.Core
{
    public class DomainError(string? errorMessage, ErrorType errorType)
    {
        public string? ErrorMessage { get; } = errorMessage;
        public ErrorType ErrorType { get; } = errorType;
        public static DomainError NotFound(string? message = "Given element not found.") => new(message, ErrorType.NotFound);
        public static DomainError Conflict(string? message = "Conflict operation.") => new(message, ErrorType.Conflict);
        public static DomainError NotValid(string? message = "Don't pass validation on the server.") => new(message, ErrorType.NotValid);
    }

    public enum ErrorType
    {
        NotFound,
        Conflict,
        NotValid
    }
}