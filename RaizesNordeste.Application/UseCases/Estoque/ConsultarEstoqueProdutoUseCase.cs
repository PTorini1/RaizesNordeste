using RaizesNordeste.Application.DTOs.Estoque;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Estoques;

public class ConsultarEstoqueProdutoUseCase(
    IEstoqueRepository estoqueRepository,
    IProdutoRepository produtoRepository,
    IUnidadeRepository unidadeRepository)
{
    public async Task<EstoqueResponse?> ExecutarAsync(int unidadeId, int produtoId, CancellationToken cancellationToken = default)
    {
        var unidade = await unidadeRepository.GetByIdAsync(unidadeId, cancellationToken);
        _ = unidade ?? throw new DomainException($"Unidade ID {unidadeId} não encontrada.");

        var estoque = await estoqueRepository.ObterPorUnidadeEProdutoAsync(unidadeId, produtoId, cancellationToken);

        return estoque is null
            ? null
            : await EstoqueResponseMapper.MapearAsync(estoque, produtoRepository, cancellationToken);
    }
}
