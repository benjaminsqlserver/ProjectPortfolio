using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when a project is resumed after being on hold
    public class ProjectResumedEvent : BaseDomainEvent
    {
        // Unique identifier of the resumed project
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code of the project
        public string ProjectCode { get; }

        // Timestamp indicating when the project was resumed
        public DateTime ResumedDate { get; }

        // Constructor to initialize the event with relevant project data
        public ProjectResumedEvent(
            Guid projectId,
            string projectName,
            string projectCode)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            ResumedDate = DateTime.UtcNow; // Automatically set to current UTC time
        }
    }
}
