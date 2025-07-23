using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>.
    /// Domain event raised when a time entry is rejected.
    /// This event captures key rejection information such as who rejected the entry,
    /// the reason provided, and the amount of time that was not accepted.
    /// It can be used for auditing, notifications, and workflow adjustments.
    /// </summary>
    public class TimeEntryRejectedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the ID of the time entry that was rejected.
        /// </summary>
        public Guid TimeEntryId { get; }

        /// <summary>
        /// Gets the ID of the work item associated with the rejected time entry.
        /// </summary>
        public Guid WorkItemId { get; }

        /// <summary>
        /// Gets the ID of the team member who submitted the time entry.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the number of hours that were logged and subsequently rejected.
        /// </summary>
        public decimal HoursLogged { get; }

        /// <summary>
        /// Gets the ID of the user who performed the rejection.
        /// </summary>
        public Guid RejectedBy { get; }

        /// <summary>
        /// Gets the reason provided for rejecting the time entry.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeEntryRejectedEvent"/> class,
        /// encapsulating all relevant details about the rejection.
        /// </summary>
        /// <param name="timeEntryId">The ID of the rejected time entry.</param>
        /// <param name="workItemId">The ID of the associated work item.</param>
        /// <param name="teamMemberId">The ID of the team member who submitted the time entry.</param>
        /// <param name="hoursLogged">The number of hours logged in the rejected entry.</param>
        /// <param name="rejectedBy">The ID of the person who rejected the time entry.</param>
        /// <param name="reason">The reason why the time entry was rejected.</param>
        public TimeEntryRejectedEvent(
            Guid timeEntryId,
            Guid workItemId,
            Guid teamMemberId,
            decimal hoursLogged,
            Guid rejectedBy,
            string reason)
        {
            TimeEntryId = timeEntryId;
            WorkItemId = workItemId;
            TeamMemberId = teamMemberId;
            HoursLogged = hoursLogged;
            RejectedBy = rejectedBy;
            Reason = reason;
        }
    }

}
