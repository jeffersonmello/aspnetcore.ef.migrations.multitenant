using Amazon;
using Serilog;
using MultiTenant.Data.Context;
using MultiTenant.Tools.MigrationTool.Logger.Contract;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace MultiTenant.Tools.MigrationTool.Logger;

public class ApplicationLogger : IApplicationLogger
{
    private static string loggerTemplate =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {MainProperty}{Message:lj}{NewLine}{Exception}";

    public ILogger _logger;

    private LogContext _logContext;


    public ApplicationLogger(LogContext logContext, ConsoleSettings settings)
    {
        _logger = Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: loggerTemplate)
            .WriteTo.File(Directory.GetCurrentDirectory() + "\\logs\\multitenant.migrationtool.txt",
                rollingInterval: RollingInterval.Day, outputTemplate: loggerTemplate)
            .WriteTo.AmazonS3(
                "multitenant.migrationtool.txt",
                settings.AwsBucket,
                RegionEndpoint.USEast1,
                settings.AwsAccessKey,
                settings.AwsSecretKey,
                LevelAlias.Minimum,
                loggerTemplate,
                rollingInterval: Serilog.Sinks.AmazonS3.RollingInterval.Day
            )
            .CreateLogger();

        _logContext = logContext;
    }

    public async Task Error(Exception e, string message)
    {
        _logger.Error(e, message);
        await SaveLog(new Model.Log
        {
            Schema = string.Empty,
            Mensagem = message,
            Acontecimento = nameof(_logger.Error),
            StackTrace = $"{message}\n\n{e.Message}\n{e.StackTrace}"
        });
    }

    public async Task Error(string message)
    {
        _logger.Error(message);
        await SaveLog(new Model.Log
        {
            Schema = string.Empty,
            Mensagem = message,
            Acontecimento = nameof(_logger.Error),
            StackTrace = string.Empty
        });
    }

    public async Task Fatal(Exception e, string message)
    {
        _logger.Fatal(e, message);
        await SaveLog(new Model.Log
        {
            Schema = string.Empty,
            Mensagem = message,
            Acontecimento = nameof(_logger.Fatal),
            StackTrace = $"{message}\n\n{e.Message}\n{e.StackTrace}"
        });
    }

    public async Task Fatal(string message)
    {
        _logger.Fatal(message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            StackTrace = string.Empty,
            Acontecimento = nameof(_logger.Fatal),
        });
    }

    public async Task Information(Exception e, string message)
    {
        _logger.Information(e, message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            Acontecimento = nameof(_logger.Information),
            StackTrace = $"{message}\n\n{e.Message}\n{e.StackTrace}"
        });
    }

    public async Task Information(string message)
    {
        _logger.Information(message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            StackTrace = string.Empty,
            Acontecimento = nameof(_logger.Information)
        });
    }

    public async Task Verbose(Exception e, string message)
    {
        _logger.Verbose(e, message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            Acontecimento = nameof(_logger.Verbose),
            StackTrace = $"{message}\n\n{e.Message}\n{e.StackTrace}"
        });
    }

    public async Task Verbose(string message)
    {
        _logger.Verbose(message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            StackTrace = string.Empty,
            Acontecimento = nameof(_logger.Verbose),
        });
    }

    public async Task Warning(Exception e, string message)
    {
        _logger.Warning(e, message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            Acontecimento = nameof(_logger.Warning),
            StackTrace = $"{message}\n\n{e.Message}\n{e.StackTrace}"
        });
    }

    public async Task Warning(string message)
    {
        _logger.Warning(message);
        await SaveLog(new Model.Log
        {
            Mensagem = message,
            Schema = string.Empty,
            StackTrace = string.Empty,
            Acontecimento = nameof(_logger.Warning),
        });
    }

    private async Task SaveLog(Model.Log log)
    {
        await _logContext.Set<Model.Log>().AddAsync(log);
        await _logContext.SaveChangesAsync();
    }
}