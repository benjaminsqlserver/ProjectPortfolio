using System.Collections.Generic;
using System.Linq;

namespace ProjectPortfolio.Domain.Common
{
    // Abstract base class for value objects in Domain-Driven Design (DDD).
    // Value objects are compared based on their properties, not identity.
    public abstract class ValueObject
    {
        // Helper method to check equality between two value objects using the == operator logic
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            // XOR check: if only one is null, return false
            if (left is null ^ right is null) return false;

            // If both are null, return true; otherwise use .Equals()
            return left?.Equals(right) != false;
        }

        // Helper method to check inequality using the != operator logic
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        // Must be implemented by derived classes to specify which properties are used for equality
        protected abstract IEnumerable<object> GetEqualityComponents();

        // Override of the standard Equals method to compare value objects based on components
        public override bool Equals(object? obj)
        {
            // Return false if null or types don't match
            if (obj == null || obj.GetType() != GetType()) return false;

            // Cast and compare equality components
            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
        
        // Override of GetHashCode that computes hash code based on all equality components
        public override int GetHashCode()
        {
            // Use XOR to combine hash codes of all components
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0) // Handle null values safely
                .Aggregate((x, y) => x ^ y);        // Combine using XOR
        }
    }
}
