using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Abstract;
using MultiTenant.Model.Master;

namespace MultiTenant.Data.Configuration.Master;

public class ClienteConfiguration : ConfigBase<Cliente>
{
    protected override void ConfigureFK()
    {
    }

    protected override void ConfigurePK()
    {
        builder.HasKey(pk => pk.Id);
    }

    protected override void ConfigureFieldsTable()
    {
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(p => p.Nome)
            .HasColumnName("nome")
            .HasMaxLength(155)
            .IsRequired();
        
        builder.Property(p => p.Schema)
            .HasColumnName("schema")
            .HasMaxLength(50)
            .IsRequired();
    }

    protected override void ConfigureTableName()
    {
        builder.ToTable("tab_cliente");
    }
}