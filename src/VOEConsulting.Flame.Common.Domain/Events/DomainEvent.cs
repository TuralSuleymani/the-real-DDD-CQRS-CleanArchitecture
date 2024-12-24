using System.Reflection;
using VOEConsulting.Flame.Common.Domain.Events.Decorators;

namespace VOEConsulting.Flame.Common.Domain.Events
{
    public class DomainEvent : IDomainEvent
    {
        public int Version { get; set; } = 1;  // Default version set to 1

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid AggregateId { get; set; }

        public DateTimeOffset OccurredOnUtc { get; set; }

        public string EventType { get; set; }

        public string AggregateType { get; set; }

        public string? TraceInfo { get; set; }

        // Default constructor
        public DomainEvent() { }

        // Parameterized constructor
        protected DomainEvent(Guid aggregateId, DateTimeOffset occurredOnUtc)
        {
            AggregateId = aggregateId != Guid.Empty ? aggregateId : throw new ArgumentNullException(nameof(aggregateId));
            OccurredOnUtc = occurredOnUtc;
            AggregateType = GetAggregateType(GetType()) ?? throw new InvalidOperationException("Aggregate type cannot be null.");
            EventType = GetEventType(this);
        }

        // Retrieves the AggregateType for a given event type
        public static string GetAggregateType<TEvent>() where TEvent : IDomainEvent =>
            GetAggregateType(typeof(TEvent));

        public static string GetAggregateType(Type eventType)
        {
            var attribute = eventType.GetCustomAttribute<AggregateTypeAttribute>();
            return attribute?.AggregateType ?? string.Empty;
        }

        // Retrieves the EventType based on the event and its aggregate type
        public static string GetEventType(IDomainEvent @event) =>
            GetEventType(@event.GetType(), @event.AggregateType);

        public static string GetEventType<TEvent>() where TEvent : IDomainEvent =>
            GetEventType(typeof(TEvent));

        public static string GetEventType(Type eventType, string? prefix = null)
        {
            prefix ??= GetAggregateType(eventType);
            return $"{prefix}.{eventType.Name}";
        }
    }

}
