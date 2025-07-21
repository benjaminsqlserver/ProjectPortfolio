using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event that is raised when a project is officially started
    public class ProjectStartedEvent : BaseDomainEvent
    {
        // Unique identifier of the project
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code assigned to the project
        public string ProjectCode { get; }

        // Identifier of the project manager who started the project
        public Guid ProjectManagerId { get; }

        // The official start date of the project
        public DateTime StartDate { get; }

        // Constructor to initialize the event with all relevant details
        public ProjectStartedEvent(
            Guid projectId,
            string projectName,
            string projectCode,
            Guid projectManagerId,
            DateTime startDate)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            ProjectManagerId = projectManagerId;
            StartDate = startDate;
        }
    }
}
