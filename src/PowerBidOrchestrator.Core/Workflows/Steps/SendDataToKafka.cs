using PowerBidOrchestrator.Core.Abstractions;
using PowerBidOrchestrator.Core.Execution;

namespace PowerBidOrchestrator.Core.Workflows.Steps;

public class SendDataToKafka : ISagaStep
{
    public string StepName => nameof(SendDataToKafka);

    public Task ExecuteAsync(SagaContext context)
    {
        Console.WriteLine("Publishing message to Kafka");
        throw new NotImplementedException();
        return Task.CompletedTask;
    }

    public Task CompensateAsync(SagaContext context)
    {
        Console.WriteLine("Publishing compensating message to Kafka");
        return Task.CompletedTask;
    }
}