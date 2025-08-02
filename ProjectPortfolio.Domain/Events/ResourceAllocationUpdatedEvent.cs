using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event that represents an update to an existing resource allocation.
    /// This event is typically raised when the allocation percentage is changed,
    /// allowing for audit tracking, notifications, or downstream processing.
    /// </summary>
    public class ResourceAllocationUpdatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the allocation that was updated.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Gets the unique identifier of the project associated with the allocation.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the unique identifier of the team member whose allocation was changed.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// Gets the previous allocation percentage before the update.
        /// </summary>
        public decimal PreviousPercentage { get; }

        /// <summary>
        /// Gets the new allocation percentage after the update.
        /// </summary>
        public decimal NewPercentage { get; }

        /// <summary>
        /// Gets the reason provided for the allocation change.
        /// This field may be empty if no reason was recorded.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAllocationUpdatedEvent"/> class
        /// with details about the change in allocation.
        /// </summary>
        /// <param name="allocationId">The ID of the updated allocation.</param>
        /// <param name="projectId">The project the allocation belongs to.</param>
        /// <param name="teamMemberId">The ID of the affected team member.</param>
        /// <param name="previousPercentage">The old allocation percentage.</param>
        /// <param name="newPercentage">The updated allocation percentage.</param>
        /// <param name="reason">The reason for the change (optional).</param>
        public ResourceAllocationUpdatedEvent(
            Guid allocationId,
            Guid projectId,
            Guid teamMemberId,
            decimal previousPercentage,
            decimal newPercentage,
            string reason)
        {
            AllocationId = allocationId;
            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            PreviousPercentage = previousPercentage;
            NewPercentage = newPercentage;
            Reason = reason;
        }
    }

}
