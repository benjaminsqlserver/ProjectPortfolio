using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    /// <summary>
    /// Domain event raised when a team member's role in a resource allocation is updated.
    /// </summary>
    public class ResourceRoleUpdatedEvent : BaseDomainEvent
    {
        /// <summary>
        /// Unique identifier of the allocation that was updated.
        /// </summary>
        public Guid AllocationId { get; }

        /// <summary>
        /// Identifier of the project to which the allocation belongs.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Identifier of the team member whose role was changed.
        /// </summary>
        public Guid TeamMemberId { get; }

        /// <summary>
        /// The role previously assigned to the team member.
        /// </summary>
        public string PreviousRole { get; }

        /// <summary>
        /// The new role now assigned to the team member.
        /// </summary>
        public string NewRole { get; }

        /// <summary>
        /// The reason provided for the role change, if any.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ResourceRoleUpdatedEvent"/> class.
        /// </summary>
        /// <param name="allocationId">The ID of the affected allocation.</param>
        /// <param name="projectId">The ID of the associated project.</param>
        /// <param name="teamMemberId">The ID of the team member affected by the role change.</param>
        /// <param name="previousRole">The original role before the update.</param>
        /// <param name="newRole">The updated role after the change.</param>
        /// <param name="reason">The reason or context for the change.</param>
        public ResourceRoleUpdatedEvent(
            Guid allocationId,
            Guid projectId,
            Guid teamMemberId,
            string previousRole,
            string newRole,
            string reason)
        {
            AllocationId = allocationId;
            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            PreviousRole = previousRole;
            NewRole = newRole;
            Reason = reason;
        }
    }

}
