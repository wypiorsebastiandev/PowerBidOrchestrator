using PowerBidOrchestrator.Core.Execution;

namespace PowerBidOrchestrator.Core.Abstractions;

public interface ISagaWorkflow
{
    IEnumerable<SagaStepDescriptor> GetSteps();
    string WorkflowName { get; }
}