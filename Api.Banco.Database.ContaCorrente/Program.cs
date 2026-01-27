using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Logs;
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

var connectionString = builder.Configuration.GetConnectionString("ContaCorrenteDb");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine($"Caminho procurado: {Path.Combine(AppContext.BaseDirectory, "appsettings.json")}");
    return; 
}
builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

using IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

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
        if (ex.InnerException != null)
            Console.WriteLine($"Detalhe Interno: {ex.InnerException.Message}");
    }
}

await host.RunAsync();