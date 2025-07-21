namespace ProjectPortfolio.Domain.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a work item.
    /// </summary>
    public enum WorkItemStatus
    {
        /// <summary>
        /// The work item has been created but not yet reviewed or scheduled.
        /// </summary>
        New = 1,

        /// <summary>
        /// The work item is ready to be picked up and started.
        /// </summary>
        Ready = 2,

        /// <summary>
        /// Work is currently in progress.
        /// </summary>
        InProgress = 3,

        /// <summary>
        /// Work is blocked and cannot proceed due to an external or internal issue.
        /// </summary>
        Blocked = 4,

        /// <summary>
        /// The work item has been completed successfully.
        /// </summary>
        Completed = 5,

        /// <summary>
        /// The work item has been intentionally cancelled and will not be completed.
        /// </summary>
        Cancelled = 6
    }
}
