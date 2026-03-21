namespace DotNetBuildingBlocks.ConsoleDemo
{
    /// <summary>
    /// DemoResponse
    /// </summary>
    /// <param name="CorrelationId"></param>
    /// <param name="CustomerName"></param>
    /// <param name="Status"></param>
    /// <param name="OptionalNote"></param>
    public sealed record DemoResponse(
        Guid CorrelationId,
        string CustomerName,
        DemoStatus Status,
        string? OptionalNote);
}
