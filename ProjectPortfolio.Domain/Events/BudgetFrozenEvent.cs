using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event triggered when a budget is frozen, preventing further modifications or expenditures.
    /// This is typically used to pause spending during audits, policy reviews, or exceptional circumstances.
    /// </summary>
    public class BudgetFrozenEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget that was frozen.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with the frozen budget.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget that was frozen (e.g., "Marketing", "Operations").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the reason for freezing the budget.
        /// This may include internal notes like "pending audit", "overspending", or "awaiting approval".
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetFrozenEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the frozen budget.</param>
        /// <param name="projectId">The ID of the related project.</param>
        /// <param name="category">The category of the frozen budget.</param>
        /// <param name="reason">The reason the budget was frozen.</param>
        public BudgetFrozenEvent(Guid budgetId, Guid projectId, string category, string reason)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            Reason = reason;
        }
    }

}
