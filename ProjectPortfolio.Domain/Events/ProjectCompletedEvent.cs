using ProjectPortfolio.Domain.Common;

namespace ProjectPortfolio.Domain.Events
{
    // Domain event raised when a project is completed
    public class ProjectCompletedEvent : BaseDomainEvent
    {
        // Unique identifier of the completed project
        public Guid ProjectId { get; }

        // Name of the project
        public string ProjectName { get; }

        // Unique code of the project
        public string ProjectCode { get; }

        // The planned end date that was originally set
        public DateTime PlannedEndDate { get; }

        // The actual date on which the project was completed
        public DateTime ActualEndDate { get; }

        // Budget that was allocated for the project
        public decimal Budget { get; }

        // Total actual cost incurred by the project
        public decimal ActualCost { get; }

        // Indicates whether the project was completed on or before the planned date
        public bool WasOnTime { get; }

        // Indicates whether the project stayed within the allocated budget
        public bool WasOnBudget { get; }

        // Constructor to initialize event details and compute outcome flags
        public ProjectCompletedEvent(
            Guid projectId,
            string projectName,
            string projectCode,
            DateTime plannedEndDate,
            DateTime actualEndDate,
            decimal budget,
            decimal actualCost)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectCode = projectCode;
            PlannedEndDate = plannedEndDate;
            ActualEndDate = actualEndDate;
            Budget = budget;
            ActualCost = actualCost;

            // Compute whether the project was completed on time and within budget
            WasOnTime = actualEndDate <= plannedEndDate;
            WasOnBudget = actualCost <= budget;
        }
    }
}
