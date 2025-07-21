using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Domain.Common
{
    // Abstract base class representing an aggregate root in Domain-Driven Design (DDD).
    // Inherits from BaseEntity and adds support for domain events.
    public abstract class AggregateRoot : BaseEntity
    {
        // Backing field for storing domain events internally
        private readonly List<object> _domainEvents = new();

        // Exposes domain events as a read-only list.
        // The [NotMapped] attribute ensures this property is not persisted to the database.
        [NotMapped]
        public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();

        // Default constructor calls base class constructor
        protected AggregateRoot() : base() { }

        // Constructor that accepts a specific ID, useful for reconstructing existing aggregates
        protected AggregateRoot(Guid id) : base(id) { }

        // Adds a domain event to the internal list.
        // This allows the aggregate to emit events that other parts of the system can react to.
        protected void AddDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // Clears all stored domain events.
        // Typically called after the events have been dispatched.
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
