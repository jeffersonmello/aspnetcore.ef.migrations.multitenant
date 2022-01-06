using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Configuration.Log;
using MultiTenant.Model;

namespace MultiTenant.Data.Context;

public class LogContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LogConfiguration());
    }
    
    public LogContext(DbContextOptions<LogContext> options)
        : base(options)
    {
    }
    
    public LogContext(string connectionString)
        : base(new DbContextOptionsBuilder<LogContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 36))).Options)          
    {
    }
}