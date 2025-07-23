using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Exceptions
{
    /// <summary>
    /// Represents errors that occur when a time entry violates domain rules or constraints.
    /// Inherits from the base <see cref="DomainException"/> to signify a business logic failure.
    /// </summary>
    public class InvalidTimeEntryException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTimeEntryException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message describing the invalid time entry condition.</param>
        public InvalidTimeEntryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTimeEntryException"/> class
        /// with a specified error message and a reference to the inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The error message describing the invalid time entry condition.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidTimeEntryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
