using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that occurs when a new team leader is assigned to a team.
    // It captures both the new leader's and the previous leader's (if any) information.
    public class TeamLeadAssignedEvent : BaseDomainEvent
    {
        // Unique identifier of the team where the leadership change occurred.
        public Guid TeamId { get; }

        // Name of the team receiving the new leader.
        public string TeamName { get; }

        // Optional ID of the previous team leader, if one existed before this assignment.
        public Guid? PreviousLeaderId { get; }

        // ID of the newly assigned team leader.
        public Guid NewLeaderId { get; }

        // Optional name of the previous team leader, if one existed.
        public string? PreviousLeaderName { get; }

        // Name of the newly assigned team leader.
        public string NewLeaderName { get; }

        // Constructor initializes the properties with the provided values when the event is created.
        public TeamLeadAssignedEvent(
            Guid teamId,              // ID of the team
            string teamName,          // Name of the team
            Guid? previousLeaderId,   // ID of the old leader (if any)
            Guid newLeaderId,         // ID of the new leader
            string? previousLeaderName, // Name of the old leader (if any)
            string newLeaderName)       // Name of the new leader
        {
            TeamId = teamId;                       // Set the team ID
            TeamName = teamName;                   // Set the team name
            PreviousLeaderId = previousLeaderId;   // Set the previous leader ID (nullable)
            NewLeaderId = newLeaderId;             // Set the new leader ID
            PreviousLeaderName = previousLeaderName; // Set the previous leader name (nullable)
            NewLeaderName = newLeaderName;         // Set the new leader name
        }
    }

}
