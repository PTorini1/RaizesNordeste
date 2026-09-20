namespace RaizesNordeste.Domain.Entities;

public class Unidade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string TipoOperacao { get; set; } = string.Empty;
    public bool Ativa { get; set; } = true;

    public void Atualizar(string nome, string cidade, string estado, string endereco, string tipoOperacao, bool ativa)
    {
        Nome = nome;
        Cidade = cidade;
        Estado = estado;
        Endereco = endereco;
        TipoOperacao = tipoOperacao;
        Ativa = ativa;
    }

    public void Desativar()
    {
        Ativa = false;
    }
}
