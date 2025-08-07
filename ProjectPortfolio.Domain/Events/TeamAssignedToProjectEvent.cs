using ProjectPortfolio.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPortfolio.Domain.Events
{
    // This class represents a domain event that is raised when a team is assigned to a project.
    // It contains information about both the team and the project involved in the assignment.
    public class TeamAssignedToProjectEvent : BaseDomainEvent
    {
        // Unique identifier of the team being assigned to the project.
        public Guid TeamId { get; }

        // Name of the team being assigned.
        public string TeamName { get; }

        // Unique identifier of the project to which the team is assigned.
        public Guid ProjectId { get; }

        // Name of the project to which the team is assigned.
        public string ProjectName { get; }

        // Constructor that initializes all properties with the provided values when the event is created.
        public TeamAssignedToProjectEvent(
            Guid teamId,        // ID of the team being assigned
            string teamName,    // Name of the team
            Guid projectId,     // ID of the project
            string projectName) // Name of the project
        {
            TeamId = teamId;           // Assign the team ID
            TeamName = teamName;       // Assign the team name
            ProjectId = projectId;     // Assign the project ID
            ProjectName = projectName; // Assign the project name
        }
    }

}
