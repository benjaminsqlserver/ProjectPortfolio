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
    /// Domain event triggered when a previously deactivated resource allocation is reactivated.
    /// Used for tracking changes in resource assignment state.
    /// </summary>
    public class ResourceAllocationReactivatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Unique identifier for the allocation that was reactivated.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Identifier of the project to which the resource is allocated.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Identifier of the team member whose allocation was reactivated.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Percentage of the team member's time allocated to the project.
        /// </summary>
        public decimal AllocationPercentage { get; }

        /// <summary>
        /// The time period for which the allocation is valid.
        /// </summary>
        public DateRange AllocationPeriod { get; }

        /// <summary>
        /// Optional reason provided for reactivation, useful for auditing or reporting.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAllocationReactivatedEvent"/> class.
        /// </summary>
        /// <param name="allocationId">Unique identifier of the allocation.</param>
        /// <param name="projectId">The project the resource is assigned to.</param>
        /// <param name="teamMemberId">The resource (team member) being reactivated.</param>
        /// <param name="allocationPercentage">The percentage of allocation.</param>
        /// <param name="allocationPeriod">The time range of the allocation.</param>
        /// <param name="reason">Optional reason for reactivation.</param>
        public ResourceAllocationReactivatedEvent(
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
