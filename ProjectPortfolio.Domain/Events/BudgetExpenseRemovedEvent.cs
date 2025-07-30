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
    /// Domain event raised when an expense is removed from a budget.
    /// This may occur due to corrections, refunds, or accounting adjustments,
    /// and helps maintain auditability and traceability of budget changes.
    /// </summary>
    public class BudgetExpenseRemovedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget from which the expense was removed.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the associated project.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget affected.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the amount of the expense that was removed.
        /// </summary>
        public Money ExpenseAmount { get; }

        /// <summary>
        /// Gets the total amount spent in the budget after the expense was removed.
        /// </summary>
        public Money TotalSpent { get; }

        /// <summary>
        /// Gets the reason or explanation for the removal of the expense.
        /// This is important for audit trails and accountability.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetExpenseRemovedEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the affected budget.</param>
        /// <param name="projectId">The ID of the associated project.</param>
        /// <param name="category">The category of the budget.</param>
        /// <param name="expenseAmount">The amount of the expense that was removed.</param>
        /// <param name="totalSpent">The updated total spent after removal.</param>
        /// <param name="reason">The reason the expense was removed (e.g., "duplicate", "refund").</param>
        public BudgetExpenseRemovedEvent(
            Guid budgetId,
            Guid projectId,
            string category,
            Money expenseAmount,
            Money totalSpent,
            string reason)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            ExpenseAmount = expenseAmount;
            TotalSpent = totalSpent;
            Reason = reason;
        }
    }

}
