namespace VOEConsulting.Flame.Common.Domain.Exceptions
{
    public class EventualConsistencyException : Exception
    {
        public string ErrorCode { get; }
        public string ErrorMessage { get; }
        public List<string> Details { get; }

        public EventualConsistencyException(string errorCode, string errorMessage, List<string>? details = null)
            : base(message: errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Details = details ?? new();
        }
    }

}
