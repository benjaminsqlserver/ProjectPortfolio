using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a date range is considered invalid,
    /// such as when the start date is after the end date,
    /// or the duration is negative.
    /// </summary>
    public class InvalidDateRangeException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateRangeException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidDateRangeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateRangeException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidDateRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
