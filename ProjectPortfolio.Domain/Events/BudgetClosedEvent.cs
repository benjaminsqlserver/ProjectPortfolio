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
    /// Domain event triggered when a budget is officially closed, marking the end of its lifecycle.
    /// No further modifications or expenses are allowed after closure.
    /// </summary>
    public class BudgetClosedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget that was closed.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with the closed budget.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget that was closed (e.g., "R&D", "Marketing").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the total amount that was allocated to the budget.
        /// </summary>
        public Money AllocatedAmount { get; }

        /// <summary>
        /// Gets the total amount that was spent from the budget.
        /// </summary>
        public Money SpentAmount { get; }

        /// <summary>
        /// Gets the reason why the budget was closed.
        /// This could include explanations such as "project completed", "reallocation", or "contract ended".
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetClosedEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the budget being closed.</param>
        /// <param name="projectId">The ID of the associated project.</param>
        /// <param name="category">The category of the budget.</param>
        /// <param name="allocatedAmount">The total allocated budget amount.</param>
        /// <param name="spentAmount">The total spent from the budget.</param>
        /// <param name="reason">The reason the budget was closed.</param>
        public BudgetClosedEvent(Guid budgetId, Guid projectId, string category, Money allocatedAmount, Money spentAmount, string reason)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            AllocatedAmount = allocatedAmount;
            SpentAmount = spentAmount;
            Reason = reason;
        }
    }

}
