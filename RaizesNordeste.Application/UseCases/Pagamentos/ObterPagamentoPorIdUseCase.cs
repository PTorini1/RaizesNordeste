using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Pagamentos;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Pagamentos;

public class ObterPagamentoPorIdUseCase(
    IPagamentoRepository pagamentoRepository,
    IPedidoRepository pedidoRepository,
    IClienteRepository clienteRepository)
{
    public async Task<ResultadoOperacao<PagamentoResponse>> ExecutarAsync(
        int id,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        var pagamento = await pagamentoRepository.GetByIdAsync(id, cancellationToken);
        if (pagamento is null)
        {
            return ResultadoOperacao<PagamentoResponse>.NaoEncontrado($"Pagamento ID {id} nao encontrado.");
        }

        if (usuarioAutenticado?.EhCliente == true)
        {
            var permitido = await ClientePodeAcessarPedidoAsync(usuarioAutenticado, pagamento.PedidoId, cancellationToken);
            if (!permitido)
            {
                return ResultadoOperacao<PagamentoResponse>.Proibido(
                    "Operacao nao permitida: o cliente autenticado so pode consultar os seus proprios pagamentos.",
                    "TITULARIDADE_INVALIDA");
            }
        }

        return ResultadoOperacao<PagamentoResponse>.Sucesso(PagamentoResponseMapper.Mapear(pagamento));
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
