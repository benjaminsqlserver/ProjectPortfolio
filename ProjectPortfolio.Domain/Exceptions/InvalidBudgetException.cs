using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Exceptions
{
    /// <summary>
    /// Exception type used to indicate invalid operations or data related to budget entities.
    /// </summary>
    public class InvalidBudgetException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBudgetException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidBudgetException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBudgetException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidBudgetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
