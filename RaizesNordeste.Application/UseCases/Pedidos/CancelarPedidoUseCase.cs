using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Application.Mappers;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Pedidos;

public class CancelarPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IEstoqueRepository estoqueRepository,
    IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
    IAuditoriaRepository auditoriaRepository,
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork,
    EstoqueDomainService estoqueDomainService,
    PedidoDomainService pedidoDomainService)
{
    public async Task<PedidoResponse> ExecutarAsync(int pedidoId, int usuarioId, string motivo, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var (pedido, statusAnterior) = await ValidarEIniciarCancelamentoAsync(pedidoId, usuarioId, motivo, cancellationToken);

            if (pedido.Status == StatusPedido.Cancelado)
            {
                await unitOfWork.CommitAsync(cancellationToken);
                return UtilPedidoMapper.MapearParaResponse(pedido);
            }

            await EstornarEstoqueAsync(pedido, usuarioId, motivo, cancellationToken);
            await pedidoRepository.UpdateAsync(pedido, cancellationToken);
            await RegistrarAuditoriaAsync(pedido, usuarioId, motivo, statusAnterior, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            return UtilPedidoMapper.MapearParaResponse(pedido);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<(Pedido Pedido, StatusPedido StatusAnterior)> ValidarEIniciarCancelamentoAsync(
        int pedidoId,
        int usuarioId,
        string motivo,
        CancellationToken cancellationToken)
    {
        var pedido = await pedidoRepository.ObterPorIdComItensAsync(pedidoId, cancellationToken);
        pedidoDomainService.ValidarPodeCancelar(pedido, pedidoId);

        var usuario = await usuarioRepository.GetByIdAsync(usuarioId, cancellationToken);
        pedidoDomainService.ValidarUsuarioResponsavel(usuario);

        var statusAnterior = pedido!.Status;

        if (statusAnterior != StatusPedido.Cancelado)
        {
            pedido.Cancelar(usuarioId, motivo);
        }

        return (pedido, statusAnterior);
    }

    private async Task EstornarEstoqueAsync(Pedido pedido, int usuarioId, string motivo, CancellationToken cancellationToken)
    {
        foreach (var item in pedido.Itens)
        {
            var estoque = await estoqueRepository.ObterPorUnidadeEProdutoAsync(
                pedido.UnidadeId, item.ProdutoId, cancellationToken);

            if (estoque is null) continue;

            estoqueDomainService.ReporEstoque(estoque, item.Quantidade);
            await estoqueRepository.UpdateAsync(estoque, cancellationToken);

            var movimentacao = pedidoDomainService.CriarMovimentacaoEstornoAjuste(
                estoque.Id, usuarioId, item.Quantidade, pedido.Id, motivo);
            await movimentacaoEstoqueRepository.InsertAsync(movimentacao, cancellationToken);
        }
    }

    private async Task RegistrarAuditoriaAsync(
        Pedido pedido,
        int usuarioId,
        string motivo,
        StatusPedido statusAnterior,
        CancellationToken cancellationToken)
    {
        var auditoria = new Auditoria
        {
            UsuarioId = usuarioId,
            Entidade = "Pedido",
            EntidadeId = pedido.Id,
            Acao = "CANCELAR",
            DadosAnteriores = $"Status anterior: {statusAnterior}",
            DadosNovos = $"Status novo: Cancelado. Motivo: {motivo}. Responsavel ID: {usuarioId}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);
    }
}
