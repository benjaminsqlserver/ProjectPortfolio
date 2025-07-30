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
    /// Domain event triggered when the allocated amount of a budget is changed.
    /// This allows tracking changes in financial planning and helps maintain an audit trail.
    /// </summary>
    public class BudgetAllocationUpdatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the affected budget.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the associated project.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget that was updated (e.g., "Marketing", "R&D").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the previous allocation value before the update.
        /// Useful for understanding the change delta or reverting if necessary.
        /// </summary>
        public Money PreviousAllocation { get; }

        /// <summary>
        /// Gets the new allocation value that has been applied.
        /// </summary>
        public Money NewAllocation { get; }

        /// <summary>
        /// Gets the reason for the budget allocation update.
        /// This can include business justifications or correction notes.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetAllocationUpdatedEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the updated budget.</param>
        /// <param name="projectId">The ID of the associated project.</param>
        /// <param name="category">The category of the budget being updated.</param>
        /// <param name="previousAllocation">The previous budget allocation.</param>
        /// <param name="newAllocation">The new budget allocation.</param>
        /// <param name="reason">The reason for the allocation change.</param>
        public BudgetAllocationUpdatedEvent(
            Guid budgetId,
            Guid projectId,
            string category,
            Money previousAllocation,
            Money newAllocation,
            string reason)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            PreviousAllocation = previousAllocation;
            NewAllocation = newAllocation;
            Reason = reason;
        }
    }

}
