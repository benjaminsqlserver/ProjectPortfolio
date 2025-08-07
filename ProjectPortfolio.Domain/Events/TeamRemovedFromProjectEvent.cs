using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that is triggered when a team is removed from a project.
    // It contains details about the team, the project, and the reason for the removal.
    public class TeamRemovedFromProjectEvent : BaseDomainEvent
    {
        // Unique identifier of the team that was removed.
        public Guid TeamId { get; }

        // Name of the team that was removed.
        public string TeamName { get; }

        // Unique identifier of the project from which the team was removed.
        public Guid ProjectId { get; }

        // Name of the project from which the team was removed.
        public string ProjectName { get; }

        // Reason why the team was removed from the project.
        public string Reason { get; }

        // Constructor that initializes all fields of the event.
        public TeamRemovedFromProjectEvent(
            Guid teamId,         // ID of the removed team
            string teamName,     // Name of the removed team
            Guid projectId,      // ID of the project
            string projectName,  // Name of the project
            string reason)       // Reason for the team's removal from the project
        {
            TeamId = teamId;           // Set the team ID
            TeamName = teamName;       // Set the team name
            ProjectId = projectId;     // Set the project ID
            ProjectName = projectName; // Set the project name
            Reason = reason;           // Set the reason for removal
        }
    }

}
