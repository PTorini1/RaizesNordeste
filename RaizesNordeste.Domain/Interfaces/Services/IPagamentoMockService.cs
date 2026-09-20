namespace RaizesNordeste.Domain.Interfaces.Services;

public interface IPagamentoMockService
{
    Task<(bool Sucesso, string TransacaoId, string PayloadEnvio, string PayloadRetorno)> ProcessarPagamentoMockAsync(
        int pedidoId,
        decimal valor,
        string provedor,
        CancellationToken cancellationToken = default);
}
