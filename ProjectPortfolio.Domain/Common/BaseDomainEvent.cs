namespace ProjectPortfolio.Domain.Common
{
    // Base class for domain events in the system.
    // All domain events should inherit from this class to ensure consistency and traceability.
    public abstract class BaseDomainEvent
    {
        // Unique identifier for the event instance
        public Guid Id { get; }

        // Timestamp indicating when the event occurred (in UTC)
        public DateTime OccurredOn { get; }

        // Constructor sets the event's unique ID and occurrence timestamp
        protected BaseDomainEvent()
        {
            Id = Guid.NewGuid();              // Generate a new unique ID for the event
            OccurredOn = DateTime.UtcNow;     // Capture the time the event was created
        }
    }
}
