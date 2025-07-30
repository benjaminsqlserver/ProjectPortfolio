using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event raised when the total spending exceeds the allocated budget.
    /// This signals a breach in budget limits, typically used to trigger alerts, escalate issues,
    /// or update downstream financial systems or dashboards.
    /// </summary>
    public class BudgetExceededEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget that has been exceeded.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with this budget.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget (e.g., "Infrastructure", "HR").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the originally allocated budget amount.
        /// </summary>
        public Money AllocatedAmount { get; }

        /// <summary>
        /// Gets the actual amount spent, which has exceeded the allocation.
        /// </summary>
        public Money SpentAmount { get; }

        /// <summary>
        /// Gets the variance between spent and allocated amounts.
        /// Positive value indicates how much the budget was exceeded by.
        /// </summary>
        public Money Variance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetExceededEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the budget that was exceeded.</param>
        /// <param name="projectId">The ID of the related project.</param>
        /// <param name="category">The category of the budget.</param>
        /// <param name="allocatedAmount">The amount that was originally allocated to the budget.</param>
        /// <param name="spentAmount">The actual amount spent that exceeded the budget.</param>
        /// <param name="variance">The difference between spent and allocated amounts.</param>
        public BudgetExceededEvent(
            Guid budgetId,
            Guid projectId,
            string category,
            Money allocatedAmount,
            Money spentAmount,
            Money variance)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            AllocatedAmount = allocatedAmount;
            SpentAmount = spentAmount;
            Variance = variance;
        }
    }

}
