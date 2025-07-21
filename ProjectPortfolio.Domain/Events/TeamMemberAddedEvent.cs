using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when a team member is added to a project
    public class TeamMemberAddedEvent : BaseDomainEvent
    {
        // Unique identifier of the project to which the member was added
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique identifier of the team member who was added
        public Guid TeamMemberId { get; }

        // Constructor to initialize the event with relevant details
        public TeamMemberAddedEvent(
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
