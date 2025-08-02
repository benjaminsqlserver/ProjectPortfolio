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
    /// Domain event raised when a resource (team member) is allocated to a project
    /// at a percentage exceeding 100%, indicating a potential overallocation.
    /// </summary>
    public class ResourceOverallocationDetectedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the resource allocation that triggered the event.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with the overallocation.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the unique identifier of the team member who is overallocated.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the percentage of allocation that caused the overallocation.
        /// Values greater than 100% indicate the overcommitment.
        /// </summary>
        public decimal AllocationPercentage { get; }

        /// <summary>
        /// Gets the date range over which the overallocated assignment applies.
        /// </summary>
        public DateRange AllocationPeriod { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOverallocationDetectedEvent"/> class.
        /// </summary>
        /// <param name="allocationId">The ID of the allocation where overallocation was detected.</param>
        /// <param name="projectId">The ID of the project associated with the allocation.</param>
        /// <param name="teamMemberId">The ID of the team member who is overallocated.</param>
        /// <param name="allocationPercentage">The overallocated percentage (must be greater than 100%).</param>
        /// <param name="allocationPeriod">The period during which the overallocation occurs.</param>
        public ResourceOverallocationDetectedEvent(
            Guid allocationId,
            Guid projectId,
            Guid teamMemberId,
            decimal allocationPercentage,
            DateRange allocationPeriod)
        {
            AllocationId = allocationId;
            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            AllocationPercentage = allocationPercentage;
            AllocationPeriod = allocationPeriod;
        }
    }

}
