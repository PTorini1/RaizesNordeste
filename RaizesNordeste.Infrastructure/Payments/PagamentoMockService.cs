using RaizesNordeste.Domain.Interfaces.Services;
using System.Text.Json;

namespace RaizesNordeste.Infrastructure.Payments;

public class PagamentoMockService : IPagamentoMockService
{
    public async Task<(bool Sucesso, string TransacaoId, string PayloadEnvio, string PayloadRetorno)> ProcessarPagamentoMockAsync(
        int pedidoId,
        decimal valor,
        string provedor,
        CancellationToken cancellationToken = default)
    {
        // Simulates network latency
        await Task.Delay(400, cancellationToken);

        var transacaoId = Guid.NewGuid().ToString("N");

        var envio = new
        {
            PedidoId = pedidoId,
            Valor = valor,
            Provedor = provedor,
            Timestamp = DateTime.UtcNow
        };

        // Simula aprovação para valores diferentes de 999.99, e recusa para esse valor específico
        bool sucesso = valor != 999.99m;

        var retorno = new
        {
            TransacaoId = transacaoId,
            Sucesso = sucesso,
            Status = sucesso ? "APPROVED" : "DECLINED",
            Mensagem = sucesso ? "Transação aprovada com sucesso." : "Limite insuficiente ou erro no cartão.",
            Timestamp = DateTime.UtcNow
        };

        var payloadEnvio = JsonSerializer.Serialize(envio);
        var payloadRetorno = JsonSerializer.Serialize(retorno);

        return (sucesso, transacaoId, payloadEnvio, payloadRetorno);
    }
}
