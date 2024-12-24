namespace VOEConsulting.Flame.Common.Core.Events
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public int Version { get; set; } = 1; // Default version for the event
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the event
        public Guid AggregateId { get; set; } // Identifier of the related aggregate
        public DateTimeOffset OccurredOnUtc { get; set; } = DateTimeOffset.UtcNow; // Timestamp of event occurrence
        public string EventType { get; set; } // Type of the event

        // Constructor to initialize basic properties
        protected IntegrationEvent(Guid aggregateId)
        {
            if (aggregateId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(aggregateId), "AggregateId cannot be empty.");
            }

            AggregateId = aggregateId;
            EventType = GetType().Name; // Use the class name as the event type
        }

        // Default constructor for serialization frameworks
        protected IntegrationEvent() { }
    }

}
