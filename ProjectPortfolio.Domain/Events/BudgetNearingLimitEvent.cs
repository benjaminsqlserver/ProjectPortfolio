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
    /// Domain event raised when a budget's utilization crosses a predefined threshold
    /// (typically 90%), indicating that the budget is close to being exhausted.
    /// This can be used to alert stakeholders to take preventive actions.
    /// </summary>
    public class BudgetNearingLimitEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget approaching its limit.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with this budget.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget (e.g., "Operations", "IT", "Travel").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the total amount that was originally allocated to this budget.
        /// </summary>
        public Money AllocatedAmount { get; }

        /// <summary>
        /// Gets the total amount that has been spent so far.
        /// </summary>
        public Money SpentAmount { get; }

        /// <summary>
        /// Gets the utilization percentage of the budget,
        /// typically used to determine proximity to budget exhaustion.
        /// </summary>
        public decimal UtilizationPercentage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetNearingLimitEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the budget nearing its spending limit.</param>
        /// <param name="projectId">The ID of the associated project.</param>
        /// <param name="category">The category of the budget.</param>
        /// <param name="allocatedAmount">The total allocated budget.</param>
        /// <param name="spentAmount">The amount already spent.</param>
        /// <param name="utilizationPercentage">The calculated percentage of budget used (e.g., 91.5).</param>
        public BudgetNearingLimitEvent(
            Guid budgetId,
            Guid projectId,
            string category,
            Money allocatedAmount,
            Money spentAmount,
            decimal utilizationPercentage)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            AllocatedAmount = allocatedAmount;
            SpentAmount = spentAmount;
            UtilizationPercentage = utilizationPercentage;
        }
    }

}
