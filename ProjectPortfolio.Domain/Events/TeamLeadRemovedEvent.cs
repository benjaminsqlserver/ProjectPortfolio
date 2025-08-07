using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that occurs when a team leader is removed from a team.
    // It stores information about the team, the removed leader, and the reason for the removal.
    public class TeamLeadRemovedEvent : BaseDomainEvent
    {
        // Unique identifier of the team from which the leader was removed.
        public Guid TeamId { get; }

        // Name of the team affected by the leader removal.
        public string TeamName { get; }

        // ID of the leader who was removed.
        public Guid PreviousLeaderId { get; }

        // Name of the leader who was removed.
        public string PreviousLeaderName { get; }

        // Reason for the removal of the team leader.
        public string Reason { get; }

        // Constructor to initialize the event with all relevant details about the removal.
        public TeamLeadRemovedEvent(
            Guid teamId,              // ID of the team
            string teamName,          // Name of the team
            Guid previousLeaderId,    // ID of the removed team leader
            string previousLeaderName,// Name of the removed team leader
            string reason)            // Reason for the leader's removal
        {
            TeamId = teamId;                       // Set the team ID
            TeamName = teamName;                   // Set the team name
            PreviousLeaderId = previousLeaderId;   // Set the removed leader's ID
            PreviousLeaderName = previousLeaderName; // Set the removed leader's name
            Reason = reason;                       // Set the reason for removal
        }
    }

}
