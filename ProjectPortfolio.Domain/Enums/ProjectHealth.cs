namespace ProjectPortfolio.Domain.Enums
{
    // Enum representing the overall health or risk level of a project
    public enum ProjectHealth
    {
        // Green: The project is on track with no major issues
        Green = 1,

        // Yellow: The project is experiencing some issues that may affect progress
        Yellow = 2,

        // Red: The project has critical problems that need immediate resolution
        Red = 3
    }
}
