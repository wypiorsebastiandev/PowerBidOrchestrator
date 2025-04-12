using Microsoft.EntityFrameworkCore;
using PowerBidOrchestrator.Core.Persistence;

namespace PowerBidOrchestrator.Infrastructure.Persistence;

public class PowerBidDbContext : DbContext
{
    public DbSet<PowerBidSagaState> SagaStates => Set<PowerBidSagaState>();

    public PowerBidDbContext(DbContextOptions<PowerBidDbContext> options) : base(options) { }
}