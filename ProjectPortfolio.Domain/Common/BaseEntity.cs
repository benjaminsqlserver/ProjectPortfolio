using System;

namespace ProjectPortfolio.Domain.Common
{
    // Abstract base class for domain entities, providing common properties and behavior
    public abstract class BaseEntity
    {
        // Unique identifier for the entity
        public Guid Id { get; protected set; }

        // Timestamp for when the entity was created
        public DateTime CreatedAt { get; protected set; }

        // Timestamp for when the entity was last updated (nullable)
        public DateTime? UpdatedAt { get; protected set; }

        // Username or identifier of the user who created the entity
        public string CreatedBy { get; protected set; } = string.Empty;

        // Username or identifier of the user who last updated the entity (nullable)
        public string? UpdatedBy { get; protected set; }

        // Default constructor initializes Id and CreatedAt with default values
        protected BaseEntity()
        {
            Id = Guid.NewGuid();              // Generate a new GUID for the entity
            CreatedAt = DateTime.UtcNow;      // Set the creation time to current UTC time
        }

        // Constructor that accepts a specific Guid, useful when restoring entities from persistence
        protected BaseEntity(Guid id)
        {
            Id = id;                          // Assign the provided Guid
            CreatedAt = DateTime.UtcNow;      // Set the creation time to current UTC time
        }

        // Method to update the UpdatedAt and UpdatedBy properties
        public void SetUpdated(string updatedBy)
        {
            UpdatedAt = DateTime.UtcNow;      // Set update time to current UTC time
            UpdatedBy = updatedBy;            // Set the user who made the update
        }

        // Override for equality comparison based on Id and type
        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity entity) return false;              // Return false if not a BaseEntity
            if (ReferenceEquals(this, entity)) return true;              // Return true if same instance
            if (GetType() != entity.GetType()) return false;             // Ensure both objects are of the same type
            return Id == entity.Id;                                      // Compare based on Id
        }

        // Override for generating hash code, based on Id
        public override int GetHashCode()
        {
            return Id.GetHashCode();                                     // Use Id's hash code
        }
    }
}
