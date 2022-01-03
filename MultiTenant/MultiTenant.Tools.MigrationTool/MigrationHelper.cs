using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using MultiTenant.Data.Context;
using MultiTenant.Model.Master;
using MultiTenant.Repository.Contract.Master;
using Serilog;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace MultiTenant.Tools.MigrationTool;

public class MigrationHelper
{
    public static string loggerTemplate =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {MainProperty}{Message:lj}{NewLine}{Exception}";
    
    public static readonly ILogger Logger = Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: loggerTemplate)
        .WriteTo.File( Directory.GetCurrentDirectory() + "\\logs\\multitenant.migrationtool.txt", rollingInterval: RollingInterval.Day, outputTemplate: loggerTemplate)
        .CreateLogger();
    
    private static Func<EventId, LogLevel, bool> MigrationInfoLogFilter() => (eventId, level) =>
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
    
    private static DbContextOptions CreateDefaultDbContextOptions(string connectionString) => 
        new DbContextOptionsBuilder()
            .LogTo(action: Logger.Information, filter: MigrationInfoLogFilter(), options: DbContextLoggerOptions.None)
            .UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 36)))
            .Options;

    public async static Task<List<Cliente>> GetConfiguredTenants(IClienteRepository? clienteRepository) =>
        await clienteRepository?.Select(w => !string.IsNullOrEmpty(w.Schema));
    
    public static async Task MigrateTenantDatabase(Cliente tenant, string defaultConnection)
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
            Logger.Error(e, "Error occurred during migration");
            throw;
        }
    }
}