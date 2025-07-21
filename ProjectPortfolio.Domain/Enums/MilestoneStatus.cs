namespace ProjectPortfolio.Domain.Enums
{
    /// <summary>
    /// Represents the various states a project milestone can be in.
    /// </summary>
    public enum MilestoneStatus
    {
        /// <summary>
        /// Milestone is planned but not yet started.
        /// </summary>
        Planned = 1,

        /// <summary>
        /// Milestone work is currently in progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Milestone has been reviewed and approved, but not necessarily completed.
        /// </summary>
        Approved = 3,

        /// <summary>
        /// Milestone has been fully completed.
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Milestone was not completed on time and has been marked as missed.
        /// </summary>
        Missed = 5
    }
}
