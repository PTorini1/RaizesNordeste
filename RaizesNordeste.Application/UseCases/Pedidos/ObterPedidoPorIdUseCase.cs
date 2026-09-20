using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Pedidos;
using RaizesNordeste.Application.Mappers;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Pedidos;

public class ObterPedidoPorIdUseCase(
    IPedidoRepository pedidoRepository,
    IClienteRepository clienteRepository)
{
    public async Task<ResultadoOperacao<PedidoResponse>> ExecutarAsync(
        int id,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdComItensAsync(id, cancellationToken);
        if (pedido is null)
        {
            return ResultadoOperacao<PedidoResponse>.NaoEncontrado($"Pedido ID {id} nao encontrado.");
        }

        if (usuarioAutenticado?.EhCliente == true)
        {
            var autorizado = await ClientePodeAcessarAsync(usuarioAutenticado, pedido.ClienteId, cancellationToken);
            if (!autorizado)
            {
                return ResultadoOperacao<PedidoResponse>.Proibido(
                    "Operacao nao permitida: o cliente autenticado so pode consultar os seus proprios pedidos.",
                    "TITULARIDADE_INVALIDA");
            }
        }

        return ResultadoOperacao<PedidoResponse>.Sucesso(UtilPedidoMapper.MapearParaResponse(pedido));
    }

    private async Task<bool> ClientePodeAcessarAsync(
        UsuarioAutenticado usuarioAutenticado,
        int? clienteId,
        CancellationToken cancellationToken)
    {
        if (!usuarioAutenticado.UsuarioId.HasValue || !clienteId.HasValue)
        {
            return false;
        }

        var cliente = await clienteRepository.ObterPorUsuarioIdAsync(usuarioAutenticado.UsuarioId.Value, cancellationToken);
        return cliente is not null && cliente.Id == clienteId.Value;
    }
}
