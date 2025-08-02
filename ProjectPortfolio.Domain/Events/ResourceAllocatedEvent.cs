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
    /// Domain event that is raised when a resource (team member) is successfully allocated to a project.
    /// This event captures key data about the allocation such as the assigned percentage, 
    /// time period, and role of the team member.
    /// </summary>
    public class ResourceAllocatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the resource allocation.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Gets the unique identifier of the project to which the resource is allocated.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the unique identifier of the team member (resource) being allocated.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the percentage of time the team member is allocated to the project.
        /// A value of 100 represents full-time allocation.
        /// </summary>
        public decimal AllocationPercentage { get; }

        /// <summary>
        /// Gets the time range (start and end dates) during which the allocation is valid.
        /// </summary>
        public DateRange AllocationPeriod { get; }

        /// <summary>
        /// Gets the role or title that the team member will perform during the allocation.
        /// </summary>
        public string Role { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAllocatedEvent"/> class
        /// with the specified details of the allocation.
        /// </summary>
        /// <param name="allocationId">The unique identifier of the allocation.</param>
        /// <param name="projectId">The project to which the team member is assigned.</param>
        /// <param name="teamMemberId">The team member being allocated.</param>
        /// <param name="allocationPercentage">The percentage of effort allocated to the project.</param>
        /// <param name="allocationPeriod">The duration of the allocation.</param>
        /// <param name="role">The assigned role of the team member in the project.</param>
        public ResourceAllocatedEvent(
            Guid allocationId,
            Guid projectId,
            Guid teamMemberId,
            decimal allocationPercentage,
            DateRange allocationPeriod,
            string role)
        {
            AllocationId = allocationId;
            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            AllocationPercentage = allocationPercentage;
            AllocationPeriod = allocationPeriod;
            Role = role;
        }
    }

}
