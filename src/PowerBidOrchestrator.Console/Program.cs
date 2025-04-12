using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerBidOrchestrator.Core.Abstractions;
using PowerBidOrchestrator.Core.Execution;
using PowerBidOrchestrator.Core.Persistence;
using PowerBidOrchestrator.Core.Workflows;
using PowerBidOrchestrator.Core.Workflows.Steps;
using PowerBidOrchestrator.Infrastructure;
using PowerBidOrchestrator.Infrastructure.Persistence;

// Twoje typy
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.AddDbContext<PowerBidDbContext>(options =>
        options.UseNpgsql("Host=localhost;Port=5432;Database=powerbid;Username=postgres;Password=postgres"));

    services.AddScoped<PowerBidOrchestrator.Core.Execution.PowerBidOrchestrator>();
    services.AddScoped<ISagaWorkflow, SampleWorkflow>();
    // services.AddScoped<ISagaStep, SaveMatrixDataToDatabase>();
    // services.AddScoped<ISagaStep, SendDataToKafka>();
});

var host = builder.Build();

using var scope = host.Services.CreateScope();
var orchestrator = scope.ServiceProvider.GetRequiredService<PowerBidOrchestrator.Core.Execution.PowerBidOrchestrator>();
await orchestrator.ResumeAbandonedSagasAsync(null, TimeSpan.FromMinutes(10));

var workflow = scope.ServiceProvider.GetRequiredService<ISagaWorkflow>();
var context = new SagaContext();

await orchestrator.ExecuteAsync(workflow, context);
