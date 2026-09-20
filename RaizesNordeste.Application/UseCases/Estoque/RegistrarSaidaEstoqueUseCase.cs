using RaizesNordeste.Application.DTOs.Estoque;
using RaizesNordeste.Application.Services;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;

namespace RaizesNordeste.Application.UseCases.Estoques;

public class RegistrarSaidaEstoqueUseCase(
    IEstoqueRepository estoqueRepository,
    IProdutoRepository produtoRepository,
    RegistrarMovimentacaoEstoqueUseCase registrarMovimentacaoEstoqueUseCase,
    EstoqueAppService estoqueAppService,
    EstoqueDomainService estoqueDomainService,
    IUnitOfWork unitOfWork)
{
    public async Task<EstoqueResponse> ExecutarAsync(SaidaEstoqueRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await estoqueAppService.ValidarReferenciasAsync(
                request.UnidadeId, request.ProdutoId, request.UsuarioId, cancellationToken);

            var estoque = await estoqueRepository.ObterPorUnidadeEProdutoAsync(request.UnidadeId, request.ProdutoId, cancellationToken);
            _ = estoque ??
                throw new DomainException($"Registro de estoque não encontrado para a unidade ID {request.UnidadeId} e produto ID {request.ProdutoId}.");

            estoqueDomainService.ValidarSaida(request.TipoMovimentacao);

            var result = await registrarMovimentacaoEstoqueUseCase.ExecutarAsync(
                estoque.Id, request.UsuarioId, request.TipoMovimentacao,
                request.Quantidade, request.Motivo, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return await EstoqueResponseMapper.MapearAsync(result, produtoRepository, cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
