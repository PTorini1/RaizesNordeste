using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Estoques;

public class RegistrarMovimentacaoEstoqueUseCase(
    IEstoqueRepository estoqueRepository,
    IUsuarioRepository usuarioRepository,
    IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
    IAuditoriaRepository auditoriaRepository,
    EstoqueDomainService estoqueDomainService)
{
    public async Task<Estoque> ExecutarAsync(
        int estoqueId,
        int usuarioId,
        TipoMovimentacaoEstoque tipo,
        int quantidade,
        string motivo,
        CancellationToken cancellationToken = default)
    {
        var estoque = await estoqueRepository.GetByIdAsync(estoqueId, cancellationToken);
        _ = estoque ?? throw new DomainException($"Registro de estoque ID {estoqueId} não encontrado.");

        var usuario = await usuarioRepository.GetByIdAsync(usuarioId, cancellationToken);
        if (usuario is null || !usuario.Ativo)
        {
            throw new DomainException("Usuário inválido ou inativo.");
        }

        var quantidadeAnterior = estoqueDomainService.RegistrarMovimentacao(estoque, tipo, quantidade);
        new EstoqueValidator().ValidaOuLancaExcecao(estoque);
        await estoqueRepository.UpdateAsync(estoque, cancellationToken);

        var mov = new MovimentacaoEstoque
        {
            EstoqueId = estoque.Id,
            UsuarioId = usuarioId,
            TipoMovimentacao = tipo,
            Quantidade = quantidade,
            Motivo = motivo,
            CriadoEm = DateTime.UtcNow
        };
        new MovimentacaoEstoqueValidator().ValidaOuLancaExcecao(mov);
        await movimentacaoEstoqueRepository.InsertAsync(mov, cancellationToken);

        var auditoria = new Auditoria
        {
            UsuarioId = usuarioId,
            Entidade = "Estoque",
            EntidadeId = estoque.Id,
            Acao = "MOVIMENTACAO_ESTOQUE",
            DadosAnteriores = $"Quantidade anterior: {quantidadeAnterior}",
            DadosNovos = $"Quantidade nova: {estoque.QuantidadeAtual}, Tipo: {tipo}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);

        return estoque;
    }
}
