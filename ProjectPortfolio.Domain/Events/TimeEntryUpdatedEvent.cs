using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event raised when a time entry's logged hours are modified.
    /// This event provides the old and new hour values to support auditing, notification,
    /// and downstream recalculations (e.g., budget, resource allocation).
    /// </summary>
    public class TimeEntryUpdatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the updated time entry.
        /// </summary>
        public Guid TimeEntryId { get; }

        /// <summary>
        /// Gets the identifier of the work item associated with the time entry.
        /// </summary>
        public Guid WorkItemId { get; }

        /// <summary>
        /// Gets the identifier of the team member who owns the time entry.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the value of the hours that were originally logged before the update.
        /// </summary>
        public decimal PreviousHours { get; }

        /// <summary>
        /// Gets the new value of the logged hours after the update.
        /// </summary>
        public decimal NewHours { get; }

        /// <summary>
        /// Gets the date that the time was originally logged for.
        /// </summary>
        public DateTime LoggedDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeEntryUpdatedEvent"/> class with all relevant update information.
        /// </summary>
        /// <param name="timeEntryId">The ID of the time entry that was updated.</param>
        /// <param name="workItemId">The ID of the associated work item.</param>
        /// <param name="teamMemberId">The ID of the team member who logged the time.</param>
        /// <param name="previousHours">The original hours before the update.</param>
        /// <param name="newHours">The new hours after the update.</param>
        /// <param name="loggedDate">The date for which time was logged.</param>
        public TimeEntryUpdatedEvent(
            Guid timeEntryId,
            Guid workItemId,
            Guid teamMemberId,
            decimal previousHours,
            decimal newHours,
            DateTime loggedDate)
        {
            TimeEntryId = timeEntryId;
            WorkItemId = workItemId;
            TeamMemberId = teamMemberId;
            PreviousHours = previousHours;
            NewHours = newHours;
            LoggedDate = loggedDate;
        }
    }

}
