using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when a project is cancelled
    public class ProjectCancelledEvent : BaseDomainEvent
    {
        // Unique identifier of the cancelled project
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code of the project
        public string ProjectCode { get; }

        // Reason provided for the project's cancellation
        public string Reason { get; }

        // Timestamp when the project was cancelled
        public DateTime CancelledDate { get; }

        // Progress percentage at the time of cancellation
        public int ProgressAtCancellation { get; }

        // Constructor to initialize all relevant data when a project is cancelled
        public ProjectCancelledEvent(
            Guid projectId,
            string projectName,
            string projectCode,
            string reason,
            int progressAtCancellation)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            Reason = reason;
            CancelledDate = DateTime.UtcNow; // Set cancellation time to now (UTC)
            ProgressAtCancellation = progressAtCancellation;
        }
    }
}
