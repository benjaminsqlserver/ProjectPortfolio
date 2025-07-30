using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event triggered when a previously frozen budget is unfrozen, allowing modifications and expenses again.
    /// This typically occurs after audits, approvals, or other conditions have been satisfied.
    /// </summary>
    public class BudgetUnfrozenEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget that was unfrozen.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with the budget.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget that was unfrozen (e.g., "Marketing", "IT Infrastructure").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the reason for unfreezing the budget.
        /// Typically includes context like "audit complete", "approval granted", or "funds restored".
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetUnfrozenEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the unfrozen budget.</param>
        /// <param name="projectId">The ID of the project associated with the budget.</param>
        /// <param name="category">The category of the budget that was unfrozen.</param>
        /// <param name="reason">The reason for unfreezing the budget.</param>
        public BudgetUnfrozenEvent(Guid budgetId, Guid projectId, string category, string reason)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            Reason = reason;
        }
    }

}
