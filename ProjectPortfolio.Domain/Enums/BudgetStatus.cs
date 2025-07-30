using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Enums
{

    /// <summary>
    /// Enum to represent the current state of a budget.
    /// </summary>
    public enum BudgetStatus
    {
        Active = 1,
        Frozen = 2,
        Closed = 3
    }
}
