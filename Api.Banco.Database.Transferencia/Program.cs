
using Api.Banco.Database.Transferencia.Context;
using Api.Banco.Database.Transferencia.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


var connectionString = builder.Configuration.GetConnectionString("UsuariosDb");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("--- ERRO CRÍTICO ---");
    Console.WriteLine("A ConnectionString 'UsuariosDb' não foi encontrada no appsettings.json.");
    return;
}


builder.Services.AddSingleton<AuditInterceptor>();


builder.Services.AddDbContext<TransferenciaDbContext>((sp, options) =>
{
    var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .AddInterceptors(auditInterceptor); 
});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

using IHost host = builder.Build();


using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TransferenciaDbContext>();
        var databaseCreator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

        if (databaseCreator != null)
        {
            if (!databaseCreator.Exists())
            {
                
                databaseCreator.Create();
            }
        }

        
        context.Database.Migrate();

        
        
        
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Mensagem: {ex.Message}");
        
    }
}

await host.RunAsync();