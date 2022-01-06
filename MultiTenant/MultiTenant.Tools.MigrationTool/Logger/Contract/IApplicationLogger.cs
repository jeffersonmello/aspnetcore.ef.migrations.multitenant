namespace MultiTenant.Tools.MigrationTool.Logger.Contract;

public interface IApplicationLogger
{
    Task Error(Exception e, string message);

    Task Error(string message);

    Task Fatal(Exception e, string message);

    Task Fatal(string message);

    Task Information(string message);

    Task Verbose(string message);

    Task Warning(Exception e, string message);

    Task Warning(string message);
}