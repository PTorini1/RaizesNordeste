namespace RaizesNordeste.Application.Common;

public class ResultadoOperacao
{
    protected ResultadoOperacao(
        StatusOperacao status,
        string? codigo = null,
        string? mensagem = null,
        IReadOnlyDictionary<string, string[]>? erros = null)
    {
        Status = status;
        Codigo = codigo;
        Mensagem = mensagem;
        Erros = erros;
    }

    public StatusOperacao Status { get; }
    public string? Codigo { get; }
    public string? Mensagem { get; }
    public IReadOnlyDictionary<string, string[]>? Erros { get; }
    public bool EhSucesso => Status is StatusOperacao.Sucesso or StatusOperacao.Criado or StatusOperacao.SemConteudo;

    public static ResultadoOperacao SemConteudo() => new(StatusOperacao.SemConteudo);

    public static ResultadoOperacao BadRequest(string codigo, string mensagem, IReadOnlyDictionary<string, string[]>? erros = null) =>
        new(StatusOperacao.BadRequest, codigo, mensagem, erros);

    public static ResultadoOperacao NaoEncontrado(string mensagem, string codigo = "RECURSO_NAO_ENCONTRADO") =>
        new(StatusOperacao.NaoEncontrado, codigo, mensagem);

    public static ResultadoOperacao Proibido(string mensagem, string codigo = "ACESSO_NEGADO") =>
        new(StatusOperacao.Proibido, codigo, mensagem);

    public static ResultadoOperacao Conflito(string mensagem, string codigo = "CONFLITO") =>
        new(StatusOperacao.Conflito, codigo, mensagem);

    public static ResultadoOperacao Validacao(string mensagem, IReadOnlyDictionary<string, string[]>? erros = null, string codigo = "ERRO_VALIDACAO") =>
        new(StatusOperacao.Validacao, codigo, mensagem, erros);
}

public sealed class ResultadoOperacao<T> : ResultadoOperacao
{
    private ResultadoOperacao(
        StatusOperacao status,
        T? valor = default,
        string? codigo = null,
        string? mensagem = null,
        IReadOnlyDictionary<string, string[]>? erros = null)
        : base(status, codigo, mensagem, erros)
    {
        Valor = valor;
    }

    public T? Valor { get; }

    public static ResultadoOperacao<T> Sucesso(T valor) => new(StatusOperacao.Sucesso, valor);
    public static ResultadoOperacao<T> Criado(T valor) => new(StatusOperacao.Criado, valor);
    public static new ResultadoOperacao<T> SemConteudo() => new(StatusOperacao.SemConteudo);

    public static new ResultadoOperacao<T> BadRequest(string codigo, string mensagem, IReadOnlyDictionary<string, string[]>? erros = null) =>
        new(StatusOperacao.BadRequest, default, codigo, mensagem, erros);

    public static new ResultadoOperacao<T> NaoEncontrado(string mensagem, string codigo = "RECURSO_NAO_ENCONTRADO") =>
        new(StatusOperacao.NaoEncontrado, default, codigo, mensagem);

    public static new ResultadoOperacao<T> Proibido(string mensagem, string codigo = "ACESSO_NEGADO") =>
        new(StatusOperacao.Proibido, default, codigo, mensagem);

    public static new ResultadoOperacao<T> Conflito(string mensagem, string codigo = "CONFLITO") =>
        new(StatusOperacao.Conflito, default, codigo, mensagem);

    public static new ResultadoOperacao<T> Validacao(string mensagem, IReadOnlyDictionary<string, string[]>? erros = null, string codigo = "ERRO_VALIDACAO") =>
        new(StatusOperacao.Validacao, default, codigo, mensagem, erros);
}
