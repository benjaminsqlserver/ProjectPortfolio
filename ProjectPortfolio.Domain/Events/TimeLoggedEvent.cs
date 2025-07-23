using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event that is raised when a new time entry is successfully logged.
    /// Used to notify other parts of the system (e.g., event handlers, integrations, auditing)
    /// that time has been recorded by a team member for a work item.
    /// </summary>
    public class TimeLoggedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the time entry that was logged.
        /// </summary>
        public Guid TimeEntryId { get; }

        /// <summary>
        /// Gets the identifier of the work item associated with the time entry.
        /// </summary>
        public Guid WorkItemId { get; }

        /// <summary>
        /// Gets the identifier of the team member who logged the time.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the number of hours that were logged.
        /// </summary>
        public decimal HoursLogged { get; }

        /// <summary>
        /// Gets the date the time was logged for.
        /// </summary>
        public DateTime LoggedDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeLoggedEvent"/> class with
        /// the specified time entry details.
        /// </summary>
        /// <param name="timeEntryId">The ID of the newly created time entry.</param>
        /// <param name="workItemId">The ID of the associated work item.</param>
        /// <param name="teamMemberId">The ID of the team member who logged time.</param>
        /// <param name="hoursLogged">The number of hours logged.</param>
        /// <param name="loggedDate">The date for which the time was logged.</param>
        public TimeLoggedEvent(Guid timeEntryId, Guid workItemId, Guid teamMemberId, decimal hoursLogged, DateTime loggedDate)
        {
            TimeEntryId = timeEntryId;
            WorkItemId = workItemId;
            TeamMemberId = teamMemberId;
            HoursLogged = hoursLogged;
            LoggedDate = loggedDate;
        }
    }

}
