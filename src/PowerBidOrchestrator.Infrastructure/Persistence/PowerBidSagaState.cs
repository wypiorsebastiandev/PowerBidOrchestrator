using System.ComponentModel.DataAnnotations.Schema;

namespace PowerBidOrchestrator.Core.Persistence;

[Table("SagaStates")]
public class PowerBidSagaState
{
    public Guid Id { get; set; }
    public string WorkflowName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? LastCompletedStep { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    
}