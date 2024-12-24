namespace VOEConsulting.Flame.Common.Core.Events
{
    public interface IIntegrationEvent
    {
        int Version { get; }
        string EventType { get; }
        Guid Id { get; }
        DateTimeOffset OccurredOnUtc { get; }
        Guid AggregateId { get; }
    }
}
