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
    /// Domain event raised when a new expense is added to a budget.
    /// Used to notify the system that spending has increased within a specific budget category.
    /// </summary>
    public class BudgetExpenseAddedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the budget the expense was added to.
        /// </summary>
        public Guid BudgetId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with this budget.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the category of the budget to which the expense was added.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the amount of the individual expense that was added.
        /// </summary>
        public Money ExpenseAmount { get; }

        /// <summary>
        /// Gets the new total amount spent after adding the expense.
        /// </summary>
        public Money TotalSpent { get; }

        /// <summary>
        /// Gets a description or label provided with the expense (e.g., "Vendor invoice #123").
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetExpenseAddedEvent"/> class.
        /// </summary>
        /// <param name="budgetId">The ID of the budget.</param>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="category">The category of the budget.</param>
        /// <param name="expenseAmount">The amount of the expense that was added.</param>
        /// <param name="totalSpent">The updated total spent amount.</param>
        /// <param name="description">An optional description of the expense.</param>
        public BudgetExpenseAddedEvent(
            Guid budgetId,
            Guid projectId,
            string category,
            Money expenseAmount,
            Money totalSpent,
            string description)
        {
            BudgetId = budgetId;
            ProjectId = projectId;
            Category = category;
            ExpenseAmount = expenseAmount;
            TotalSpent = totalSpent;
            Description = description;
        }
    }

}
