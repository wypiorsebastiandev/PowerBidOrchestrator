using PowerBidOrchestrator.Core.Execution;

namespace PowerBidOrchestrator.Core.Abstractions;

public interface ISagaStep
{
    Task ExecuteAsync(SagaContext context);
    Task CompensateAsync(SagaContext context);
    string StepName { get; }
}