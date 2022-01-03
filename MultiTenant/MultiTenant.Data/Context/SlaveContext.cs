using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Configuration.Slave;

namespace MultiTenant.Data.Context;

public class SlaveContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
    }

    public SlaveContext(DbContextOptions<SlaveContext> options)
        : base(options)
    {
    }

    public SlaveContext(string connectionString)
        : base(new DbContextOptionsBuilder<SlaveContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 36))).Options)
    {
    }

    public SlaveContext(DbContextOptions dbContextOptions)
        : base(options: dbContextOptions)
    {
    }
}