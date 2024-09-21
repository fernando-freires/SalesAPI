using Serilog;
using Microsoft.EntityFrameworkCore;
using Vendas.Data.Context;
using Vendas.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Lê as configurações do arquivo appsettings.json, se aplicável
    .Enrich.FromLogContext()
    .WriteTo.Console() // Log no console
    .WriteTo.File("logs/vendas_log.txt", rollingInterval: RollingInterval.Day) // Log em arquivo, separado por dias
    .CreateLogger();

builder.Host.UseSerilog(); // Usar Serilog como o logger padrão

// Adicionar serviços ao contêiner
builder.Services.AddDbContext<VendasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Registrar o IVendaService e VendaService no contêiner de DI
builder.Services.AddScoped<IVendaService, VendaService>();

// Configuração do JSON para lidar com ciclos de referência
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Iniciando a aplicação");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
}
finally
{
    Log.CloseAndFlush();
}
