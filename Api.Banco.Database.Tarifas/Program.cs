using Api.Banco.Database.Tarifas.Application.Command;
using Api.Banco.Database.Tarifas.Context;
using Api.Banco.Database.Tarifas.Logs;
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

var connectionString = builder.Configuration.GetConnectionString("TarifasDb");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("--- ERRO CRÍTICO ---");
    Console.WriteLine("A ConnectionString 'TarifasDb' não foi encontrada no appsettings.json.");
    Console.WriteLine($"Caminho procurado: {Path.Combine(AppContext.BaseDirectory, "appsettings.json")}");
    return; 
}
builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddDbContext<TarifasDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateTarifaCommand).Assembly,
        typeof(Program).Assembly
    );
});
using IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TarifasDbContext>();

        var databaseCreator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

        if (databaseCreator != null)
        {
            if (!databaseCreator.Exists())
            {
                Console.WriteLine("Banco de dados não encontrado no MySQL. Criando schema...");
                databaseCreator.Create();
                Console.WriteLine("Schema do banco de dados criado com sucesso.");
            }
        }

        Console.WriteLine("Sincronizando tabelas com o banco de dados...");
        context.Database.Migrate();

        
        Console.WriteLine("##  Sucesso: Banco e Tabelas sincronizados.  ##");
        
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n--- ERRO NA INICIALIZAÇÃO ---");
        Console.WriteLine($"Mensagem: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"Detalhe Interno: {ex.InnerException.Message}");
        
    }
}

await host.RunAsync();