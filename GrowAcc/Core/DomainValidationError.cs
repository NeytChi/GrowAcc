namespace GrowAcc.Core
{
    public class DomainValidationError : DomainError
    {
        public Dictionary<string, string> errors { get; set; }
        public ErrorType errorType = ErrorType.NotValid;
        public static DomainValidationError NotValid(Dictionary<string, string> errors) => new("", ErrorType.NotValid, errors);
        public DomainValidationError(string? errorMessage, ErrorType errorType) : base(errorMessage, errorType)
        {

        }
        public DomainValidationError(string? errorMessage, ErrorType errorType, Dictionary<string, string> errors) : base(errorMessage, errorType)
        {
            this.errors = errors;
        }
    }
}
