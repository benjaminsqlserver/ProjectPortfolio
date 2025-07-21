using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;
using ProjectPortfolio.Domain.Events;

namespace ProjectPortfolio.Domain.Entities
{
    // Represents a project aggregate root in the domain model
    public class Project : AggregateRoot
    {
        // Basic project info
        public string Name { get; private set; } = string.Empty;               // Project name
        public string Description { get; private set; } = string.Empty;        // Description of the project
        public string Code { get; private set; } = string.Empty;               // Unique project code

        // Status indicators
        public ProjectStatus Status { get; private set; }                      // Current project status (Planning, Active, etc.)
        public Priority Priority { get; private set; }                         // Priority level of the project
        public ProjectHealth Health { get; private set; }                      // Health/risk indicator (Green, Yellow, Red)

        // Timeline
        public DateTime StartDate { get; private set; }                        // Project start date
        public DateTime PlannedEndDate { get; private set; }                   // Expected end date
        public DateTime? ActualEndDate { get; private set; }                   // Actual end date (nullable)

        // Financials
        public decimal Budget { get; private set; }                            // Planned budget
        public decimal ActualCost { get; private set; }                        // Accumulated actual cost

        // Project manager information
        public Guid ProjectManagerId { get; private set; }                     // Assigned project manager ID
        public string ProjectManagerName { get; private set; } = string.Empty;// Project manager's name

        // Client/stakeholder info
        public string? ClientName { get; private set; }                        // Client name (nullable)
        public string? ClientContact { get; private set; }                     // Client contact info (nullable)

        // Progress
        public int ProgressPercentage { get; private set; }                    // Completion percentage (0–100)

        // Team members (as a list of GUIDs)
        private readonly List<Guid> _teamMemberIds = new();                   // Backing field for team members
        public IReadOnlyList<Guid> TeamMemberIds => _teamMemberIds.AsReadOnly(); // Public read-only view

        // Private constructor used by EF Core
        private Project() : base() { }

        // Factory method to create a new project with initial state
        public static Project Create(
            string name,
            string description,
            string code,
            DateTime startDate,
            DateTime plannedEndDate,
            decimal budget,
            Guid projectManagerId,
            string projectManagerName,
            Priority priority = Priority.Medium,
            string? clientName = null,
            string? clientContact = null)
        {
            var project = new Project();

            // Set up initial values
            project.SetBasicInfo(name, description, code);
            project.SetTimeline(startDate, plannedEndDate);
            project.SetBudget(budget);
            project.SetProjectManager(projectManagerId, projectManagerName);
            project.SetPriority(priority);
            project.SetClient(clientName, clientContact);

            // Default values
            project.Status = ProjectStatus.Planning;
            project.Health = ProjectHealth.Green;
            project.ProgressPercentage = 0;
            project.ActualCost = 0;

            // Add domain event - ProjectCreated

            project.AddDomainEvent(new ProjectCreatedEvent(
    project.Id,
    project.Name,
    project.Code,
    project.ProjectManagerId,
    project.ProjectManagerName,
    project.StartDate,
    project.PlannedEndDate,
    project.Budget,
    project.Priority,
    project.ClientName));



            return project;
        }

        // Sets basic identifying information
        public void SetBasicInfo(string name, string description, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Project code cannot be empty", nameof(code));

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Code = code.Trim().ToUpperInvariant();
        }

        // Sets the planned timeline for the project
        public void SetTimeline(DateTime startDate, DateTime plannedEndDate)
        {
            if (plannedEndDate <= startDate)
                throw new ArgumentException("Planned end date must be after start date");

            StartDate = startDate;
            PlannedEndDate = plannedEndDate;
        }

        // Sets the project's planned budget
        public void SetBudget(decimal budget)
        {
            if (budget < 0)
                throw new ArgumentException("Budget cannot be negative", nameof(budget));

            Budget = budget;
        }

        // Assigns a project manager to the project
        public void SetProjectManager(Guid projectManagerId, string projectManagerName)
        {
            if (projectManagerId == Guid.Empty)
                throw new ArgumentException("Project manager ID cannot be empty", nameof(projectManagerId));

            if (string.IsNullOrWhiteSpace(projectManagerName))
                throw new ArgumentException("Project manager name cannot be empty", nameof(projectManagerName));

            ProjectManagerId = projectManagerId;
            ProjectManagerName = projectManagerName.Trim();
        }

        // Sets the priority level
        public void SetPriority(Priority priority)
        {
            Priority = priority;
        }

        // Sets client/stakeholder info
        public void SetClient(string? clientName, string? clientContact)
        {
            ClientName = clientName?.Trim();
            ClientContact = clientContact?.Trim();
        }

        // Starts the project (only if it's in Planning state)
        public void StartProject()
        {
            if (Status != ProjectStatus.Planning)
                throw new InvalidOperationException("Only projects in Planning status can be started");

            Status = ProjectStatus.Active;

            //  Add domain event - ProjectStarted
            AddDomainEvent(new ProjectStartedEvent(Id, Name, Code, ProjectManagerId, StartDate));
        }

        // Puts the project on hold
        public void PutOnHold(string reason)
        {
            if (Status != ProjectStatus.Active)
                throw new InvalidOperationException("Only active projects can be put on hold");

            Status = ProjectStatus.OnHold;

            //Add domain event - ProjectPutOnHold

            AddDomainEvent(new ProjectPutOnHoldEvent(Id, Name, Code, reason));
        }

        // Resumes a paused project
        public void Resume()
        {
            if (Status != ProjectStatus.OnHold)
                throw new InvalidOperationException("Only projects on hold can be resumed");

            Status = ProjectStatus.Active;

            // Add domain event - ProjectResumed
            AddDomainEvent(new ProjectResumedEvent(Id, Name, Code));
        }

        // Completes an active project
        public void CompleteProject()
        {
            if (Status != ProjectStatus.Active)
                throw new InvalidOperationException("Only active projects can be completed");

            Status = ProjectStatus.Completed;
            ActualEndDate = DateTime.UtcNow;
            ProgressPercentage = 100;

            //  Add domain event - ProjectCompleted
            AddDomainEvent(new ProjectCompletedEvent(Id, Name, Code, PlannedEndDate, ActualEndDate.Value, Budget, ActualCost));
        }

        // Cancels a project (not allowed if already completed)
        public void CancelProject(string reason)
        {
            if (Status == ProjectStatus.Completed)
                throw new InvalidOperationException("Completed projects cannot be cancelled");

            Status = ProjectStatus.Cancelled;
            ActualEndDate = DateTime.UtcNow;

            //  Add domain event - ProjectCancelled
            AddDomainEvent(new ProjectCancelledEvent(Id, Name, Code, reason, ProgressPercentage));
        }

        // Updates the progress percentage
        public void UpdateProgress(int progressPercentage)
        {
            if (progressPercentage < 0 || progressPercentage > 100)
                throw new ArgumentException("Progress percentage must be between 0 and 100", nameof(progressPercentage));

            ProgressPercentage = progressPercentage;
        }

        // Updates the health status
        public void UpdateHealth(ProjectHealth health)
        {
            if (Health != health)
            {
                var previousHealth = Health;
                Health = health;

                // Add domain event - ProjectHealthChanged
                AddDomainEvent(new ProjectHealthChangedEvent(Id, Name, Code, previousHealth, health));
            }
        }

        // Adds cost to the actual cost incurred
        public void AddActualCost(decimal cost)
        {
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(cost));

            ActualCost += cost;
        }

        // Adds a team member to the project
        public void AddTeamMember(Guid teamMemberId)
        {
            if (teamMemberId == Guid.Empty)
                throw new ArgumentException("Team member ID cannot be empty", nameof(teamMemberId));

            if (!_teamMemberIds.Contains(teamMemberId))
            {
                _teamMemberIds.Add(teamMemberId);

                //  Add domain event - TeamMemberAdded
                AddDomainEvent(new TeamMemberAddedEvent(Id, Name, teamMemberId));
            }
        }

        // Removes a team member from the project
        public void RemoveTeamMember(Guid teamMemberId)
        {
            if (_teamMemberIds.Remove(teamMemberId))
            {
                //  Add domain event - TeamMemberRemoved
                AddDomainEvent(new TeamMemberRemovedEvent(Id, Name, teamMemberId));
            }
        }

        // Computed property: returns true if actual cost exceeds budget
        public bool IsOverBudget => ActualCost > Budget;

        // Computed property: returns true if project is past its planned end date and not completed
        public bool IsOverdue => DateTime.UtcNow > PlannedEndDate && Status != ProjectStatus.Completed;

        // Computed property: returns budget variance (positive = under budget, negative = over)
        public decimal BudgetVariance => Budget - ActualCost;

        // Computed property: days remaining until planned end date
        public int DaysRemaining => Status == ProjectStatus.Completed ? 0 : Math.Max(0, (PlannedEndDate - DateTime.UtcNow).Days);

        // Computed property: total planned duration of the project in days
        public int TotalDuration => (PlannedEndDate - StartDate).Days;
    }
}
