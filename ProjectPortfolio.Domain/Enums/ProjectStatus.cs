namespace ProjectPortfolio.Domain.Enums
{
    // Enum representing the various statuses a project can have in its lifecycle
    public enum ProjectStatus
    {
        // The project is in the planning phase and has not started yet
        Planning = 1,

        // The project is currently active and in progress
        Active = 2,

        // The project has been temporarily paused
        OnHold = 3,

        // The project has been completed successfully
        Completed = 4,

        // The project has been cancelled and will not proceed further
        Cancelled = 5
    }
}
