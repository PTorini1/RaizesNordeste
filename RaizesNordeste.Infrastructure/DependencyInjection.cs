using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Infrastructure.Cache;
using RaizesNordeste.Infrastructure.Firebase;
using RaizesNordeste.Infrastructure.Payments;
using RaizesNordeste.Infrastructure.Persistence;
using RaizesNordeste.Infrastructure.Repositories;
using StackExchange.Redis;

namespace RaizesNordeste.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<RaizesNordesteDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure()));

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")
                ?? "localhost:6379"));

        services.AddScoped(typeof(IGenericRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRedisRepository<>), typeof(RedisRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IUnidadeRepository, UnidadeRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<ICardapioUnidadeRepository, CardapioUnidadeRepository>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IContaFidelidadeRepository, ContaFidelidadeRepository>();
        services.AddScoped<IMovimentacaoPontosRepository, MovimentacaoPontosRepository>();
        services.AddScoped<IConsentimentoLGPDRepository, ConsentimentoLGPDRepository>();
        services.AddScoped<IPromocaoRepository, PromocaoRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

        services.AddScoped<IDataStoreInitializerService, DataStoreInitializerService>();
        services.AddScoped<IDataStoreResetService, DataStoreResetService>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IPagamentoMockService, PagamentoMockService>();
        services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();

        services.AddScoped<CardapioDomainService>();
        services.AddScoped<EstoqueDomainService>();
        services.AddScoped<FidelidadeDomainService>();
        services.AddScoped<PromocaoDomainService>();

        return services;
    }
}
