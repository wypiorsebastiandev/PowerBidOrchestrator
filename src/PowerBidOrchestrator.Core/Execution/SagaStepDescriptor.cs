using PowerBidOrchestrator.Core.Abstractions;

namespace PowerBidOrchestrator.Core.Execution;

public class SagaStepDescriptor
{
    public ISagaStep Step { get; set; }
    public SagaStepRetryPolicy RetryPolicy { get; set; } = new();
}