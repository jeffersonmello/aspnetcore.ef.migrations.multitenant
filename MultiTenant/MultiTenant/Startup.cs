using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Context;
using MultiTenant.Repository;

namespace MultiTenant;

public static class Startup
{
    public static WebApplication InitializeApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
      
        var app = builder.Build();
        Configure(app);

        return app;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var masterConnectionString = builder.Configuration["Connections:Master"];
        var slaveDefaultConnectionString = builder.Configuration["Connections:SlaveDefault"];
        
        builder.Services.AddDbContext<MasterContext>(options => 
            options.UseMySql(masterConnectionString, new MySqlServerVersion(new Version(5, 7, 36)))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
        
        builder.Services.AddDbContext<SlaveContext>(options => 
            options.UseMySql(slaveDefaultConnectionString, new MySqlServerVersion(new Version(5, 7, 36)))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );

        builder.Services.AddRepository();
    }

    private static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}