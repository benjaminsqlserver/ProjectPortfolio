using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when a project is put on hold
    public class ProjectPutOnHoldEvent : BaseDomainEvent
    {
        // Unique identifier of the project that was put on hold
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code of the project
        public string ProjectCode { get; }

        // Reason for putting the project on hold
        public string Reason { get; }

        // Timestamp of when the project was put on hold
        public DateTime PutOnHoldDate { get; }

        // Constructor to initialize the event with relevant details
        public ProjectPutOnHoldEvent(
            Guid projectId,
            string projectName,
            string projectCode,
            string reason)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            Reason = reason;
            PutOnHoldDate = DateTime.UtcNow; // Set the hold date to the current UTC time
        }
    }
}
