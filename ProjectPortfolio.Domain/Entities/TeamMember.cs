using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Enums;

namespace ProjectPortfolio.Domain.Entities
{
    /// <summary>
    /// Represents an individual who can be assigned to projects.
    /// </summary>
    public class TeamMember : AggregateRoot
    {
        // Basic personal info
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? PhoneNumber { get; private set; }

        // Job-related information
        public string JobTitle { get; private set; } = string.Empty;
        public string Department { get; private set; } = string.Empty;
        public decimal HourlyRate { get; private set; }
        public bool IsActive { get; private set; }

        // Skills and availability
        public string Skills { get; private set; } = string.Empty; // Currently a comma-separated string
        public int AvailabilityPercentage { get; private set; } = 100; // 0-100 percentage available

        // Optional manager relationship
        public Guid? ManagerId { get; private set; }

        // Projects the team member is assigned to
        private readonly List<Guid> _projectIds = new();
        public IReadOnlyList<Guid> ProjectIds => _projectIds.AsReadOnly();

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private TeamMember() : base() { }

        /// <summary>
        /// Factory method to create a new team member.
        /// </summary>
        public static TeamMember Create(
            string firstName,
            string lastName,
            string email,
            string jobTitle,
            string department,
            decimal hourlyRate,
            string? phoneNumber = null,
            string skills = "",
            Guid? managerId = null)
        {
            var teamMember = new TeamMember();

            teamMember.SetPersonalInfo(firstName, lastName, email, phoneNumber);
            teamMember.SetJobInfo(jobTitle, department, hourlyRate);
            teamMember.SetSkills(skills);
            teamMember.ManagerId = managerId;
            teamMember.IsActive = true;
            teamMember.AvailabilityPercentage = 100;

            return teamMember;
        }

        /// <summary>
        /// Sets or updates personal information.
        /// </summary>
        public void SetPersonalInfo(string firstName, string lastName, string email, string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format", nameof(email));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLowerInvariant();
            PhoneNumber = phoneNumber?.Trim();
        }

        /// <summary>
        /// Sets or updates job information.
        /// </summary>
        public void SetJobInfo(string jobTitle, string department, decimal hourlyRate)
        {
            if (string.IsNullOrWhiteSpace(jobTitle))
                throw new ArgumentException("Job title cannot be empty", nameof(jobTitle));

            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty", nameof(department));

            if (hourlyRate < 0)
                throw new ArgumentException("Hourly rate cannot be negative", nameof(hourlyRate));

            JobTitle = jobTitle.Trim();
            Department = department.Trim();
            HourlyRate = hourlyRate;
        }

        /// <summary>
        /// Sets or updates the skills list (stored as a comma-separated string).
        /// </summary>
        public void SetSkills(string skills)
        {
            Skills = skills?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Sets the availability percentage (0-100).
        /// </summary>
        public void SetAvailability(int availabilityPercentage)
        {
            if (availabilityPercentage < 0 || availabilityPercentage > 100)
                throw new ArgumentException("Availability percentage must be between 0 and 100", nameof(availabilityPercentage));

            AvailabilityPercentage = availabilityPercentage;
        }

        /// <summary>
        /// Sets or updates the manager relationship.
        /// </summary>
        public void SetManager(Guid? managerId)
        {
            ManagerId = managerId;
        }

        /// <summary>
        /// Activates the team member.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the team member (soft delete or temporarily unavailable).
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Assigns the team member to a project.
        /// </summary>
        public void AssignToProject(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("Project ID cannot be empty", nameof(projectId));

            if (!_projectIds.Contains(projectId))
            {
                _projectIds.Add(projectId);
            }
        }

        /// <summary>
        /// Removes the team member from a project.
        /// </summary>
        public void RemoveFromProject(Guid projectId)
        {
            _projectIds.Remove(projectId);
        }

        // ----------------------------
        // Computed / Read-only Properties
        // ----------------------------

        /// <summary>
        /// Full name of the team member.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Indicates if the team member is active and available.
        /// </summary>
        public bool IsAvailable => IsActive && AvailabilityPercentage > 0;

        /// <summary>
        /// Total number of assigned projects.
        /// </summary>
        public int ProjectCount => _projectIds.Count;

        /// <summary>
        /// Validates an email string using .NET built-in parser.
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
