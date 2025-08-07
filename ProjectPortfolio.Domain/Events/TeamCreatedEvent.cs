using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that is triggered when a new team is created.
    // It inherits from BaseDomainEvent, which typically includes metadata such as the event timestamp.
    public class TeamCreatedEvent : BaseDomainEvent
    {
        // Unique identifier for the newly created team.
        public Guid TeamId { get; }

        // Name of the team that was created.
        public string TeamName { get; }

        // Department under which the team operates.
        public string Department { get; }

        // Optional identifier for the team leader.
        // Nullable in case a team leader is not yet assigned.
        public Guid? TeamLeaderId { get; }

        // Optional name of the team leader.
        // Nullable to account for cases where the name is unknown or not yet assigned.
        public string? TeamLeaderName { get; }

        // Constructor to initialize all properties of the TeamCreatedEvent.
        // Parameters are passed when the event is instantiated.
        public TeamCreatedEvent(Guid teamId, string teamName, string department, Guid? teamLeaderId, string? teamLeaderName)
        {
            // Assign the team ID from the parameter to the read-only property.
            TeamId = teamId;

            // Assign the team name from the parameter.
            TeamName = teamName;

            // Assign the department name from the parameter.
            Department = department;

            // Assign the optional team leader ID, if provided.
            TeamLeaderId = teamLeaderId;

            // Assign the optional team leader name, if provided.
            TeamLeaderName = teamLeaderName;
        }
    }

}