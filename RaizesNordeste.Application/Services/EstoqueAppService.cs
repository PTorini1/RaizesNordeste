using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.Services;

/// <summary>
/// Application Service de Estoque: validações que precisam de acesso a repositórios.
/// Regras puras (sem I/O) permanecem no EstoqueDomainService.
/// </summary>
public class EstoqueAppService(
    IUnidadeRepository unidadeRepository,
    IProdutoRepository produtoRepository,
    IUsuarioRepository usuarioRepository)
{
    public async Task ValidarReferenciasAsync(
        int unidadeId,
        int produtoId,
        int usuarioId,
        CancellationToken cancellationToken)
    {
        var unidade = await unidadeRepository.GetByIdAsync(unidadeId, cancellationToken);
        if (unidade is null || !unidade.Ativa)
        {
            throw new DomainException("Unidade inválida ou inativa.");
        }

        var produto = await produtoRepository.GetByIdAsync(produtoId, cancellationToken);
        if (produto is null || !produto.Ativo)
        {
            throw new DomainException("Produto inválido ou inativo.");
        }

        var usuario = await usuarioRepository.GetByIdAsync(usuarioId, cancellationToken);
        if (usuario is null || !usuario.Ativo)
        {
            throw new DomainException("Usuário inválido ou inativo.");
        }
    }
}
