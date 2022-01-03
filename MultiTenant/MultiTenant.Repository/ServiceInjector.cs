using Microsoft.Extensions.DependencyInjection;
using MultiTenant.Repository.Contract.Master;
using MultiTenant.Repository.Contract.Slave;
using MultiTenant.Repository.Data.Master;
using MultiTenant.Repository.Data.Slave;

namespace MultiTenant.Repository;

public static class ServiceInjector
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
        
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
    }
}