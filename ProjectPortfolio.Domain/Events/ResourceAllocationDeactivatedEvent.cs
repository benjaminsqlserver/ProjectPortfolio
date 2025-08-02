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
    /// Domain event raised when a resource allocation is deactivated.
    /// </summary>
    public class ResourceAllocationDeactivatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the allocation that was deactivated.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Gets the identifier of the project associated with the allocation.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the identifier of the team member who was allocated.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the percentage of time the resource was allocated.
        /// </summary>
        public decimal AllocationPercentage { get; }

        /// <summary>
        /// Gets the period during which the allocation was active.
        /// </summary>
        public DateRange AllocationPeriod { get; }

        /// <summary>
        /// Gets the reason for deactivating the allocation.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAllocationDeactivatedEvent"/> class.
        /// </summary>
        /// <param name="allocationId">The ID of the allocation.</param>
        /// <param name="projectId">The project to which the resource was assigned.</param>
        /// <param name="teamMemberId">The ID of the team member whose allocation was deactivated.</param>
        /// <param name="allocationPercentage">The percentage of the resource allocation.</param>
        /// <param name="allocationPeriod">The date range over which the allocation was valid.</param>
        /// <param name="reason">The reason for deactivation (can be empty).</param>
        public ResourceAllocationDeactivatedEvent(
            Guid allocationId,
            Guid projectId,
            Guid teamMemberId,
            decimal allocationPercentage,
            DateRange allocationPeriod,
            string reason)
        {
            AllocationId = allocationId;
            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            AllocationPercentage = allocationPercentage;
            AllocationPeriod = allocationPeriod;
            Reason = reason;
        }
    }

}
