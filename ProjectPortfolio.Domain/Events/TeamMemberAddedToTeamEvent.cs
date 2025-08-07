using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event triggered when a new member is added to a team.
    // It holds information about the team and the newly added member.
    public class TeamMemberAddedToTeamEvent : BaseDomainEvent
    {
        // Unique identifier of the team to which the member was added.
        public Guid TeamId { get; }

        // Name of the team to which the member was added.
        public string TeamName { get; }

        // Unique identifier of the newly added team member.
        public Guid MemberId { get; }

        // Name of the newly added team member.
        public string MemberName { get; }

        // Constructor to initialize the event with details about the team and the new member.
        public TeamMemberAddedToTeamEvent(
            Guid teamId,         // ID of the team
            string teamName,     // Name of the team
            Guid memberId,       // ID of the new member
            string memberName)   // Name of the new member
        {
            TeamId = teamId;           // Set the team ID
            TeamName = teamName;       // Set the team name
            MemberId = memberId;       // Set the new member's ID
            MemberName = memberName;   // Set the new member's name
        }
    }

}
