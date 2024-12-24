
namespace VOEConsulting.Flame.Common.Domain.Errors
{
    public record DomainError : IDomainError
    {
        // Static factory methods for common error types
        public static DomainError Conflict(string? message = "The data provided conflicts with existing data.") =>
            new(message ?? "The data provided conflicts with existing data.", ErrorType.Conflict);

        public static DomainError NotFound(string? message = "The requested item could not be found.") =>
            new(message ?? "The requested item could not be found.", ErrorType.NotFound);

        public static DomainError BadRequest(string? message = "Invalid request or parameters.") =>
            new(message ?? "Invalid request or parameters.", ErrorType.BadRequest);

        public static DomainError Validation(string? message = "Validation Failed.", List<string>? errors = null) =>
            new(message ?? "Validation Failed.", ErrorType.Validation, errors);

        public static DomainError UnExpected(string? message = "Unexpected error happened.") =>
            new(message ?? "Something when wrong.", ErrorType.Unexpected);

        // Constructor is private to enforce the usage of static methods
        private DomainError(string? message, ErrorType errorType, List<string>? errors = null)
        {
            ErrorMessage = message;
            ErrorType = errorType;
            Errors = errors ?? new List<string>();
        }

        // Properties for error message and type
        public string? ErrorMessage { get; init; }
        public ErrorType ErrorType { get; init; }
        public List<string>? Errors { get; init; }
    }

}
