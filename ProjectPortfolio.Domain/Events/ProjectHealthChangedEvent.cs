using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when the health status of a project changes
    public class ProjectHealthChangedEvent : BaseDomainEvent
    {
        // Unique identifier of the project whose health changed
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code of the project
        public string ProjectCode { get; }

        // The previous health status of the project
        public ProjectHealth PreviousHealth { get; }

        // The new health status of the project
        public ProjectHealth NewHealth { get; }

        // Constructor to initialize the event with health change details
        public ProjectHealthChangedEvent(
            Guid projectId,
            string projectName,
            string projectCode,
            ProjectHealth previousHealth,
            ProjectHealth newHealth)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            PreviousHealth = previousHealth;
            NewHealth = newHealth;
        }
    }
}
