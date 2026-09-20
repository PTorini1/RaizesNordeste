using RaizesNordeste.Application.Common;
using RaizesNordeste.Application.DTOs.Fidelidade;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Fidelidade;

public class ObterSaldoFidelidadeUseCase(
    IContaFidelidadeRepository contaRepository,
    IMovimentacaoPontosRepository movimentacaoRepository,
    IClienteRepository clienteRepository,
    IUsuarioRepository usuarioRepository)
{
    public async Task<ResultadoOperacao<ContaFidelidadeResponse>> ExecutarAsync(
        int clienteId,
        UsuarioAutenticado? usuarioAutenticado = null,
        CancellationToken cancellationToken = default)
    {
        var titularidade = await ValidarTitularidadeAsync(clienteId, usuarioAutenticado, cancellationToken);
        if (titularidade is not null)
        {
            return titularidade;
        }

        var conta = await contaRepository.ObterPorClienteIdAsync(clienteId, cancellationToken);
        if (conta is null)
        {
            return ResultadoOperacao<ContaFidelidadeResponse>.NaoEncontrado(
                $"Conta fidelidade nao encontrada para o cliente ID {clienteId}.");
        }

        var cliente = await clienteRepository.GetByIdAsync(clienteId, cancellationToken);
        var clienteNome = $"Cliente {clienteId}";

        if (cliente is not null)
        {
            var usuario = await usuarioRepository.GetByIdAsync(cliente.UsuarioId, cancellationToken);
            clienteNome = usuario?.Nome ?? clienteNome;
        }

        var movimentacoes = await movimentacaoRepository.ListAsync(
            predicate: m => m.ContaFidelidadeId == conta.Id,
            orderBy: q => q.OrderByDescending(m => m.CriadoEm),
            cancellationToken: cancellationToken);

        return ResultadoOperacao<ContaFidelidadeResponse>.Sucesso(new ContaFidelidadeResponse
        {
            Id = conta.Id,
            ClienteId = conta.ClienteId,
            ClienteNome = clienteNome,
            SaldoPontos = conta.SaldoPontos,
            Ativa = conta.Ativa,
            CriadoEm = conta.CriadoEm,
            Movimentacoes = movimentacoes.Select(m => new MovimentacaoPontosResponse
            {
                Id = m.Id,
                PedidoId = m.PedidoId,
                TipoMovimentacao = m.TipoMovimentacao.ToString().ToUpper(),
                Pontos = m.Pontos,
                Descricao = m.Descricao,
                CriadoEm = m.CriadoEm
            }).ToList()
        });
    }

    private async Task<ResultadoOperacao<ContaFidelidadeResponse>?> ValidarTitularidadeAsync(
        int clienteId,
        UsuarioAutenticado? usuarioAutenticado,
        CancellationToken cancellationToken)
    {
        if (usuarioAutenticado?.EhCliente != true)
        {
            return null;
        }

        if (!usuarioAutenticado.UsuarioId.HasValue)
        {
            return ResultadoOperacao<ContaFidelidadeResponse>.Proibido(
                "Operacao nao permitida: cliente autenticado nao encontrado.",
                "CLIENTE_NAO_ENCONTRADO");
        }

        var cliente = await clienteRepository.ObterPorUsuarioIdAsync(usuarioAutenticado.UsuarioId.Value, cancellationToken);
        return cliente is not null && cliente.Id == clienteId
            ? null
            : ResultadoOperacao<ContaFidelidadeResponse>.Proibido(
                "Operacao nao permitida: o cliente autenticado so pode consultar o seu proprio saldo de fidelidade.",
                "TITULARIDADE_INVALIDA");
    }
}
