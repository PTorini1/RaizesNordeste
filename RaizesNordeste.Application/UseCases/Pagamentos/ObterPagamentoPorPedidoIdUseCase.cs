using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Pagamentos;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Pagamentos;

public class ObterPagamentoPorPedidoIdUseCase(
    IPagamentoRepository pagamentoRepository,
    IPedidoRepository pedidoRepository,
    IClienteRepository clienteRepository)
{
    public async Task<ResultadoOperacao<PagamentoResponse>> ExecutarAsync(
        int pedidoId,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        if (usuarioAutenticado?.EhCliente == true)
        {
            var permitido = await ClientePodeAcessarPedidoAsync(usuarioAutenticado, pedidoId, cancellationToken);
            if (!permitido)
            {
                return ResultadoOperacao<PagamentoResponse>.Proibido(
                    "Operacao nao permitida: o cliente autenticado so pode consultar os seus proprios pagamentos.",
                    "TITULARIDADE_INVALIDA");
            }
        }

        var pagamento = await pagamentoRepository.ObterPorPedidoIdAsync(pedidoId, cancellationToken);
        return pagamento is null
            ? ResultadoOperacao<PagamentoResponse>.NaoEncontrado($"Pagamento para o pedido ID {pedidoId} nao encontrado.")
            : ResultadoOperacao<PagamentoResponse>.Sucesso(PagamentoResponseMapper.Mapear(pagamento));
    }

    private async Task<bool> ClientePodeAcessarPedidoAsync(
        UsuarioAutenticado usuarioAutenticado,
        int pedidoId,
        CancellationToken cancellationToken)
    {
        if (!usuarioAutenticado.UsuarioId.HasValue)
        {
            return false;
        }

        var cliente = await clienteRepository.ObterPorUsuarioIdAsync(usuarioAutenticado.UsuarioId.Value, cancellationToken);
        var pedido = await pedidoRepository.GetByIdAsync(pedidoId, cancellationToken);

        return cliente is not null && pedido is not null && pedido.ClienteId == cliente.Id;
    }
}
