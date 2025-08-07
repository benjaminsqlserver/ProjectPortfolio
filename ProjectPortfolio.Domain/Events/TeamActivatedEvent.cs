using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that occurs when a team is activated.
    // It contains the identifier and name of the team that has been activated.
    public class TeamActivatedEvent : BaseDomainEvent
    {
        // Unique identifier of the team that was activated.
        public Guid TeamId { get; }

        // Name of the team that was activated.
        public string TeamName { get; }

        // Constructor initializes the event with the team ID and name.
        public TeamActivatedEvent(
            Guid teamId,      // ID of the activated team
            string teamName)  // Name of the activated team
        {
            TeamId = teamId;         // Set the team ID
            TeamName = teamName;     // Set the team name
        }
    }

}
