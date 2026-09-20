using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Estoques;

internal static class EstoqueUseCaseValidacoes
{
    public static async Task ValidarReferenciasAsync(
        int unidadeId,
        int produtoId,
        int usuarioId,
        IUnidadeRepository unidadeRepository,
        IProdutoRepository produtoRepository,
        IUsuarioRepository usuarioRepository,
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
