using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that occurs when a member is removed from a team.
    // It includes details about the team, the member removed, and the reason for removal.
    public class TeamMemberRemovedFromTeamEvent : BaseDomainEvent
    {
        // Unique identifier of the team from which the member was removed.
        public Guid TeamId { get; }

        // Name of the team affected by the member removal.
        public string TeamName { get; }

        // Unique identifier of the member who was removed from the team.
        public Guid MemberId { get; }

        // Name of the member who was removed.
        public string MemberName { get; }

        // Explanation or justification for why the member was removed.
        public string Reason { get; }

        // Constructor initializes all relevant properties when the event is raised.
        public TeamMemberRemovedFromTeamEvent(
            Guid teamId,         // ID of the team
            string teamName,     // Name of the team
            Guid memberId,       // ID of the removed member
            string memberName,   // Name of the removed member
            string reason)       // Reason for removing the member
        {
            TeamId = teamId;         // Set the team ID
            TeamName = teamName;     // Set the team name
            MemberId = memberId;     // Set the removed member's ID
            MemberName = memberName; // Set the removed member's name
            Reason = reason;         // Set the reason for removal
        }
    }

}
