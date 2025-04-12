using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PowerBidOrchestrator.Core.Persistence;
using PowerBidOrchestrator.Infrastructure.Persistence;

namespace PowerBidOrchestrator.Infrastructure;

public class PowerBidDbContextFactory : IDesignTimeDbContextFactory<PowerBidDbContext>
{
    public PowerBidDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PowerBidDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=powerbid;Username=postgres;Password=postgres");

        return new PowerBidDbContext(optionsBuilder.Options);
    }
}
