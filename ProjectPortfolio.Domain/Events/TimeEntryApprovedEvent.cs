using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event raised when a time entry is approved.
    /// This event can be used to notify other parts of the system (e.g., audit log, billing process, reporting)
    /// that the time entry has transitioned to an approved state.
    /// </summary>
    public class TimeEntryApprovedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the ID of the approved time entry.
        /// </summary>
        public Guid TimeEntryId { get; }

        /// <summary>
        /// Gets the ID of the work item associated with the time entry.
        /// </summary>
        public Guid WorkItemId { get; }

        /// <summary>
        /// Gets the ID of the team member who originally logged the time.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the number of hours that were approved.
        /// </summary>
        public decimal HoursLogged { get; }

        /// <summary>
        /// Gets the ID of the user who approved the time entry.
        /// </summary>
        public Guid ApprovedBy { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeEntryApprovedEvent"/> class with
        /// the relevant approval details.
        /// </summary>
        /// <param name="timeEntryId">The ID of the time entry being approved.</param>
        /// <param name="workItemId">The ID of the associated work item.</param>
        /// <param name="teamMemberId">The ID of the team member who logged the time.</param>
        /// <param name="hoursLogged">The number of hours logged and now approved.</param>
        /// <param name="approvedBy">The ID of the user who approved the time entry.</param>
        public TimeEntryApprovedEvent(
            Guid timeEntryId,
            Guid workItemId,
            Guid teamMemberId,
            decimal hoursLogged,
            Guid approvedBy)
        {
            TimeEntryId = timeEntryId;
            WorkItemId = workItemId;
            TeamMemberId = teamMemberId;
            HoursLogged = hoursLogged;
            ApprovedBy = approvedBy;
        }
    }

}
