using RaizesNordeste.Application.DTOs.Estoque;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;

namespace RaizesNordeste.Application.UseCases.Estoques;

public class ConsultarEstoqueUnidadeUseCase(
    IEstoqueRepository estoqueRepository,
    IProdutoRepository produtoRepository,
    IUnidadeRepository unidadeRepository)
{
    public async Task<IReadOnlyList<EstoqueResponse>> ExecutarAsync(int unidadeId, CancellationToken cancellationToken = default)
    {
        var unidade = await unidadeRepository.GetByIdAsync(unidadeId, cancellationToken);
        _ = unidade ?? throw new DomainException($"Unidade ID {unidadeId} não encontrada.");

        var estoques = await estoqueRepository.ListAsync(
            predicate: e => e.UnidadeId == unidadeId,
            cancellationToken: cancellationToken);

        var response = new List<EstoqueResponse>();
        foreach (var estoque in estoques)
        {
            response.Add(await EstoqueResponseMapper.MapearAsync(estoque, produtoRepository, cancellationToken));
        }

        return response;
    }
}
