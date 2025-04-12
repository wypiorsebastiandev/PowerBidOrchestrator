using PowerBidOrchestrator.Core.Abstractions;
using PowerBidOrchestrator.Core.Execution;

namespace PowerBidOrchestrator.Core.Workflows.Steps;


public class SaveMatrixDataToDatabase : ISagaStep
{
    public string StepName => nameof(SaveMatrixDataToDatabase);

    public Task ExecuteAsync(SagaContext context)
    {
        Console.WriteLine("Saving data to database");
        return Task.CompletedTask;
    }

    public Task CompensateAsync(SagaContext context)
    {
        Console.WriteLine("Rolling back data persistance");
        return Task.CompletedTask;
    }
}