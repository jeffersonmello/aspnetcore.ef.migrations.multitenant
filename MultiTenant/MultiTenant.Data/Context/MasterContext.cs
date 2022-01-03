using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Configuration.Master;

namespace MultiTenant.Data.Context;

public class MasterContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
    }
    
    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }
    
    public MasterContext(string connectionString)
        : base(new DbContextOptionsBuilder<MasterContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 36))).Options)          
    {
    }
}