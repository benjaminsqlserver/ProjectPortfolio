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
    /// Domain event that represents the reduction of a resource allocation period.
    /// </summary>
    public class ResourceAllocationReducedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the resource allocation.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Gets the identifier of the associated project.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the identifier of the team member whose allocation was reduced.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the original allocation period before reduction.
        /// </summary>
        public DateRange PreviousPeriod { get; }

        /// <summary>
        /// Gets the updated allocation period after reduction.
        /// </summary>
        public DateRange NewPeriod { get; }

        /// <summary>
        /// Gets the reason provided for the reduction, if any.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAllocationReducedEvent"/> class.
        /// </summary>
        /// <param name="allocationId">The unique identifier of the allocation.</param>
        /// <param name="projectId">The project ID the allocation is related to.</param>
        /// <param name="teamMemberId">The team member affected by the change.</param>
        /// <param name="previousPeriod">The previous date range of the allocation.</param>
        /// <param name="newPeriod">The updated date range after the reduction.</param>
        /// <param name="reason">Optional reason for the change, used for auditing and traceability.</param>
        public ResourceAllocationReducedEvent(
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
