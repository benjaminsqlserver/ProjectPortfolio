using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Events;

namespace ProjectPortfolio.Domain.Entities
{
    /// <summary>
    /// Represents a team of members working together on projects
    /// </summary>
    public class Team : AggregateRoot
    {
        #region Properties

        /// <summary>
        /// The name of the team
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Description of the team's purpose or responsibilities
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// The ID of the team leader
        /// </summary>
        public Guid? TeamLeaderId { get; private set; }

        /// <summary>
        /// The name of the team leader
        /// </summary>
        public string? TeamLeaderName { get; private set; }

        /// <summary>
        /// Department the team belongs to
        /// </summary>
        public string Department { get; private set; } = string.Empty;

        /// <summary>
        /// Whether the team is currently active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Maximum number of members allowed in the team
        /// </summary>
        public int? MaxMembers { get; private set; }

        /// <summary>
        /// List of team member IDs
        /// </summary>
        private readonly List<Guid> _memberIds = new();
        public IReadOnlyList<Guid> MemberIds => _memberIds.AsReadOnly();

        /// <summary>
        /// List of project IDs the team is working on
        /// </summary>
        private readonly List<Guid> _projectIds = new();
        public IReadOnlyList<Guid> ProjectIds => _projectIds.AsReadOnly();

        #endregion

        #region Computed Properties

        /// <summary>
        /// Current number of team members
        /// </summary>
        public int MemberCount => _memberIds.Count;

        /// <summary>
        /// Whether the team has reached its maximum capacity
        /// </summary>
        public bool IsAtCapacity => MaxMembers.HasValue && MemberCount >= MaxMembers.Value;

        /// <summary>
        /// Number of active projects
        /// </summary>
        public int ActiveProjectCount => _projectIds.Count;

        /// <summary>
        /// Whether the team has a designated leader
        /// </summary>
        public bool HasTeamLead => TeamLeaderId.HasValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor for EF
        /// </summary>
        private Team() : base() { }

        /// <summary>
        /// Creates a new team
        /// </summary>
        public static Team Create(
            string name,
            string description,
            string department,
            Guid? teamLeaderId = null,
            string? teamLeaderName = null,
            int? maxMembers = null)
        {
            var team = new Team();

            team.SetBasicInfo(name, description, department);
            team.SetMaxMembers(maxMembers);
            team.IsActive = true;

            if (teamLeaderId.HasValue && !string.IsNullOrWhiteSpace(teamLeaderName))
            {
                team.SetTeamLead(teamLeaderId.Value, teamLeaderName);
            }

            // Raise domain event
            team.AddDomainEvent(new TeamCreatedEvent(
                team.Id, team.Name, team.Department, teamLeaderId, teamLeaderName));

            return team;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets basic team information
        /// </summary>
        public void SetBasicInfo(string name, string description, string department)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Team name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty", nameof(department));

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Department = department.Trim();
        }

        /// <summary>
        /// Sets the maximum number of team members
        /// </summary>
        public void SetMaxMembers(int? maxMembers)
        {
            if (maxMembers.HasValue && maxMembers.Value <= 0)
                throw new ArgumentException("Max members must be greater than zero", nameof(maxMembers));

            if (maxMembers.HasValue && MemberCount > maxMembers.Value)
                throw new InvalidOperationException($"Cannot set max members to {maxMembers} when team already has {MemberCount} members");

            MaxMembers = maxMembers;
        }

        /// <summary>
        /// Sets the team leader
        /// </summary>
        public void SetTeamLead(Guid teamLeaderId, string teamLeaderName)
        {
            if (teamLeaderId == Guid.Empty)
                throw new ArgumentException("Team leader ID cannot be empty", nameof(teamLeaderId));

            if (string.IsNullOrWhiteSpace(teamLeaderName))
                throw new ArgumentException("Team leader name cannot be empty", nameof(teamLeaderName));

            var previousLeaderId = TeamLeaderId;
            var previousLeaderName = TeamLeaderName;

            TeamLeaderId = teamLeaderId;
            TeamLeaderName = teamLeaderName.Trim();

            // Add team leader as member if not already
            if (!_memberIds.Contains(teamLeaderId))
            {
                _memberIds.Add(teamLeaderId);
            }

            // Raise domain event
            AddDomainEvent(new TeamLeadAssignedEvent(
                Id, Name, previousLeaderId, teamLeaderId, previousLeaderName, teamLeaderName));
        }

        /// <summary>
        /// Removes the team leader
        /// </summary>
        public void RemoveTeamLead(string reason = "")
        {
            if (!TeamLeaderId.HasValue)
                throw new InvalidOperationException("Team does not have a team leader to remove");

            var previousLeaderId = TeamLeaderId.Value;
            var previousLeaderName = TeamLeaderName ?? string.Empty;

            TeamLeaderId = null;
            TeamLeaderName = null;

            // Raise domain event
            AddDomainEvent(new TeamLeadRemovedEvent(
                Id, Name, previousLeaderId, previousLeaderName, reason));
        }

        /// <summary>
        /// Adds a member to the team
        /// </summary>
        public void AddMember(Guid memberId, string memberName)
        {
            if (memberId == Guid.Empty)
                throw new ArgumentException("Member ID cannot be empty", nameof(memberId));

            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentException("Member name cannot be empty", nameof(memberName));

            if (!IsActive)
                throw new InvalidOperationException("Cannot add members to an inactive team");

            if (IsAtCapacity)
                throw new InvalidOperationException($"Team is at maximum capacity of {MaxMembers} members");

            if (_memberIds.Contains(memberId))
                throw new InvalidOperationException("Member is already part of the team");

            _memberIds.Add(memberId);

            // Raise domain event
            AddDomainEvent(new TeamMemberAddedToTeamEvent(Id, Name, memberId, memberName));
        }

        /// <summary>
        /// Removes a member from the team
        /// </summary>
        public void RemoveMember(Guid memberId, string memberName, string reason = "")
        {
            if (!_memberIds.Contains(memberId))
                throw new InvalidOperationException("Member is not part of the team");

            // Cannot remove team leader without first removing their leadership
            if (TeamLeaderId == memberId)
                throw new InvalidOperationException("Cannot remove team leader. Remove leadership first.");

            _memberIds.Remove(memberId);

            // Raise domain event
            AddDomainEvent(new TeamMemberRemovedFromTeamEvent(Id, Name, memberId, memberName, reason));
        }

        /// <summary>
        /// Assigns the team to a project
        /// </summary>
        public void AssignToProject(Guid projectId, string projectName)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("Project ID cannot be empty", nameof(projectId));

            if (!IsActive)
                throw new InvalidOperationException("Cannot assign inactive team to project");

            if (!_projectIds.Contains(projectId))
            {
                _projectIds.Add(projectId);

                // Raise domain event
                AddDomainEvent(new TeamAssignedToProjectEvent(Id, Name, projectId, projectName));
            }
        }

        /// <summary>
        /// Removes the team from a project
        /// </summary>
        public void RemoveFromProject(Guid projectId, string projectName, string reason = "")
        {
            if (_projectIds.Remove(projectId))
            {
                // Raise domain event
                AddDomainEvent(new TeamRemovedFromProjectEvent(Id, Name, projectId, projectName, reason));
            }
        }

        /// <summary>
        /// Activates the team
        /// </summary>
        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Team is already active");

            IsActive = true;

            // Raise domain event
            AddDomainEvent(new TeamActivatedEvent(Id, Name));
        }

        /// <summary>
        /// Deactivates the team
        /// </summary>
        public void Deactivate(string reason = "")
        {
            if (!IsActive)
                throw new InvalidOperationException("Team is already inactive");

            IsActive = false;

            // Raise domain event
            AddDomainEvent(new TeamDeactivatedEvent(Id, Name, reason));
        }

        /// <summary>
        /// Checks if a member belongs to the team
        /// </summary>
        public bool HasMember(Guid memberId)
        {
            return _memberIds.Contains(memberId);
        }

        /// <summary>
        /// Checks if the team is assigned to a project
        /// </summary>
        public bool IsAssignedToProject(Guid projectId)
        {
            return _projectIds.Contains(projectId);
        }

        /// <summary>
        /// Gets available capacity for new members
        /// </summary>
        public int? GetAvailableCapacity()
        {
            return MaxMembers.HasValue ? MaxMembers.Value - MemberCount : null;
        }

        #endregion
    }
}
