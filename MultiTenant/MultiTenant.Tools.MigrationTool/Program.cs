using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiTenant.Data.Context;
using MultiTenant.Repository;
using MultiTenant.Tools.MigrationTool;
using MultiTenant.Tools.MigrationTool.Logger;
using MultiTenant.Tools.MigrationTool.Logger.Contract;

IApplicationLogger? logger;


async Task LogAndConsole(string message)
{
    await logger?.Information(message)!;
    Console.WriteLine(message);
}


var services = new ServiceCollection();
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

var config = builder.Build();

#region Connection Strings

var masterConnectionString = config["Connections:Master"];
var slaveDefaultConnectionString = config["Connections:SlaveDefault"];
var logConnectionString = config["Connections:Log"];

#endregion

#region DbContext

services.AddDbContext<MasterContext>(options => 
    options.UseMySql(masterConnectionString, new MySqlServerVersion(new Version(5, 7, 36)))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
);

services.AddDbContext<LogContext>(options => 
    options.UseMySql(logConnectionString, new MySqlServerVersion(new Version(5, 7, 36)))
);

#endregion

#region Services

services.AddRepository();
services.AddScoped<IApplicationLogger, ApplicationLogger>();
services.AddScoped<IMigrationHelper, MigrationHelper>();
services.AddScoped(_ => new ConsoleSettings
{
    AwsAccessKey = config["Aws:access_key"],
    AwsSecretKey = config["Aws:secret_key"],
    AwsBucket = config["Aws:bucket_name"]
});

#endregion


var serviceProvider = services.BuildServiceProvider();

logger = serviceProvider.GetService<IApplicationLogger>();

var migration = serviceProvider.GetService<IMigrationHelper>();


Console.WriteLine("Para iniciar as migrações pressione qualquer tecla");
Console.ReadKey();


var tenants = await migration?.GetConfiguredTenants()!;
var tasks = tenants.Select(cliente => migration.MigrateTenantDatabase(cliente, slaveDefaultConnectionString));

try
{
    await LogAndConsole("Iniciando execução das migrações pendentes em paralelo...");
    await Task.WhenAll(tasks);
}
catch
{
    await LogAndConsole("A execução das migrações em paralelo foi finalizada com erros.");
    Console.WriteLine("Pressione qualquer tecla para finalizar");
    Console.ReadKey();
}

await LogAndConsole("Migrações realizadas com sucesso");

await logger?.Error(new Exception("fdsfsfsd"), "sdsd")!;

Console.WriteLine("Pressione qualquer tecla para finalizar");
Console.ReadKey();

