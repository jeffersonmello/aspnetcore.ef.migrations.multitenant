using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MultiTenant.Data.Abstract;


public abstract class ConfigBase<TEntity> : IEntityTypeConfiguration<TEntity> 
    where TEntity : class
{
    protected EntityTypeBuilder<TEntity> builder;
    
    protected abstract void ConfigureFK();

    protected abstract void ConfigurePK();

    protected abstract void ConfigureFieldsTable();

    protected abstract void ConfigureTableName();

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        this.builder = builder;
        ConfigureTableName();
        ConfigureFieldsTable();
        ConfigurePK();
        ConfigureFK();
    }
}