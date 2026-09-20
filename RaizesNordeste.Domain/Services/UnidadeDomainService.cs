using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class UnidadeDomainService
{
    public Unidade CriarUnidade(string nome, string cidade, string estado, string endereco, string tipoOperacao)
    {
        var unidade = new Unidade
        {
            Nome = nome,
            Cidade = cidade,
            Estado = estado,
            Endereco = endereco,
            TipoOperacao = tipoOperacao,
            Ativa = true
        };

        new UnidadeValidator().ValidaOuLancaExcecao(unidade);
        return unidade;
    }

    public void AtualizarUnidade(Unidade unidade, string nome, string cidade, string estado, string endereco, string tipoOperacao, bool ativa)
    {
        unidade.Nome = nome;
        unidade.Cidade = cidade;
        unidade.Estado = estado;
        unidade.Endereco = endereco;
        unidade.TipoOperacao = tipoOperacao;
        unidade.Ativa = ativa;

        new UnidadeValidator().ValidaOuLancaExcecao(unidade);
    }

    public void Desativar(Unidade unidade) =>
        unidade.Ativa = false;

    public void ValidarExistente(Unidade? unidade, int id) =>
        _ = unidade ?? throw new DomainException($"Unidade ID {id} não encontrada.");

    public void ValidarAtiva(Unidade? unidade)
    {
        if (unidade is null || !unidade.Ativa)
        {
            throw new DomainException("Unidade inválida ou inativa.");
        }
    }
}
