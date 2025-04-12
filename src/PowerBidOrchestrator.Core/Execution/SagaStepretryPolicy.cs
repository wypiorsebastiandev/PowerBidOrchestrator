namespace PowerBidOrchestrator.Core.Execution;

public class SagaStepRetryPolicy
{
    public int MaxRetries { get; set; } = 3;
    public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(1);
}