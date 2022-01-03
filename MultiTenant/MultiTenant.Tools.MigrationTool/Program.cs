using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiTenant.Data.Context;
using MultiTenant.Model.Master;
using MultiTenant.Repository;
using MultiTenant.Repository.Contract.Master;
using MultiTenant.Tools.MigrationTool;

static void LogAndConsole(string message)
{
    MigrationHelper.Logger.Information(message);
    Console.WriteLine(message);
}


var services = new ServiceCollection();
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

var config = builder.Build();

var masterConnectionString = config["Connections:Master"];
var slaveDefaultConnectionString = config["Connections:SlaveDefault"];

services.AddDbContext<MasterContext>(options => 
    options.UseMySql(masterConnectionString, new MySqlServerVersion(new Version(5, 7, 36)))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
services.AddRepository();


var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Para iniciar as migrações pressione qualquer tecla");
Console.ReadKey();

List<Cliente> tenants = await MigrationHelper.GetConfiguredTenants(serviceProvider.GetService<IClienteRepository>());

IEnumerable<Task> tasks = tenants.Select(cliente => MigrationHelper.MigrateTenantDatabase(cliente, slaveDefaultConnectionString));

try
{
    LogAndConsole("Iniciando execução das migrações pendentes em paralelo...");
    await Task.WhenAll(tasks);
}
catch
{
    LogAndConsole("A execução das migrações em paralelo foi finalizada com erros.");
    Console.WriteLine("Pressione qualquer tecla para finalizar");
    Console.ReadKey();
}

LogAndConsole("Migrações realizadas com sucesso");
Console.WriteLine("Pressione qualquer tecla para finalizar");
Console.ReadKey();