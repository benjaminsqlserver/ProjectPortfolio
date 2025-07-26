// Value Object related exception specifically for invalid monetary operations or states
using ProjectPortfolio.Domain.Exceptions;

public class InvalidMoneyException : DomainException
{
    // Constructor that accepts a custom message
    public InvalidMoneyException(string message) : base(message)
    {
    }

    // Constructor that accepts a custom message and an inner exception
    // Useful for wrapping lower-level exceptions while preserving stack trace and context
    public InvalidMoneyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
