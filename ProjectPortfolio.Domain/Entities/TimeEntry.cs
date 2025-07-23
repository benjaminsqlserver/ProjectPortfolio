using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Events;
using ProjectPortfolio.Domain.Exceptions;

namespace ProjectPortfolio.Domain.Entities
{
    // Represents a single time tracking entry associated with a work item and team member
    public class TimeEntry : AggregateRoot
    {
        // Foreign key to the associated WorkItem
        public Guid WorkItemId { get; private set; }

        // Foreign key to the associated TeamMember (who logged the time)
        public Guid TeamMemberId { get; private set; }

        // Number of hours logged in this entry
        public decimal HoursLogged { get; private set; }

        // The specific date for which the hours are logged
        public DateTime LoggedDate { get; private set; }

        // Optional notes or comments attached to this time entry
        public string Notes { get; private set; } = string.Empty;

        // Timestamp when the time entry was created
        public DateTime CreatedAt { get; private set; }

        // Timestamp of last modification, if applicable
        public DateTime? ModifiedAt { get; private set; }

        // Indicates if the time entry has been approved
        public bool IsApproved { get; private set; }

        // ID of the person who approved the time entry (if approved)
        public Guid? ApprovedBy { get; private set; }

        // Timestamp of when the approval occurred
        public DateTime? ApprovedAt { get; private set; }

        // Navigation property to the related WorkItem entity
        public virtual WorkItem WorkItem { get; private set; } = null!;

        // Navigation property to the TeamMember who logged this entry
        public virtual TeamMember TeamMember { get; private set; } = null!;

        // Navigation property to the TeamMember who approved this entry, if any
        public virtual TeamMember? Approver { get; private set; }

        // Private constructor for EF Core to instantiate via reflection
        private TimeEntry() { }

        // Constructor for creating a new time entry
        public TimeEntry(Guid workItemId, Guid teamMemberId, decimal hoursLogged, DateTime loggedDate, string notes = "")
        {
            // Validate logged hours are greater than 0
            if (hoursLogged <= 0)
                throw new InvalidTimeEntryException("Hours logged must be greater than zero.");

            // Prevent logging more than 24 hours in a day
            if (hoursLogged > 24)
                throw new InvalidTimeEntryException("Cannot log more than 24 hours in a single day.");

            // Prevent logging time in the future
            if (loggedDate > DateTime.UtcNow.Date)
                throw new InvalidTimeEntryException("Cannot log time for future dates.");

            WorkItemId = workItemId;
            TeamMemberId = teamMemberId;
            HoursLogged = hoursLogged;
            LoggedDate = loggedDate.Date; // Normalize to date only (ignore time)
            Notes = notes ?? string.Empty;
            CreatedAt = DateTime.UtcNow;
            IsApproved = false;

            // Emit a domain event to notify the system that time has been logged
            AddDomainEvent(new TimeLoggedEvent(Id, WorkItemId, TeamMemberId, HoursLogged, LoggedDate));
        }

        // Allows updating the logged hours and optionally the notes
        public void UpdateHours(decimal newHours, string updatedNotes = null)
        {
            // Prevent modification of approved entries
            if (IsApproved)
                throw new InvalidTimeEntryException("Cannot modify approved time entries.");

            // Validate new hours
            if (newHours <= 0)
                throw new InvalidTimeEntryException("Hours logged must be greater than zero.");

            if (newHours > 24)
                throw new InvalidTimeEntryException("Cannot log more than 24 hours in a single day.");

            var oldHours = HoursLogged;
            HoursLogged = newHours;

            // Update notes if provided and not empty
            if (!string.IsNullOrWhiteSpace(updatedNotes))
                Notes = updatedNotes;

            ModifiedAt = DateTime.UtcNow;

            // Emit domain event for updating the time entry
            AddDomainEvent(new TimeEntryUpdatedEvent(Id, WorkItemId, TeamMemberId, oldHours, newHours, LoggedDate));
        }

        // Marks the time entry as approved by a specific user
        public void Approve(Guid approvedById)
        {
            // Prevent re-approval
            if (IsApproved)
                throw new InvalidTimeEntryException("Time entry is already approved.");

            IsApproved = true;
            ApprovedBy = approvedById;
            ApprovedAt = DateTime.UtcNow;

            // Emit event indicating approval
            AddDomainEvent(new TimeEntryApprovedEvent(Id, WorkItemId, TeamMemberId, HoursLogged, approvedById));
        }

        // Emits a domain event for rejection (without changing state)
        public void Reject(Guid rejectedById, string reason)
        {
            // Cannot reject an already approved entry
            if (IsApproved)
                throw new InvalidTimeEntryException("Cannot reject an already approved time entry.");

            // Emit event indicating rejection with reason
            AddDomainEvent(new TimeEntryRejectedEvent(Id, WorkItemId, TeamMemberId, HoursLogged, rejectedById, reason));
        }

        // Checks if the given user is allowed to modify the time entry
        public bool CanBeModifiedBy(Guid userId)
        {
            // Only modifiable if not approved, and either the owner or someone who could approve
            return !IsApproved && (TeamMemberId == userId || CanBeApprovedBy(userId));
        }

        // Determines whether the specified user can approve the entry
        public bool CanBeApprovedBy(Guid userId)
        {
            // Business rule: Users cannot approve their own entries
            return TeamMemberId != userId;
        }

        // Utility method to determine if the entry exceeds standard working hours (default = 8)
        public bool IsOvertime(decimal standardWorkingHours = 8)
        {
            return HoursLogged > standardWorkingHours;
        }
    }
}
