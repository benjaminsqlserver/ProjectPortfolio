using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Events;
using ProjectPortfolio.Domain.Exceptions;
using ProjectPortfolio.Domain.ValueObjects;

namespace ProjectPortfolio.Domain.Entities
{
    /// <summary>
    /// Represents the allocation of a specific team member to a project
    /// for a specified time period and effort percentage.
    /// </summary>
    public class ResourceAllocation : AggregateRoot
    {
        // Identifies the project to which this allocation is associated
        public Guid ProjectId { get; private set; }

        // Identifies the team member being allocated
        public Guid TeamMemberId { get; private set; }

        // Percentage of the team member's time allocated to the project (e.g., 50%, 100%)
        public decimal AllocationPercentage { get; private set; }

        // The start and end date of the allocation
        public DateRange AllocationPeriod { get; private set; }

        // Role assigned to the team member for the project (e.g., Developer, Tester)
        public string Role { get; private set; } = string.Empty;

        // Optional free-text notes about this allocation (e.g., reason, changes)
        public string Notes { get; private set; } = string.Empty;

        // Date and time the allocation record was created
        public DateTime CreatedAt { get; private set; }

        // Date and time of the most recent modification (if any)
        public DateTime? ModifiedAt { get; private set; }

        // Identifier of the user who created the allocation
        public Guid CreatedBy { get; private set; }

        // Identifier of the user who last modified the allocation (if any)
        public Guid? ModifiedBy { get; private set; }

        // Indicates if the allocation is currently active
        public bool IsActive { get; private set; }

        // Navigation property for the associated Project (used by EF Core)
        public virtual Project Project { get; private set; } = null!;

        // Navigation property for the associated TeamMember (used by EF Core)
        public virtual TeamMember TeamMember { get; private set; } = null!;

        // Indicates whether the current date is within the allocation period
        public bool IsCurrentAllocation => AllocationPeriod.IsCurrent;

        // True if allocation percentage exceeds 100% (overallocated)
        public bool IsOverAllocated => AllocationPercentage > 100;

        // True if allocation is full-time or more (100% or above)
        public bool IsFullTimeAllocation => AllocationPercentage >= 100;

        // True if allocation is part-time (between 0% and 100%)
        public bool IsPartTimeAllocation => AllocationPercentage > 0 && AllocationPercentage < 100;

        // Total number of days the allocation spans
        public int AllocationDurationInDays => AllocationPeriod.DurationInDays;

        // Total effort in hours (based on allocation percentage and duration)
        public decimal TotalEffortHours => CalculateTotalEffortHours();

        // True if the allocation period has started
        public bool HasStarted => AllocationPeriod.HasStarted;

        // True if the allocation period has ended
        public bool HasEnded => AllocationPeriod.HasEnded;

        // Private constructor for EF Core (required for ORM materialization)
        private ResourceAllocation() { }

        /// <summary>
        /// Public constructor used to create a new resource allocation with validation.
        /// </summary>
        public ResourceAllocation(
            Guid projectId,
            Guid teamMemberId,
            decimal allocationPercentage,
            DateRange allocationPeriod,
            string role,
            Guid createdBy,
            string notes = "")
        {
            // Ensure valid allocation percentage
            ValidateAllocationPercentage(allocationPercentage);

            // Ensure date range is valid (start < end, etc.)
            ValidateAllocationPeriod(allocationPeriod);

            // Ensure role is non-empty and well-formed
            ValidateRole(role);

            ProjectId = projectId;
            TeamMemberId = teamMemberId;
            AllocationPercentage = allocationPercentage;
            AllocationPeriod = allocationPeriod;
            Role = role.Trim();
            Notes = notes ?? string.Empty;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
            IsActive = true;

            // Raise a domain event to notify the system that a new allocation was created
            AddDomainEvent(new ResourceAllocatedEvent(
                Id, ProjectId, TeamMemberId, AllocationPercentage, AllocationPeriod, Role));
        }

        /// <summary>
        /// Updates the allocation percentage with business rules and logging.
        /// </summary>
        public void UpdateAllocationPercentage(decimal newPercentage, Guid modifiedBy, string reason = "")
        {
            if (!IsActive)
                throw new InvalidAllocationException("Cannot modify inactive allocation.");

            if (HasEnded)
                throw new InvalidAllocationException("Cannot modify allocation that has already ended.");

            ValidateAllocationPercentage(newPercentage);

            var oldPercentage = AllocationPercentage;
            AllocationPercentage = newPercentage;
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;

            // Optionally append a note about the change
            if (!string.IsNullOrWhiteSpace(reason))
                Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] Allocation changed from {oldPercentage}% to {newPercentage}%: {reason}";

            // Notify the system that an allocation was updated
            AddDomainEvent(new ResourceAllocationUpdatedEvent(
                Id, ProjectId, TeamMemberId, oldPercentage, newPercentage, reason));

            // Raise a warning if the new percentage causes overallocation
            if (IsOverAllocated)
            {
                AddDomainEvent(new ResourceOverallocationDetectedEvent(
                    Id, ProjectId, TeamMemberId, AllocationPercentage, AllocationPeriod));
            }
        }

        /// <summary>
        /// Extends the allocation period to a new end date, ensuring it only moves forward.
        /// </summary>
        public void ExtendAllocation(DateTime newEndDate, Guid modifiedBy, string reason = "")
        {
            if (!IsActive)
                throw new InvalidAllocationException("Cannot extend inactive allocation.");

            if (newEndDate <= AllocationPeriod.EndDate)
                throw new InvalidAllocationException("New end date must be later than current end date.");

            var oldPeriod = AllocationPeriod;
            AllocationPeriod = new DateRange(AllocationPeriod.StartDate, newEndDate);
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;

            if (!string.IsNullOrWhiteSpace(reason))
                Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] Allocation extended to {newEndDate:yyyy-MM-dd}: {reason}";

            AddDomainEvent(new ResourceAllocationExtendedEvent(
                Id, ProjectId, TeamMemberId, oldPeriod, AllocationPeriod,reason));
        }

        /// <summary>
        /// Reduces the allocation period by setting a new, earlier end date.
        /// </summary>
        /// <param name="newEndDate">The new end date for the allocation period. Must be earlier than the current end date.</param>
        /// <param name="modifiedBy">The identifier of the user who is performing the modification.</param>
        /// <param name="reason">An optional reason for the reduction, recorded in the allocation notes and event.</param>
        /// <exception cref="InvalidAllocationException">
        /// Thrown if:
        /// - The allocation is inactive,
        /// - The new end date is not earlier than the current one,
        /// - The new end date is in the past.
        /// </exception>
        public void ReduceAllocation(DateTime newEndDate, Guid modifiedBy, string reason = "")
        {
            // Ensure the allocation is still active
            if (!IsActive)
                throw new InvalidAllocationException("Cannot reduce inactive allocation.");

            // Ensure the new end date is earlier than the current end date
            if (newEndDate >= AllocationPeriod.EndDate)
                throw new InvalidAllocationException("New end date must be earlier than current end date.");

            // Prevent reducing to a date in the past
            if (newEndDate < DateTime.Today)
                throw new InvalidAllocationException("Cannot set end date in the past.");

            // Store the previous allocation period for event tracking
            var oldPeriod = AllocationPeriod;

            // Update the allocation period with the new end date
            AllocationPeriod = new DateRange(AllocationPeriod.StartDate, newEndDate);

            // Record modification metadata
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;

            // Append reason to notes, if provided
            if (!string.IsNullOrWhiteSpace(reason))
            {
                Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] Allocation reduced to {newEndDate:yyyy-MM-dd}: {reason}";
            }

            // Raise domain event for tracking and external systems
            AddDomainEvent(new ResourceAllocationReducedEvent(
                Id,
                ProjectId,
                TeamMemberId,
                oldPeriod,
                AllocationPeriod,
                reason));
        }

        /// <summary>
        /// Updates the role assigned to the resource allocation.
        /// </summary>
        /// <param name="newRole">The new role to assign to the resource.</param>
        /// <param name="modifiedBy">The ID of the user making the change.</param>
        /// <param name="reason">Optional reason for the role change (used for auditing).</param>
        /// <exception cref="InvalidAllocationException">
        /// Thrown if the allocation is inactive or if the new role is invalid.
        /// </exception>
        public void UpdateRole(string newRole, Guid modifiedBy, string reason = "")
        {
            // Ensure the allocation is still active before allowing modification
            if (!IsActive)
                throw new InvalidAllocationException("Cannot modify inactive allocation.");

            // Validate the new role using a domain-specific validator
            ValidateRole(newRole);

            var oldRole = Role;

            // Trim w]hitespace and assign new role
            Role = newRole.Trim();

            // Audit fields for tracking the modification
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;

            // Append a note to record the reason for change, if provided
            if (!string.IsNullOrWhiteSpace(reason))
                Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] Role changed from '{oldRole}' to '{newRole}': {reason}";

            // Raise a domain event to notify the system of the role update
            AddDomainEvent(new ResourceRoleUpdatedEvent(
                Id, ProjectId, TeamMemberId, oldRole, newRole, reason));
        }


        /// <summary>
        /// Deactivates the current resource allocation.
        /// </summary>
        /// <param name="deactivatedBy">The identifier of the user performing the deactivation.</param>
        /// <param name="reason">An optional reason for deactivation, which will be appended to the notes.</param>
        /// <exception cref="InvalidAllocationException">Thrown if the allocation is already inactive.</exception>
        public void Deactivate(Guid deactivatedBy, string reason = "")
        {
            // Prevent redundant deactivation
            if (!IsActive)
                throw new InvalidAllocationException("Allocation is already inactive.");

            // Mark the allocation as inactive
            IsActive = false;

            // Audit metadata
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = deactivatedBy;

            // Append deactivation reason to notes, if provided
            if (!string.IsNullOrWhiteSpace(reason))
            {
                Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] Allocation deactivated: {reason}";
            }

            // Raise a domain event for the deactivation
            AddDomainEvent(new ResourceAllocationDeactivatedEvent(
                Id,
                ProjectId,
                TeamMemberId,
                AllocationPercentage,
                AllocationPeriod,
                reason));
        }

        /// <summary>
        /// Reactivates a previously deactivated resource allocation, if it's still within the allocation period.
        /// </summary>
        /// <param name="reactivatedBy">The ID of the user or system reactivating the allocation.</param>
        /// <param name="reason">Optional reason for reactivation, for audit/log purposes.</param>
        /// <exception cref="InvalidAllocationException">
        /// Thrown if the allocation is already active or if the allocation period has already ended.
        /// </exception>
        public void Reactivate(Guid reactivatedBy, string reason = "")
        {
            // Prevent reactivation if the allocation is already active
            if (IsActive)
                throw new InvalidAllocationException("Allocation is already active.");

            // Prevent reactivation if the end date has already passed
            if (HasEnded)
                throw new InvalidAllocationException("Cannot reactivate allocation that has already ended.");

            // Set the allocation as active again
            IsActive = true;

            // Update metadata for auditing purposes
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = reactivatedBy;

            // Append reason to notes if provided
            if (!string.IsNullOrWhiteSpace(reason))
                Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] Allocation reactivated: {reason}";

            // Raise a domain event for this change (for logging, notification, etc.)
            AddDomainEvent(new ResourceAllocationReactivatedEvent(
                Id,
                ProjectId,
                TeamMemberId,
                AllocationPercentage,
                AllocationPeriod,
                reason));
        }


        /// <summary>
        /// Checks if this allocation overlaps with another allocation
        /// for the same team member and both are active.
        /// </summary>
        /// <param name="other">The other resource allocation to compare with.</param>
        /// <returns>
        /// True if the allocations overlap in time and both are active for the same team member; otherwise, false.
        /// </returns>
        public bool OverlapsWith(ResourceAllocation other)
        {
            return TeamMemberId == other.TeamMemberId &&
                   AllocationPeriod.Overlaps(other.AllocationPeriod) &&
                   IsActive && other.IsActive;
        }

        /// <summary>
        /// Calculates the total allocation percentage including another overlapping allocation.
        /// Returns the sum of both percentages if they overlap, otherwise returns this allocation's percentage only.
        /// </summary>
        /// <param name="other">The other allocation to consider.</param>
        /// <returns>The combined allocation percentage during the overlap, if any.</returns>
        public decimal GetTotalAllocationWithOther(ResourceAllocation other)
        {
            if (!OverlapsWith(other))
                return AllocationPercentage;

            var overlap = AllocationPeriod.GetOverlap(other.AllocationPeriod);
            if (overlap == null)
                return AllocationPercentage;

            return AllocationPercentage + other.AllocationPercentage;
        }








        /// <summary>
        /// Calculates the effective allocation percentage for a given date.
        /// </summary>
        /// <param name="date">The date for which to determine the allocation percentage.</param>
        /// <returns>
        /// Returns the allocation percentage if the current object is active and the specified date 
        /// falls within the allocation period; otherwise, returns 0.
        /// </returns>
        /// <remarks>
        /// This method checks two conditions before returning the allocation:
        /// 1. <c>IsActive</c> must be <c>true</c>, indicating that the allocation is currently active.
        /// 2. <c>AllocationPeriod.Contains(date)</c> must return <c>true</c>, ensuring that the date
        ///    falls within the defined allocation period (likely a custom date range implementation).
        /// If either condition fails, the method returns 0, meaning there is no effective allocation
        /// for that date.
        /// </remarks>
        public decimal GetEffectiveAllocationForDate(DateTime date)
        {
            if (!IsActive || !AllocationPeriod.Contains(date))
                return 0;

            return AllocationPercentage;
        }


        /// <summary>
        /// Calculates the average number of hours worked per week based on the allocation percentage.
        /// </summary>
        /// <param name="standardWorkingHoursPerWeek">
        /// The standard number of working hours in a full-time workweek.
        /// Defaults to 40 hours if not specified.
        /// </param>
        /// <returns>
        /// The average weekly hours worked, computed as a percentage of the standard working hours.
        /// </returns>
        /// <remarks>
        /// This method multiplies the allocation percentage by the standard working hours per week,
        /// and divides by 100 to convert the percentage to a decimal.
        /// 
        /// For example:
        /// - If <c>AllocationPercentage</c> is 50 and the standard workweek is 40 hours,
        ///   the result is 20 hours.
        /// - If <c>AllocationPercentage</c> is 100, it represents a full-time schedule (40 hours).
        /// </remarks>
        public decimal GetAverageWeeklyHours(decimal standardWorkingHoursPerWeek = 40)
        {
            return (AllocationPercentage / 100) * standardWorkingHoursPerWeek;
        }




        /// <summary>
        /// Calculates the expected number of working hours for a given time period,
        /// based on the allocation percentage and standard working hours per day.
        /// </summary>
        /// <param name="period">
        /// The time period (as a <c>DateRange</c>) for which to calculate expected working hours.
        /// </param>
        /// <param name="standardWorkingHoursPerDay">
        /// The standard number of working hours in a single working day. Defaults to 8 hours.
        /// </param>
        /// <returns>
        /// The expected number of working hours within the specified period, 
        /// adjusted for overlap with the allocation period and the allocation percentage.
        /// Returns 0 if there is no overlap or if the allocation is inactive.
        /// </returns>
        /// <remarks>
        /// Steps performed by this method:
        /// 1. Determines the overlap between the input <paramref name="period"/> and the instance's
        ///    <c>AllocationPeriod</c>.
        /// 2. If there is no overlap or the allocation is not active (<c>IsActive</c> is false),
        ///    the method returns 0, meaning no hours are expected.
        /// 3. Otherwise, it calculates the number of weekday (non-weekend) days within the overlap.
        /// 4. Multiplies the number of working days by the standard hours per day.
        /// 5. Adjusts the result according to the allocation percentage (as a fraction of 100).
        /// 
        /// Example:
        /// - If there are 10 weekdays in the overlap, standard hours/day is 8, and allocation is 50%,
        ///   expected hours = 10 * 8 * 0.5 = 40.
        /// </remarks>
        public decimal GetExpectedHoursForPeriod(DateRange period, decimal standardWorkingHoursPerDay = 8)
        {
            var overlap = AllocationPeriod.GetOverlap(period);
            if (overlap == null || !IsActive)
                return 0;

            var workingDays = overlap.GetWeekdayCount();
            return workingDays * standardWorkingHoursPerDay * (AllocationPercentage / 100);
        }




        /// <summary>
        /// Determines whether this allocation conflicts with any other allocation in the provided list.
        /// </summary>
        /// <param name="otherAllocations">
        /// A collection of other <see cref="ResourceAllocation"/> instances to check against.
        /// </param>
        /// <returns>
        /// <c>true</c> if there is at least one allocation (with a different ID) that overlaps in time
        /// and causes the total allocation percentage to exceed 100%; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// A conflict occurs when:
        /// - The other allocation is not the same as the current one (<c>Id</c> is different),
        /// - The time periods overlap (<c>OverlapsWith(other)</c>),
        /// - The combined allocation percentage exceeds 100%.
        /// </remarks>
        public bool IsConflictingWith(IEnumerable<ResourceAllocation> otherAllocations)
        {
            return otherAllocations.Any(other =>
                other.Id != Id &&
                OverlapsWith(other) &&
                GetTotalAllocationWithOther(other) > 100);
        }



        /// <summary>
        /// Appends a timestamped note to the allocation record and updates modification metadata.
        /// </summary>
        /// <param name="note">The note text to be added. Whitespace-only or null notes are ignored.</param>
        /// <param name="addedBy">The identifier of the user adding the note.</param>
        /// <remarks>
        /// Notes are appended with a UTC timestamp in the format [yyyy-MM-dd].
        /// The <c>ModifiedAt</c> and <c>ModifiedBy</c> fields are updated accordingly.
        /// </remarks>
        public void AddNote(string note, Guid addedBy)
        {
            if (string.IsNullOrWhiteSpace(note))
                return;

            Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd}] {note}";
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = addedBy;
        }


        /// <summary>
        /// Calculates the total number of effort hours for the allocation over its duration,
        /// based on working weekdays and allocation percentage.
        /// </summary>
        /// <param name="standardWorkingHoursPerDay">
        /// Standard number of working hours in a weekday. Defaults to 8 hours.
        /// </param>
        /// <returns>
        /// The total number of hours the resource is expected to work during the allocation period,
        /// or 0 if the allocation is inactive.
        /// </returns>
        /// <remarks>
        /// The method multiplies the number of weekdays in the <c>AllocationPeriod</c> by the standard
        /// daily hours, and scales the result by the <c>AllocationPercentage</c>.
        /// 
        /// Example:
        /// - 20 weekdays × 8 hours/day × 50% allocation = 80 total effort hours.
        /// </remarks>
        private decimal CalculateTotalEffortHours(decimal standardWorkingHoursPerDay = 8)
        {
            if (!IsActive)
                return 0;

            var workingDays = AllocationPeriod.GetWeekdayCount();
            return workingDays * standardWorkingHoursPerDay * (AllocationPercentage / 100);
        }










        /// <summary>
        /// Validates that the allocation percentage is within the expected range (0–100+).
        /// </summary>
        private static void ValidateAllocationPercentage(decimal percentage)
        {
            if (percentage < 0)
                throw new InvalidAllocationException("Allocation percentage cannot be negative.");

            if (percentage > 200) // Allow up to 200% for overtime scenarios
                throw new InvalidAllocationException("Allocation percentage cannot exceed 200%.");
        }

        /// <summary>
        /// Validates that the allocation period has a valid start and end date and allocation period can not be more than two years.
        /// </summary>
        private static void ValidateAllocationPeriod(DateRange period)
        {
            if (period.StartDate < DateTime.Today.AddYears(-1))
                throw new InvalidAllocationException("Allocation start date cannot be more than 1 year in the past.");

            if (period.EndDate > DateTime.Today.AddYears(5))
                throw new InvalidAllocationException("Allocation end date cannot be more than 5 years in the future.");

            if (period.DurationInDays > 365 * 2) // 2 years max
                throw new InvalidAllocationException("Allocation period cannot exceed 2 years.");
        }

        /// <summary>
        /// Validates that the role field is not empty or whitespace and it is not more than 100 characters.
        /// </summary>
        private static void ValidateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new InvalidAllocationException("Role cannot be null or empty.");

            if (role.Trim().Length > 100)
                throw new InvalidAllocationException("Role cannot exceed 100 characters.");
        }

        ///// <summary>
        ///// Calculates the total effort in hours, assuming 8 working hours per day.
        ///// </summary>
        //private decimal CalculateTotalEffortHours()
        //{
        //    const decimal hoursPerDay = 8;
        //    return AllocationPercentage / 100 * AllocationDurationInDays * hoursPerDay;
        //}
    }
}
