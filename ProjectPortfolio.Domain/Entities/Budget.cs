using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;
using ProjectPortfolio.Domain.Events;
using ProjectPortfolio.Domain.Exceptions;
using ProjectPortfolio.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Entities
{
    /// <summary>
    /// Represents a budget allocated to a project, tracking allocation, expenses, and status.
    /// </summary>
    public class Budget : AggregateRoot
    {
        // ---------- Core Properties ----------

        /// <summary>Identifier of the related project.</summary>
        public Guid ProjectId { get; private set; }

        /// <summary>Category or type of the budget (e.g., Marketing, Development).</summary>
        public string Category { get; private set; }

        /// <summary>Amount of money allocated for this budget.</summary>
        public Money Allocated { get; private set; }

        /// <summary>Amount of money already spent from the allocated budget.</summary>
        public Money Spent { get; private set; }

        /// <summary>Timestamp when the budget was created.</summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>Timestamp when the budget was last updated (nullable).</summary>
        public DateTime? LastUpdatedAt { get; private set; }

        /// <summary>Optional notes or description related to the budget.</summary>
        public string Notes { get; private set; } = string.Empty;

        /// <summary>Status of the budget (Active, Frozen, or Closed).</summary>
        public BudgetStatus Status { get; private set; }

        // ---------- Navigation Properties ----------

        /// <summary>Reference to the associated Project entity.</summary>
        public virtual Project Project { get; private set; } = null!;

        // ---------- Computed Properties ----------

        /// <summary>Returns the remaining budget (Allocated - Spent).</summary>
        public Money Remaining => Allocated - Spent;

        /// <summary>Returns the variance (overspending) amount (Spent - Allocated).</summary>
        public Money Variance => Spent - Allocated;

        /// <summary>Returns the percentage by which the budget is over or under.</summary>
        public decimal VariancePercentage => Allocated.Amount == 0 ? 0 : Variance.GetVariance(Allocated);

        /// <summary>Indicates whether the budget has been exceeded.</summary>
        public bool IsOverBudget => Spent > Allocated;

        /// <summary>Indicates whether 90% or more of the budget has been spent.</summary>
        public bool IsNearingBudgetLimit => GetUtilizationPercentage() >= 90;

        /// <summary>Indicates whether the entire budget has been spent or exceeded.</summary>
        public bool IsBudgetExhausted => Spent >= Allocated;

        // ---------- Constructor (Private for EF Core) ----------

        private Budget() { }

        // ---------- Public Constructor ----------

        /// <summary>
        /// Initializes a new Budget instance.
        /// </summary>
        public Budget(Guid projectId, string category, Money allocatedAmount, string notes = "")
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new InvalidBudgetException("Budget category cannot be null or empty.");

            if (allocatedAmount.Amount <= 0)
                throw new InvalidBudgetException("Allocated amount must be greater than zero.");

            ProjectId = projectId;
            Category = category.Trim();
            Allocated = allocatedAmount;
            Spent = Money.Zero(allocatedAmount.Currency);
            CreatedAt = DateTime.UtcNow;
            Notes = notes ?? string.Empty;
            Status = BudgetStatus.Active;

            AddDomainEvent(new BudgetCreatedEvent(Id, ProjectId, Category, Allocated));
        }

        // ---------- Budget Operations ----------

        /// <summary>
        /// Adds an expense to the budget.
        /// </summary>
        public void AddExpense(Money amount, string description = "")
        {
            if (Status != BudgetStatus.Active)
                throw new InvalidBudgetException($"Cannot add expenses to a {Status} budget.");

            if (!Spent.SameCurrency(amount))
                throw new InvalidBudgetException($"Expense currency {amount.Currency} does not match budget currency {Spent.Currency}.");

            if (amount.Amount <= 0)
                throw new InvalidBudgetException("Expense amount must be greater than zero.");

            var previousSpent = Spent;
            var wasOverBudget = IsOverBudget;

            Spent = Spent.Add(amount);
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetExpenseAddedEvent(Id, ProjectId, Category, amount, Spent, description));

            // Raise event if newly over budget
            if (!wasOverBudget && IsOverBudget)
            {
                AddDomainEvent(new BudgetExceededEvent(Id, ProjectId, Category, Allocated, Spent, Variance));
            }

            // Raise event if nearing budget for the first time
            if (!wasOverBudget && IsNearingBudgetLimit)
            {
                AddDomainEvent(new BudgetNearingLimitEvent(Id, ProjectId, Category, Allocated, Spent, GetUtilizationPercentage()));
            }
        }

        /// <summary>
        /// Removes an expense from the budget (e.g., refund or correction).
        /// </summary>
        public void RemoveExpense(Money amount, string reason = "")
        {
            if (Status != BudgetStatus.Active)
                throw new InvalidBudgetException($"Cannot remove expenses from a {Status} budget.");

            if (!Spent.SameCurrency(amount))
                throw new InvalidBudgetException($"Expense currency {amount.Currency} does not match budget currency {Spent.Currency}.");

            if (amount.Amount <= 0)
                throw new InvalidBudgetException("Expense amount must be greater than zero.");

            if (Spent < amount)
                throw new InvalidBudgetException("Cannot remove more than the total spent amount.");

            Spent = Spent.Subtract(amount);
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetExpenseRemovedEvent(Id, ProjectId, Category, amount, Spent, reason));
        }

        /// <summary>
        /// Updates the allocated budget amount.
        /// </summary>
        public void UpdateAllocation(Money newAllocatedAmount, string reason = "")
        {
            if (Status != BudgetStatus.Active)
                throw new InvalidBudgetException($"Cannot update allocation for a {Status} budget.");

            if (!Allocated.SameCurrency(newAllocatedAmount))
                throw new InvalidBudgetException($"New allocation currency {newAllocatedAmount.Currency} does not match budget currency {Allocated.Currency}.");

            if (newAllocatedAmount.Amount <= 0)
                throw new InvalidBudgetException("Allocated amount must be greater than zero.");

            var previousAllocation = Allocated;
            Allocated = newAllocatedAmount;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetAllocationUpdatedEvent(Id, ProjectId, Category, previousAllocation, newAllocatedAmount, reason));

            // Raise alert if the new allocation is still over budget
            if (IsOverBudget)
            {
                AddDomainEvent(new BudgetExceededEvent(Id, ProjectId, Category, Allocated, Spent, Variance));
            }
        }

        /// <summary>Freezes the budget, preventing any further modifications.</summary>
        public void Freeze(string reason = "")
        {
            if (Status == BudgetStatus.Frozen)
                throw new InvalidBudgetException("Budget is already frozen.");

            Status = BudgetStatus.Frozen;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetFrozenEvent(Id, ProjectId, Category, reason));
        }

        /// <summary>Unfreezes the budget, allowing modifications again.</summary>
        public void Unfreeze(string reason = "")
        {
            if (Status != BudgetStatus.Frozen)
                throw new InvalidBudgetException("Budget is not frozen.");

            Status = BudgetStatus.Active;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetUnfrozenEvent(Id, ProjectId, Category, reason));
        }

        /// <summary>Closes the budget permanently.</summary>
        public void Close(string reason = "")
        {
            if (Status == BudgetStatus.Closed)
                throw new InvalidBudgetException("Budget is already closed.");

            Status = BudgetStatus.Closed;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetClosedEvent(Id, ProjectId, Category, Allocated, Spent, reason));
        }

        // ---------- Utility Methods ----------

        /// <summary>Calculates what percentage of the budget has been spent (capped at 100%).</summary>
        public decimal GetUtilizationPercentage()
        {
            if (Allocated.Amount == 0) return 0;
            return Math.Min((Spent.Amount / Allocated.Amount) * 100, 100);
        }

        /// <summary>Estimates the variance if additional expected expenses are incurred.</summary>
        public Money GetProjectedVariance(Money additionalExpectedExpenses)
        {
            if (!Spent.SameCurrency(additionalExpectedExpenses))
                throw new InvalidBudgetException("Additional expenses currency does not match budget currency.");

            var projectedSpent = Spent.Add(additionalExpectedExpenses);
            return projectedSpent.Subtract(Allocated);
        }

        /// <summary>Checks if a new expense can be added without exceeding the allocation.</summary>
        public bool CanAccommodateExpense(Money amount)
        {
            if (!Spent.SameCurrency(amount))
                return false;

            return Status == BudgetStatus.Active && (Spent.Add(amount) <= Allocated);
        }

        /// <summary>Updates the notes associated with the budget.</summary>
        public void UpdateNotes(string notes)
        {
            Notes = notes ?? string.Empty;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }

}
