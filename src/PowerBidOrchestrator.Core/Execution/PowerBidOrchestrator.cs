using PowerBidOrchestrator.Core.Abstractions;
using PowerBidOrchestrator.Core.Persistence;
using PowerBidOrchestrator.Infrastructure.Persistence;

namespace PowerBidOrchestrator.Core.Execution;

using Microsoft.EntityFrameworkCore;
public class PowerBidOrchestrator
{
    private readonly PowerBidDbContext _db;

    public PowerBidOrchestrator(PowerBidDbContext db)
    {
        _db = db;
    }

    public async Task ExecuteAsync(ISagaWorkflow workflow, SagaContext context)
    {
        var state = await _db.SagaStates.FindAsync(context.SagaId);
        if (state == null)
        {
            state = new PowerBidSagaState
            {
                Id = context.SagaId,
                WorkflowName = workflow.WorkflowName,
                Status = "Running",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };
            _db.SagaStates.Add(state);
            await _db.SaveChangesAsync();
        }

        var executedSteps = new Stack<ISagaStep>();
        var resumeMode = !string.IsNullOrEmpty(state.LastCompletedStep);
        var skip = resumeMode;

        foreach (var descriptor in workflow.GetSteps())
        {
            var step = descriptor.Step;

            if (skip)
            {
                if (step.StepName == state.LastCompletedStep)
                {
                    skip = false;
                }
                continue;
            }

            int attempts = 0;
            while (true)
            {
                try
                {
                    await step.ExecuteAsync(context);
                    executedSteps.Push(step);
                    state.LastCompletedStep = step.StepName;
                    state.LastUpdatedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                    break;
                }
                catch (Exception ex) when (attempts < descriptor.RetryPolicy.MaxRetries)
                {
                    attempts++;
                    await Task.Delay(descriptor.RetryPolicy.Delay);
                }
                catch
                {
                    await CompensateAsync(executedSteps, context);
                    state.Status = "Failed";
                    state.LastUpdatedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                    throw;
                }
            }
        }

        state.Status = "Completed";
        state.CompletedAt = DateTime.UtcNow;
        state.LastUpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task ResumeAbandonedSagasAsync(ISagaWorkflow workflow, TimeSpan maxInactivityTime)
    {
        var cutoff = DateTime.UtcNow.Subtract(maxInactivityTime);
        var sagasToResume = await _db.SagaStates
            .Where(s => (workflow == null || (workflow != null && s.WorkflowName == workflow.WorkflowName)) &&
            // .Where(s => 
                        s.Status == "Running" &&
                        s.LastUpdatedAt < cutoff)
            .ToListAsync();

        foreach (var saga in sagasToResume)
        {
            var context = new SagaContext { SagaId = saga.Id };
            await ExecuteAsync(workflow, context);
        }
    }

    private async Task CompensateAsync(Stack<ISagaStep> executedSteps, SagaContext context)
    {
        while (executedSteps.Count > 0)
        {
            var step = executedSteps.Pop();
            try
            {
                await step.CompensateAsync(context);
            }
            catch
            {
                // log and continue
                // In a real-world scenario, we might want to handle compensation failures differently
                // e.g., send alerts, retry, etc.
                // but we shouldn't stop the compensation process
                // and try to compensate as much as possible
            }
        }
    }
}