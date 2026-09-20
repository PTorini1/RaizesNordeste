using RaizesNordeste.Application.DTOs.Estoque;
using RaizesNordeste.Application.Services;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Estoques;

public class RegistrarEntradaEstoqueUseCase(
    IEstoqueRepository estoqueRepository,
    IProdutoRepository produtoRepository,
    RegistrarMovimentacaoEstoqueUseCase registrarMovimentacaoEstoqueUseCase,
    EstoqueAppService estoqueAppService,
    IUnitOfWork unitOfWork)
{
    public async Task<EstoqueResponse> ExecutarAsync(EntradaEstoqueRequest request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await estoqueAppService.ValidarReferenciasAsync(
                request.UnidadeId, request.ProdutoId, request.UsuarioId, cancellationToken);

            var estoque = await estoqueRepository.ObterPorUnidadeEProdutoAsync(request.UnidadeId, request.ProdutoId, cancellationToken);

            if (estoque is null)
            {
                var novoEstoque = new Estoque
                {
                    UnidadeId = request.UnidadeId,
                    ProdutoId = request.ProdutoId,
                    QuantidadeAtual = 0,
                    QuantidadeMinima = 5,
                    AtualizadoEm = DateTime.UtcNow
                };
                new EstoqueValidator().ValidaOuLancaExcecao(novoEstoque);
                estoque = await estoqueRepository.InsertAsync(novoEstoque, cancellationToken);
            }

            var result = await registrarMovimentacaoEstoqueUseCase.ExecutarAsync(
                estoque.Id, request.UsuarioId, TipoMovimentacaoEstoque.Entrada,
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
