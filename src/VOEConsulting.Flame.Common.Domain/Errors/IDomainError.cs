namespace VOEConsulting.Flame.Common.Domain.Errors
{
    public interface IDomainError
    {
        string? ErrorMessage { get; init; }
        ErrorType ErrorType { get; init; }
        public List<string>? Errors { get; init; }
    }

}
