using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;

namespace ProjectPortfolio.Domain.Entities
{
    /// <summary>
    /// Represents a task or unit of work within a project.
    /// </summary>
    public class WorkItem : AggregateRoot
    {
        // --- Basic Info ---

        /// <summary>Title of the work item.</summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>Detailed description of the work item.</summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>Priority level (Low, Medium, High, Critical).</summary>
        public Priority Priority { get; private set; }

        /// <summary>Current status of the work item (New, InProgress, Completed, etc.).</summary>
        public WorkItemStatus Status { get; private set; }

        // --- Relationships ---

        /// <summary>Identifier of the project to which this work item belongs.</summary>
        public Guid ProjectId { get; private set; }

        /// <summary>Team member ID to whom the work item is assigned.</summary>
        public Guid? AssignedToId { get; private set; }

        /// <summary>Full name of the assigned team member (redundant for display).</summary>
        public string? AssignedToName { get; private set; }

        // --- Timeline ---

        /// <summary>When the work item was started.</summary>
        public DateTime? StartDate { get; private set; }

        /// <summary>Expected due date for the work item.</summary>
        public DateTime? DueDate { get; private set; }

        /// <summary>Actual completion date.</summary>
        public DateTime? CompletedDate { get; private set; }

        // --- Effort ---

        /// <summary>Estimated effort in hours.</summary>
        public decimal EstimatedHours { get; private set; }

        /// <summary>Actual hours logged.</summary>
        public decimal ActualHours { get; private set; }

        // --- Progress ---

        /// <summary>Progress percentage (0-100).</summary>
        public int ProgressPercentage { get; private set; }

        // --- Hierarchical Relationships ---

        /// <summary>ID of the parent work item if this is a sub-task.</summary>
        public Guid? ParentWorkItemId { get; private set; }

        private readonly List<Guid> _childWorkItemIds = new();

        /// <summary>Child work item IDs.</summary>
        public IReadOnlyList<Guid> ChildWorkItemIds => _childWorkItemIds.AsReadOnly();

        // --- Metadata ---

        /// <summary>Comma-separated tags used for categorization or filtering.</summary>
        public string Tags { get; private set; } = string.Empty;

        // --- Constructors ---

        /// <summary>Private constructor for EF Core.</summary>
        private WorkItem() : base() { }

        /// <summary>Factory method to create a new work item.</summary>
        public static WorkItem Create(
            string title,
            string description,
            Guid projectId,
            Priority priority = Priority.Medium,
            decimal estimatedHours = 0,
            DateTime? dueDate = null,
            Guid? parentWorkItemId = null,
            string tags = "")
        {
            var workItem = new WorkItem();

            workItem.SetBasicInfo(title, description);
            workItem.ProjectId = projectId;
            workItem.Priority = priority;
            workItem.Status = WorkItemStatus.New;
            workItem.EstimatedHours = estimatedHours;
            workItem.DueDate = dueDate;
            workItem.ParentWorkItemId = parentWorkItemId;
            workItem.Tags = tags?.Trim() ?? string.Empty;
            workItem.ProgressPercentage = 0;

            return workItem;
        }

        // --- Business Logic Methods ---

        public void SetBasicInfo(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Work item title cannot be empty", nameof(title));

            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
        }

        public void SetPriority(Priority priority)
        {
            Priority = priority;
        }

        public void SetEstimation(decimal estimatedHours)
        {
            if (estimatedHours < 0)
                throw new ArgumentException("Estimated hours cannot be negative", nameof(estimatedHours));

            EstimatedHours = estimatedHours;
        }

        public void SetDueDate(DateTime? dueDate)
        {
            if (dueDate.HasValue && dueDate.Value < DateTime.Today)
                throw new ArgumentException("Due date cannot be in the past", nameof(dueDate));

            DueDate = dueDate;
        }

        public void AssignTo(Guid teamMemberId, string teamMemberName)
        {
            if (teamMemberId == Guid.Empty)
                throw new ArgumentException("Team member ID cannot be empty", nameof(teamMemberId));

            if (string.IsNullOrWhiteSpace(teamMemberName))
                throw new ArgumentException("Team member name cannot be empty", nameof(teamMemberName));

            AssignedToId = teamMemberId;
            AssignedToName = teamMemberName.Trim();
        }

        public void Unassign()
        {
            AssignedToId = null;
            AssignedToName = null;
        }

        public void StartWork()
        {
            if (Status != WorkItemStatus.New && Status != WorkItemStatus.Ready)
                throw new InvalidOperationException("Only new or ready work items can be started");

            Status = WorkItemStatus.InProgress;
            StartDate = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status == WorkItemStatus.Completed)
                throw new InvalidOperationException("Work item is already completed");

            Status = WorkItemStatus.Completed;
            CompletedDate = DateTime.UtcNow;
            ProgressPercentage = 100;
        }

        public void Block(string reason)
        {
            if (Status == WorkItemStatus.Completed)
                throw new InvalidOperationException("Completed work items cannot be blocked");

            Status = WorkItemStatus.Blocked;
            // Optional: store the block reason in a future implementation
        }

        public void Unblock()
        {
            if (Status != WorkItemStatus.Blocked)
                throw new InvalidOperationException("Only blocked work items can be unblocked");

            Status = WorkItemStatus.Ready;
        }

        public void UpdateProgress(int progressPercentage)
        {
            if (progressPercentage < 0 || progressPercentage > 100)
                throw new ArgumentException("Progress percentage must be between 0 and 100", nameof(progressPercentage));

            ProgressPercentage = progressPercentage;

            if (progressPercentage == 100 && Status != WorkItemStatus.Completed)
            {
                Complete();
            }
        }

        public void LogWork(decimal hours)
        {
            if (hours < 0)
                throw new ArgumentException("Hours cannot be negative", nameof(hours));

            ActualHours += hours;
        }

        public void AddChildWorkItem(Guid childWorkItemId)
        {
            if (childWorkItemId == Guid.Empty)
                throw new ArgumentException("Child work item ID cannot be empty", nameof(childWorkItemId));

            if (!_childWorkItemIds.Contains(childWorkItemId))
            {
                _childWorkItemIds.Add(childWorkItemId);
            }
        }

        public void RemoveChildWorkItem(Guid childWorkItemId)
        {
            _childWorkItemIds.Remove(childWorkItemId);
        }

        public void SetTags(string tags)
        {
            Tags = tags?.Trim() ?? string.Empty;
        }

        // --- Computed Properties ---

        /// <summary>Returns true if the item is overdue and not completed.</summary>
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Today && Status != WorkItemStatus.Completed;

        /// <summary>Returns true if the item is currently assigned to someone.</summary>
        public bool IsAssigned => AssignedToId.HasValue;

        /// <summary>Returns true if actual effort exceeds the estimated effort.</summary>
        public bool IsOverEstimate => ActualHours > EstimatedHours && EstimatedHours > 0;

        /// <summary>Difference between estimated and actual hours.</summary>
        public decimal HoursVariance => EstimatedHours - ActualHours;

        /// <summary>Returns true if the item has child tasks.</summary>
        public bool HasChildren => _childWorkItemIds.Count > 0;

        /// <summary>Duration between start and completion (if both are set).</summary>
        public TimeSpan? Duration => CompletedDate.HasValue && StartDate.HasValue
            ? CompletedDate.Value - StartDate.Value
            : null;
    }
}
