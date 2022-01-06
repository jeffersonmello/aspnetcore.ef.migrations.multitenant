using MultiTenant.Model.Master;

namespace MultiTenant.Tools.MigrationTool;

public interface IMigrationHelper
{
    Task MigrateTenantDatabase(Cliente tenant, string defaultConnection);

    Task<List<Cliente>> GetConfiguredTenants();
}