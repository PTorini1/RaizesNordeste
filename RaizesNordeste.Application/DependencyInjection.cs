using Microsoft.Extensions.DependencyInjection;
using RaizesNordeste.Application.Services;
using RaizesNordeste.Application.UseCases.Auditorias;
using RaizesNordeste.Application.UseCases.Autenticacao;
using RaizesNordeste.Application.UseCases.Cardapio;
using RaizesNordeste.Application.UseCases.Estoques;
using RaizesNordeste.Application.UseCases.Fidelidade;
using RaizesNordeste.Application.UseCases.Pagamentos;
using RaizesNordeste.Application.UseCases.Pedidos;
using RaizesNordeste.Application.UseCases.Produtos;
using RaizesNordeste.Application.UseCases.Promocoes;
using RaizesNordeste.Application.UseCases.Seed;
using RaizesNordeste.Application.UseCases.Unidades;
using RaizesNordeste.Application.UseCases.Usuarios;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region Domain Services

        services.AddScoped<UsuarioDomainService>();
        services.AddScoped<UnidadeDomainService>();
        services.AddScoped<ProdutoDomainService>();
        services.AddScoped<PedidoDomainService>();
        services.AddScoped<PagamentoDomainService>();

        #endregion

        #region Application Services

        services.AddScoped<EstoqueAppService>();

        #endregion

        #region Casos de Uso - Autenticação

        services.AddScoped<AutenticarUsuarioUseCase>();
        services.AddScoped<AutenticarUsuarioEmailSenhaUseCase>();

        #endregion

        #region Casos de Uso - Usuários

        services.AddScoped<CriarUsuarioUseCase>();
        services.AddScoped<ListarUsuariosUseCase>();
        services.AddScoped<AlterarRoleUsuarioUseCase>();
        services.AddScoped<ObterUsuarioPorEmailUseCase>();

        #endregion

        #region Casos de Uso - Unidade

        services.AddScoped<CriarUnidadeUseCase>();
        services.AddScoped<AtualizarUnidadeUseCase>();
        services.AddScoped<DesativarUnidadeUseCase>();
        services.AddScoped<ListarUnidadesUseCase>();
        services.AddScoped<ObterUnidadePorIdUseCase>();

        #endregion

        #region Casos de Uso - Produto

        services.AddScoped<CriarProdutoUseCase>();
        services.AddScoped<AtualizarProdutoUseCase>();
        services.AddScoped<AtualizarParcialProdutoUseCase>();
        services.AddScoped<ObterProdutoPorIdUseCase>();
        services.AddScoped<ListarProdutosUseCase>();
        services.AddScoped<RemoverProdutoUseCase>();

        #endregion

        #region Casos de Uso - Cardápio

        services.AddScoped<AssociarProdutoCardapioUseCase>();
        services.AddScoped<AtualizarProdutoCardapioUseCase>();
        services.AddScoped<RemoverProdutoCardapioUseCase>();
        services.AddScoped<ConsultarCardapioUnidadeUseCase>();

        #endregion

        #region Casos de Uso - Estoque

        services.AddScoped<ConsultarEstoqueUnidadeUseCase>();
        services.AddScoped<ConsultarEstoqueProdutoUseCase>();
        services.AddScoped<RegistrarMovimentacaoEstoqueUseCase>();
        services.AddScoped<RegistrarEntradaEstoqueUseCase>();
        services.AddScoped<RegistrarSaidaEstoqueUseCase>();

        #endregion

        #region Casos de Uso - Pedido

        services.AddScoped<CriarPedidoUseCase>();
        services.AddScoped<AtualizarStatusPedidoUseCase>();
        services.AddScoped<CancelarPedidoUseCase>();
        services.AddScoped<ListarPedidosUseCase>();
        services.AddScoped<ObterPedidoPorIdUseCase>();

        #endregion

        #region Casos de Uso - Pagamento

        services.AddScoped<SolicitarPagamentoUseCase>();
        services.AddScoped<ObterPagamentoPorIdUseCase>();
        services.AddScoped<ObterPagamentoPorPedidoIdUseCase>();

        #endregion

        #region Casos de Uso - Fidelidade

        services.AddScoped<AderirFidelidadeUseCase>();
        services.AddScoped<AplicarPontosFidelidadeUseCase>();
        services.AddScoped<ResgatarPontosUseCase>();
        services.AddScoped<ObterSaldoFidelidadeUseCase>();

        #endregion

        #region Casos de Uso - Promoções

        services.AddScoped<CriarPromocaoUseCase>();
        services.AddScoped<ListarPromocoesUseCase>();
        services.AddScoped<ListarPromocoesAtivasUseCase>();

        #endregion

        #region Casos de Uso - Auditoria

        services.AddScoped<ListarAuditoriasUseCase>();
        services.AddScoped<ListarAuditoriasPorEntidadeUseCase>();

        #endregion

        #region Casos de Uso - Seed

        services.AddScoped<SeedDatabaseUseCase>();

        #endregion

        return services;
    }
}
