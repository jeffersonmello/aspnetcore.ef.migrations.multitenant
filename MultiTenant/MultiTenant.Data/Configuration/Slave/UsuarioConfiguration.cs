using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Abstract;
using MultiTenant.Model.Slave;

namespace MultiTenant.Data.Configuration.Slave;

public class UsuarioConfiguration : ConfigBase<Usuario>
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
        
        builder.Property(p => p.Login)
            .HasColumnName("login")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.Senha)
            .HasColumnName("senha")
            .HasMaxLength(36)
            .IsRequired();
    }

    protected override void ConfigureTableName()
    {
        builder.ToTable("cad_usuario");
    }
}