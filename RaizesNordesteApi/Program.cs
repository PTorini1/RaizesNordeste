
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using RaizesNordeste.Application;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Infrastructure;
using RaizesNordesteApi.Configurations;
using RaizesNordesteApi.Extensions;
using RaizesNordesteApi.Filters;
using RaizesNordesteApi.Handlers;
using RaizesNordesteApi.Middlewares;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Logging

builder.Host.UseSerilog((context, _, loggerConfiguration) =>
{
    loggerConfiguration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "RaizesNordesteApi")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .Enrich.WithProperty("MachineName", Environment.MachineName)
        .Enrich.WithProperty("ProcessId", Environment.ProcessId)
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] [{Application}] [{Environment}] {Message:lj}{NewLine}{Exception}");
});

#endregion

#region Camadas da Aplicação

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

#endregion

#region Autenticação e Autorização

builder.Services.AddAuthentication("Firebase")
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>("Firebase", null);
builder.Services.AddAuthorization();

#endregion

#region Controllers e Serialização

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilter>())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var erros = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

        var resposta = FabricaErroApi.Criar(
            context.HttpContext,
            StatusCodes.Status422UnprocessableEntity,
            "ERRO_VALIDACAO",
            "A requisição contém campos inválidos.",
            erros);

        return new UnprocessableEntityObjectResult(resposta);
    };
});

builder.Services.AddScoped<ResultadoHttpHandler>();

#endregion

#region Documentação (Swagger)

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Raizes Nordeste API",
        Version = "v1",
        Description = "API para gestão de unidades, cardápios, pedidos, estoque, fidelidade, promoções e pagamentos."
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira apenas o token JWT. O Swagger adiciona o prefixo Bearer automaticamente."
    });

    options.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

#endregion

#region Telemetria (OpenTelemetry + Prometheus)

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(
        serviceName: "RaizesNordesteApi",
        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0"))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("Microsoft.AspNetCore.Hosting")
            .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
            .AddPrometheusExporter();
    });

#endregion

var app = builder.Build();

#region Inicialização do Banco de Dados

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dataStoreInitializer = services.GetRequiredService<IDataStoreInitializerService>();
        await dataStoreInitializer.InitializeAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao inicializar o banco de dados.");
    }
}

#endregion

#region Pipeline HTTP

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Raizes Nordeste API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Raizes Nordeste API";
});

app.MapPrometheusScrapingEndpoint("/metrics");

app.UseHttpsRedirection();

app.UseMiddleware<RegistroAcessoEndpointMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
