using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Exceptions
{
    /// <summary>
    /// Base class for all domain-specific exceptions.
    /// Inherits from System.Exception and is intended to represent violations of business rules or domain logic.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with a custom error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected DomainException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with a custom error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
