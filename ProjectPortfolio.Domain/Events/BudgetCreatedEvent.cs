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
    /// Domain event triggered when a new budget is created for a project.
    /// Used to notify other parts of the system (e.g., logging, auditing, projections)
    /// that a budget entity has been successfully instantiated.
    /// </summary>
    public class BudgetCreatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget that was created.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project to which this budget belongs.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget (e.g., "Marketing", "R&D").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the amount that was initially allocated for this budget.
        /// </summary>
        public Money AllocatedAmount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetCreatedEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the newly created budget.</param>
        /// <param name="projectId">The ID of the associated project.</param>
        /// <param name="category">The category of the budget.</param>
        /// <param name="allocatedAmount">The amount allocated to the budget upon creation.</param>
        public BudgetCreatedEvent(Guid budgetId, Guid projectId, string category, Money allocatedAmount)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            AllocatedAmount = allocatedAmount;
        }
    }

}
