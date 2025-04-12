using PowerBidOrchestrator.Core.Abstractions;
using PowerBidOrchestrator.Core.Execution;
using PowerBidOrchestrator.Core.Workflows.Steps;

namespace PowerBidOrchestrator.Core.Workflows;

public class SampleWorkflow : ISagaWorkflow
{
    public string WorkflowName => "CreateOrderSaga";

    public IEnumerable<SagaStepDescriptor> GetSteps()
    {
        yield return new SagaStepDescriptor { Step = new SaveMatrixDataToDatabase() };
        yield return new SagaStepDescriptor { Step = new SendDataToKafka() };
    }
    
}