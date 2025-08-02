using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a resource allocation operation violates domain rules,
    /// such as modifying an inactive or expired allocation, or setting an invalid percentage.
    /// </summary>
    public class InvalidAllocationException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAllocationException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidAllocationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAllocationException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that caused the current exception.</param>
        public InvalidAllocationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
