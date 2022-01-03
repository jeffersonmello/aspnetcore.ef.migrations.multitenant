using MultiTenant.Data.Context;
using MultiTenant.Model.Slave;
using MultiTenant.Repository.Abstract;
using MultiTenant.Repository.Contract.Slave;

namespace MultiTenant.Repository.Data.Slave;

public class UsuarioRepository : RepositoryBase<Usuario, long>, IUsuarioRepository
{
    public UsuarioRepository(SlaveContext context) 
        : base(context)
    {
    }
}