using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;

namespace ProjectPortfolio.Domain.Entities
{
    public class Milestone : BaseEntity
    {
        // Basic details
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        // Foreign key to the related project
        public Guid ProjectId { get; private set; }

        // Timeline information
        public DateTime PlannedDate { get; private set; }
        public DateTime? ActualDate { get; private set; }

        // Status of the milestone (Planned, Approved, Completed, etc.)
        public MilestoneStatus Status { get; private set; }

        // Work items linked to this milestone
        private readonly List<Guid> _workItemIds = new();
        public IReadOnlyList<Guid> WorkItemIds => _workItemIds.AsReadOnly();

        // Deliverables for the milestone
        public string Deliverables { get; private set; } = string.Empty;

        // Approval tracking
        public bool RequiresApproval { get; private set; }
        public Guid? ApprovedById { get; private set; }
        public DateTime? ApprovedDate { get; private set; }

        // Private constructor for EF Core and factory enforcement
        private Milestone() : base() { }

        // Factory method to create a new milestone with validations and initial defaults
        public static Milestone Create(
            string name,
            string description,
            Guid projectId,
            DateTime plannedDate,
            string deliverables = "",
            bool requiresApproval = false)
        {
            var milestone = new Milestone();

            milestone.SetBasicInfo(name, description);
            milestone.ProjectId = projectId;
            milestone.SetPlannedDate(plannedDate);
            milestone.Deliverables = deliverables?.Trim() ?? string.Empty;
            milestone.RequiresApproval = requiresApproval;
            milestone.Status = MilestoneStatus.Planned;

            return milestone;
        }

        // Updates name and description with validation
        public void SetBasicInfo(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Milestone name cannot be empty", nameof(name));

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
        }

        // Updates planned milestone date (must not be in the past)
        public void SetPlannedDate(DateTime plannedDate)
        {
            if (plannedDate < DateTime.Today)
                throw new ArgumentException("Planned date cannot be in the past", nameof(plannedDate));

            PlannedDate = plannedDate;
        }

        // Updates deliverables field
        public void SetDeliverables(string deliverables)
        {
            Deliverables = deliverables?.Trim() ?? string.Empty;
        }

        // Marks milestone as completed (only allowed if approved when required)
        public void Complete(DateTime? actualDate = null)
        {
            if (Status == MilestoneStatus.Completed)
                throw new InvalidOperationException("Milestone is already completed");

            if (RequiresApproval && !ApprovedById.HasValue)
                throw new InvalidOperationException("Milestone requires approval before completion");

            Status = MilestoneStatus.Completed;
            ActualDate = actualDate ?? DateTime.UtcNow;
        }

        // Approves the milestone if approval is required
        public void Approve(Guid approvedById)
        {
            if (!RequiresApproval)
                throw new InvalidOperationException("This milestone does not require approval");

            if (approvedById == Guid.Empty)
                throw new ArgumentException("Approved by ID cannot be empty", nameof(approvedById));

            ApprovedById = approvedById;
            ApprovedDate = DateTime.UtcNow;
            Status = MilestoneStatus.Approved;
        }

        // Marks the milestone as missed if not completed
        public void Miss()
        {
            if (Status == MilestoneStatus.Completed)
                throw new InvalidOperationException("Completed milestones cannot be marked as missed");

            Status = MilestoneStatus.Missed;
        }

        // Adds a work item to the milestone (by ID)
        public void AddWorkItem(Guid workItemId)
        {
            if (workItemId == Guid.Empty)
                throw new ArgumentException("Work item ID cannot be empty", nameof(workItemId));

            if (!_workItemIds.Contains(workItemId))
            {
                _workItemIds.Add(workItemId);
            }
        }

        // Removes a work item from the milestone
        public void RemoveWorkItem(Guid workItemId)
        {
            _workItemIds.Remove(workItemId);
        }

        // Computed property: whether milestone is overdue and incomplete
        public bool IsOverdue => DateTime.Today > PlannedDate && Status != MilestoneStatus.Completed;

        // Computed property: whether milestone has been approved
        public bool IsApproved => ApprovedById.HasValue;

        // Computed property: days remaining until the planned date
        public int DaysUntilDue => (PlannedDate - DateTime.Today).Days;

        // Computed property: how many days it’s overdue
        public int DaysOverdue => Status != MilestoneStatus.Completed && DateTime.Today > PlannedDate
            ? (DateTime.Today - PlannedDate).Days
            : 0;

        // Computed property: whether milestone was finished on time
        public bool WasOnTime => ActualDate.HasValue && ActualDate.Value <= PlannedDate;
    }
}
