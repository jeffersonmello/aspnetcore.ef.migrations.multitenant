using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Abstract;

namespace MultiTenant.Data.Configuration.Log;

public class LogConfiguration : ConfigBase<Model.Log>
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

        builder.Property(p => p.DataHora)
            .HasDefaultValue(DateTime.Now)
            .HasColumnName("datahora");

        builder.Property(p => p.Schema)
            .HasColumnName("schema")
            .IsRequired();
        
        builder.Property(p => p.Acontecimento)
            .HasColumnName("acontecimento");
        
        builder.Property(p => p.Mensagem)
            .HasColumnName("mensagem");
        
        builder.Property(p => p.StackTrace)
            .HasColumnName("stack_trace");
    }

    protected override void ConfigureTableName()
    {
        builder.ToTable("logs");
    }
}