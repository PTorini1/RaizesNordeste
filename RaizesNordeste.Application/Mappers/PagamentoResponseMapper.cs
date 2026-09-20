using RaizesNordeste.Application.DTOs.Pagamentos;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Application.UseCases;

internal static class PagamentoResponseMapper
{
    public static PagamentoResponse Mapear(Pagamento pagamento)
    {
        return new PagamentoResponse
        {
            Id = pagamento.Id,
            PedidoId = pagamento.PedidoId,
            Provedor = pagamento.Provedor,
            Status = pagamento.Status.ToString().ToUpper(),
            TransacaoExternaId = pagamento.TransacaoExternaId,
            SolicitadoEm = pagamento.SolicitadoEm,
            RespondidoEm = pagamento.RespondidoEm
        };
    }
}
