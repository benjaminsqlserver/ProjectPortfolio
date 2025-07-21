using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event that is triggered when a new project is created
    public class ProjectCreatedEvent : BaseDomainEvent
    {
        // Unique identifier of the project
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code assigned to the project
        public string ProjectCode { get; }

        // ID of the project manager assigned to the project
        public Guid ProjectManagerId { get; }

        // Name of the project manager
        public string ProjectManagerName { get; }

        // Start date of the project
        public DateTime StartDate { get; }

        // Planned end date of the project
        public DateTime PlannedEndDate { get; }

        // Budget allocated for the project
        public decimal Budget { get; }

        // Priority level of the project
        public Priority Priority { get; }

        // Optional: name of the client associated with the project
        public string? ClientName { get; }

        // Constructor to initialize the event with all required data
        public ProjectCreatedEvent(
            Guid projectId,
            string projectName,
            string projectCode,
            Guid projectManagerId,
            string projectManagerName,
            DateTime startDate,
            DateTime plannedEndDate,
            decimal budget,
            Priority priority,
            string? clientName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            ProjectManagerId = projectManagerId;
            ProjectManagerName = projectManagerName;
            StartDate = startDate;
            PlannedEndDate = plannedEndDate;
            Budget = budget;
            Priority = priority;
            ClientName = clientName;
        }
    }
}
