using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using MultiTenant.Data.Context;
using MultiTenant.Model.Master;
using MultiTenant.Repository.Contract.Master;
using MultiTenant.Tools.MigrationTool.Logger.Contract;
using Serilog;
using LogContext = Serilog.Context.LogContext;

namespace MultiTenant.Tools.MigrationTool;

public class MigrationHelper : IMigrationHelper
{
    private IClienteRepository _clienteRepository;
    private static IApplicationLogger _logger;
    
    public MigrationHelper(IClienteRepository clienteRepository, IApplicationLogger logger)
    {
        _clienteRepository = clienteRepository;
        _logger = logger;
    }
    
    private Func<EventId, LogLevel, bool> MigrationInfoLogFilter() => (eventId, level) =>
        level > LogLevel.Information ||
        (level == LogLevel.Information &&
         new[]
         {
             RelationalEventId.MigrationApplying,
             RelationalEventId.MigrationAttributeMissingWarning,
             RelationalEventId.MigrationGeneratingDownScript,
             RelationalEventId.MigrationGeneratingUpScript,
             RelationalEventId.MigrationReverting,
             RelationalEventId.MigrationsNotApplied,
             RelationalEventId.MigrationsNotFound,
             RelationalEventId.MigrateUsingConnection
         }.Contains(eventId));

    

    private DbContextOptions<SlaveContext> CreateDefaultDbContextOptions(string connectionString) => 
        new DbContextOptionsBuilder<SlaveContext>()
            .LogTo(action: Log.Logger.Information, 
                filter: MigrationInfoLogFilter(), 
                options: DbContextLoggerOptions.None
                )
            .UseMySql(
                connectionString, 
                new MySqlServerVersion(new Version(5, 7, 36)))
            .Options;



    public async Task<List<Cliente>> GetConfiguredTenants() =>
        await _clienteRepository?.Select(w => !string.IsNullOrEmpty(w.Schema))!;
    

    public async Task MigrateTenantDatabase(Cliente tenant, string defaultConnection)
    {
        try
        {
            using var logContext = LogContext.PushProperty("MainProperty", $"({tenant.Nome}) ");

            var connection = defaultConnection.Replace("%schema%", tenant.Schema);
            var dbContextOptions = CreateDefaultDbContextOptions(connection);

            await using var context = new SlaveContext(dbContextOptions);
            await context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            await _logger.Error(e, "Error occurred during migration");
            throw;
        }
    }
}