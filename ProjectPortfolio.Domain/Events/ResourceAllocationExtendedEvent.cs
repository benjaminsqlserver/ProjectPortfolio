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
    /// Domain event raised when an existing resource allocation is extended to a new end date,
    /// modifying the original allocation period.
    /// </summary>
    public class ResourceAllocationExtendedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the allocation that was extended.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with the allocation.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the unique identifier of the team member whose allocation was extended.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the original date range of the allocation before the extension.
        /// </summary>
        public DateRange PreviousPeriod { get; }

        /// <summary>
        /// Gets the new date range of the allocation after the extension.
        /// </summary>
        public DateRange NewPeriod { get; }

        /// <summary>
        /// Gets the reason provided for extending the allocation, if any.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAllocationExtendedEvent"/> class.
        /// </summary>
        /// <param name="allocationId">The ID of the allocation being extended.</param>
        /// <param name="projectId">The ID of the related project.</param>
        /// <param name="teamMemberId">The ID of the team member involved.</param>
        /// <param name="previousPeriod">The original allocation date range.</param>
        /// <param name="newPeriod">The updated allocation date range.</param>
        /// <param name="reason">The reason for the extension.</param>
        public ResourceAllocationExtendedEvent(
            Guid allocationId,
            Guid projectId,
            Guid teamMemberId,
            DateRange previousPeriod,
            DateRange newPeriod,
            string reason)
        {
            AllocationId = allocationId;
            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            PreviousPeriod = previousPeriod;
            NewPeriod = newPeriod;
            Reason = reason;
        }
    }

}
