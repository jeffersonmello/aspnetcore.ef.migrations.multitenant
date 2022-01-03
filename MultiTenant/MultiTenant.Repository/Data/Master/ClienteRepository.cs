using MultiTenant.Data.Context;
using MultiTenant.Model.Master;
using MultiTenant.Repository.Abstract;
using MultiTenant.Repository.Contract.Master;

namespace MultiTenant.Repository.Data.Master;

public class ClienteRepository : RepositoryBase<Cliente, long>, IClienteRepository
{
    public ClienteRepository(MasterContext context) 
        : base(context)
    {
    }
}