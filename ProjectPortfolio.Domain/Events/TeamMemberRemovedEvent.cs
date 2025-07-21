using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when a team member is removed from a project
    public class TeamMemberRemovedEvent : BaseDomainEvent
    {
        // Unique identifier of the project from which the member was removed
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique identifier of the team member who was removed
        public Guid TeamMemberId { get; }

        // Constructor to initialize the event with relevant details
        public TeamMemberRemovedEvent(
            Guid projectId,
            string projectName,
            Guid teamMemberId)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            TeamMemberId = teamMemberId;
        }
    }
}
