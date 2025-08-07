using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event triggered when a team is deactivated.
    // It holds information about the team and the reason for its deactivation.
    public class TeamDeactivatedEvent : BaseDomainEvent
    {
        // Unique identifier of the team that was deactivated.
        public Guid TeamId { get; }

        // Name of the team that was deactivated.
        public string TeamName { get; }

        // Reason explaining why the team was deactivated.
        public string Reason { get; }

        // Constructor to initialize the event with team ID, name, and deactivation reason.
        public TeamDeactivatedEvent(
            Guid teamId,       // ID of the deactivated team
            string teamName,   // Name of the deactivated team
            string reason)     // Reason for the deactivation
        {
            TeamId = teamId;       // Set the team ID
            TeamName = teamName;   // Set the team name
            Reason = reason;       // Set the deactivation reason
        }
    }

}
