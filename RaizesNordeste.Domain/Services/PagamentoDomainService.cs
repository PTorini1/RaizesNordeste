using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class PagamentoDomainService
{
    public void ValidarPedidoParaPagamento(Pedido? pedido, int pedidoId) =>
        _ = pedido ?? throw new DomainException($"Pedido ID {pedidoId} não encontrado.");

    public void ValidarIdempotencia(Pagamento? pagamentoExistente, string chaveIdempotencia)
    {
        if (pagamentoExistente is not null && pagamentoExistente.ChaveIdempotencia == chaveIdempotencia)
        {
            return;
        }

        throw new DomainException("Transação de pagamento duplicada em andamento ou chave de idempotência expirada.");
    }

    public Pagamento CriarPagamento(int pedidoId, string provedor, string chaveIdempotencia)
    {
        var pagamento = new Pagamento
        {
            PedidoId = pedidoId,
            Provedor = provedor,
            Status = StatusPagamento.Solicitado,
            ChaveIdempotencia = chaveIdempotencia,
            SolicitadoEm = DateTime.UtcNow
        };

        new PagamentoValidator().ValidaOuLancaExcecao(pagamento);
        return pagamento;
    }

    public void AtualizarAposProcessamento(
        Pagamento pagamento,
        bool sucesso,
        string transacaoId,
        string payloadEnvio,
        string payloadRetorno)
    {
        pagamento.TransacaoExternaId = transacaoId;
        pagamento.PayloadEnvio = payloadEnvio;
        pagamento.PayloadRetorno = payloadRetorno;
        pagamento.Status = sucesso ? StatusPagamento.Aprovado : StatusPagamento.Recusado;
        pagamento.RespondidoEm = DateTime.UtcNow;

        new PagamentoValidator().ValidaOuLancaExcecao(pagamento);
    }

    public void MarcarComoErro(Pagamento pagamento, string mensagemErro)
    {
        pagamento.Status = StatusPagamento.Erro;
        pagamento.PayloadRetorno = mensagemErro;
        pagamento.RespondidoEm = DateTime.UtcNow;
    }
}
