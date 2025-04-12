namespace PowerBidOrchestrator.Core.Execution;

public class SagaContext
{
    public Guid SagaId { get; set; } = Guid.NewGuid();
    public Dictionary<string, object> Data { get; } = new();
    public CancellationToken CancellationToken { get; init; } = default;
}