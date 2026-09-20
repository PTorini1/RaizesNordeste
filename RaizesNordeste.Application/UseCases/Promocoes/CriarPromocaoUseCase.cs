using RaizesNordeste.Application.DTOs.Promocoes;
using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Application.UseCases.Promocoes;

public class CriarPromocaoUseCase(
    IPromocaoRepository promocaoRepository,
    IAuditoriaRepository auditoriaRepository,
    IProdutoRepository produtoRepository,
    PromocaoDomainService promocaoDomainService,
    ProdutoDomainService produtoDomainService)
{
    public async Task<PromocaoResponse> ExecutarAsync(CriarPromocaoRequest request, CancellationToken cancellationToken = default)
    {
        if (request.ProdutoId.HasValue)
        {
            var produto = await produtoRepository.GetByIdAsync(request.ProdutoId.Value, cancellationToken);
            produtoDomainService.ValidarAtivo(produto);
        }

        var promocao = promocaoDomainService.CriarPromocao(
            request.Nome, request.Descricao, request.TipoDesconto, request.ValorDesconto,
            request.InicioVigencia, request.FimVigencia, request.ProdutoId,
            request.CanalPedido, request.PerfilCliente);

        var salvo = await promocaoRepository.InsertAsync(promocao, cancellationToken);

        var auditoria = new Auditoria
        {
            UsuarioId = null,
            Entidade = "Promocao",
            EntidadeId = salvo.Id,
            Acao = "CRIAR_PROMOCAO",
            DadosNovos = $"Promocao {salvo.Nome} criada. Tipo: {salvo.TipoDesconto}, Valor: {salvo.ValorDesconto}",
            CriadoEm = DateTime.UtcNow
        };
        new AuditoriaValidator().ValidaOuLancaExcecao(auditoria);
        await auditoriaRepository.InsertAsync(auditoria, cancellationToken);

        return new PromocaoResponse
        {
            Id = salvo.Id,
            Nome = salvo.Nome,
            Descricao = salvo.Descricao,
            TipoDesconto = salvo.TipoDesconto,
            ValorDesconto = salvo.ValorDesconto,
            InicioVigencia = salvo.InicioVigencia,
            FimVigencia = salvo.FimVigencia,
            Ativa = salvo.Ativa,
            ProdutoId = salvo.ProdutoId,
            CanalPedido = salvo.CanalPedido?.ToString().ToUpper(),
            PerfilCliente = salvo.PerfilCliente
        };
    }
}
