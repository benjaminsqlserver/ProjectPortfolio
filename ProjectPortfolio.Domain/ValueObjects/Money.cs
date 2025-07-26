/// <summary>
/// Represents a monetary value object with an amount and a currency code.
/// Enforces strong domain rules to ensure monetary consistency and safety across operations.
///
/// Key Features:
/// - Immutable structure: Properties are readonly outside the constructor.
/// - Validation:
///   - Currency must be a non-empty, 3-character ISO currency code (e.g., "USD").
///   - Amounts are rounded to two decimal places for standard financial precision.
/// - Factory methods for common currencies (Dollar, Euro, Pound).
/// - Arithmetic operations:
///   - Add, Subtract: Only allowed between Money instances of the same currency.
///   - Multiply, Divide: Support scalar multiplication/division (e.g., tax, scaling).
///   - Negate and Abs: Return negative or absolute values.
/// - Comparison:
///   - Custom comparison operators (>, <, >=, <=) validate currency equality.
///   - Boolean flags (IsPositive, IsNegative, IsZero) for state introspection.
/// - Business logic:
///   - GetVariance: Calculates percent variance from a budget.
///   - IsOverBudget: Checks if the current amount exceeds a budgeted amount.
/// - Value-based equality:
///   - Implements equality through `ValueObject` base class by comparing amount and currency.
/// - Overrides ToString for currency-formatted display, with optional formatting string.
///
/// Design Notes:
/// - Adheres to DDD principles: This is a rich Value Object modeling a concept in the financial domain.
/// - Domain exceptions (e.g., InvalidMoneyException) are thrown on illegal operations or state.
/// - Built for extensibility and integration with EF Core (via private constructor).
/// </summary>


using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Exceptions;

namespace ProjectPortfolio.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; } // The numeric value of the money
        public string Currency { get; private set; } // The ISO 3-letter currency code (e.g., USD, EUR)

        // Required for Entity Framework Core deserialization
        private Money() { }

        // Constructor with validation and normalization
        public Money(decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new InvalidMoneyException("Currency cannot be null or empty.");

            if (currency.Length != 3)
                throw new InvalidMoneyException("Currency must be a 3-character ISO code (e.g., USD, EUR).");

            Amount = Math.Round(amount, 2); // Round to 2 decimal places
            Currency = currency.ToUpperInvariant(); // Normalize to uppercase
        }

        // Creates a zero money value for a given currency
        public static Money Zero(string currency) => new Money(0, currency);

        // Factory method for US dollars
        public static Money Dollar(decimal amount) => new Money(amount, "USD");

        // Factory method for Euros
        public static Money Euro(decimal amount) => new Money(amount, "EUR");

        // Factory method for British Pounds
        public static Money Pound(decimal amount) => new Money(amount, "GBP");

        // Adds two Money objects of the same currency
        public Money Add(Money other)
        {
            if (!SameCurrency(other))
                throw new InvalidMoneyException($"Cannot add different currencies: {Currency} and {other.Currency}");

            return new Money(Amount + other.Amount, Currency);
        }

        // Subtracts another Money object from this one (currency must match)
        public Money Subtract(Money other)
        {
            if (!SameCurrency(other))
                throw new InvalidMoneyException($"Cannot subtract different currencies: {Currency} and {other.Currency}");

            return new Money(Amount - other.Amount, Currency);
        }

        // Multiplies the Money amount by a decimal factor
        public Money Multiply(decimal factor)
        {
            return new Money(Amount * factor, Currency);
        }

        // Divides the Money amount by a decimal divisor
        public Money Divide(decimal divisor)
        {
            if (divisor == 0)
                throw new InvalidMoneyException("Cannot divide by zero.");

            return new Money(Amount / divisor, Currency);
        }

        // Returns a new Money object with the negated amount
        public Money Negate()
        {
            return new Money(-Amount, Currency);
        }

        // Returns a new Money object with the absolute amount
        public Money Abs()
        {
            return new Money(Math.Abs(Amount), Currency);
        }

        // True if amount > 0
        public bool IsPositive => Amount > 0;

        // True if amount < 0
        public bool IsNegative => Amount < 0;

        // True if amount == 0
        public bool IsZero => Amount == 0;

        // Checks if the currencies of two Money objects match (case-insensitive)
        public bool SameCurrency(Money other)
        {
            return Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase);
        }

        // Calculates percentage variance from a budgeted value
        public decimal GetVariance(Money budgeted)
        {
            if (!SameCurrency(budgeted))
                throw new InvalidMoneyException($"Cannot calculate variance for different currencies: {Currency} and {budgeted.Currency}");

            if (budgeted.Amount == 0) return 0;
            return ((Amount - budgeted.Amount) / budgeted.Amount) * 100;
        }

        // Determines if this amount exceeds the budgeted amount
        public bool IsOverBudget(Money budgeted)
        {
            if (!SameCurrency(budgeted))
                throw new InvalidMoneyException($"Cannot compare different currencies: {Currency} and {budgeted.Currency}");

            return Amount > budgeted.Amount;
        }

        // Operator overloads for syntactic sugar

        public static Money operator +(Money left, Money right) => left.Add(right); // Addition

        public static Money operator -(Money left, Money right) => left.Subtract(right); // Subtraction

        public static Money operator *(Money money, decimal factor) => money.Multiply(factor); // Scalar multiplication

        public static Money operator /(Money money, decimal divisor) => money.Divide(divisor); // Scalar division

        public static Money operator -(Money money) => money.Negate(); // Unary negation

        // Greater-than comparison with currency check
        public static bool operator >(Money left, Money right)
        {
            if (!left.SameCurrency(right))
                throw new InvalidMoneyException($"Cannot compare different currencies: {left.Currency} and {right.Currency}");
            return left.Amount > right.Amount;
        }

        // Less-than comparison with currency check
        public static bool operator <(Money left, Money right)
        {
            if (!left.SameCurrency(right))
                throw new InvalidMoneyException($"Cannot compare different currencies: {left.Currency} and {right.Currency}");
            return left.Amount < right.Amount;
        }

        // Greater-than or equal
        public static bool operator >=(Money left, Money right) => !(left < right);

        // Less-than or equal
        public static bool operator <=(Money left, Money right) => !(left > right);

        // Used for ValueObject equality comparisons
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        // Default string representation (e.g., "123.45 USD")
        public override string ToString()
        {
            return $"{Amount:N2} {Currency}";
        }

        // Custom format string representation (e.g., with currency + custom decimal format)
        public string ToString(string format)
        {
            return $"{Amount.ToString(format)} {Currency}";
        }
    }
}


