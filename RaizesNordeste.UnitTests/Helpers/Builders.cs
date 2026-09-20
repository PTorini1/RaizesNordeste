using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;

namespace RaizesNordeste.UnitTests.Helpers;

internal static class Builders
{
    public static Produto Produto(
        int id = 1,
        string nome = "Produto Teste",
        decimal precoBase = 10m,
        string categoria = "Categoria",
        bool ativo = true) =>
        new()
        {
            Id = id,
            Nome = nome,
            Descricao = "Descricao do produto",
            PrecoBase = precoBase,
            Categoria = categoria,
            Sazonal = false,
            Ativo = ativo
        };

    public static Unidade Unidade(int id = 1, bool ativa = true) =>
        new()
        {
            Id = id,
            Nome = "Unidade Teste",
            Cidade = "Fortaleza",
            Estado = "CE",
            Endereco = "Rua Teste, 100",
            TipoOperacao = "Loja",
            Ativa = ativa
        };

    public static Cliente Cliente(int id = 1, bool ativo = true, int usuarioId = 1) =>
        new()
        {
            Id = id,
            UsuarioId = usuarioId,
            Cpf = string.Empty,
            Telefone = string.Empty,
            DataNascimento = DateTime.MinValue,
            Ativo = ativo,
            CriadoEm = DateTime.UtcNow
        };

    public static Usuario Usuario(
        int id = 1,
        bool ativo = true,
        RoleUsuario role = RoleUsuario.Atendente,
        string email = "teste@raizesnordeste.com") =>
        new()
        {
            Id = id,
            Nome = "Usuario Teste",
            Email = email,
            FirebaseUid = $"uid-{id}",
            Role = role,
            Ativo = ativo,
            CriadoEm = DateTime.UtcNow
        };

    public static Estoque Estoque(
        int id = 1,
        int unidadeId = 1,
        int produtoId = 1,
        int quantidadeAtual = 50) =>
        new()
        {
            Id = id,
            UnidadeId = unidadeId,
            ProdutoId = produtoId,
            QuantidadeAtual = quantidadeAtual,
            QuantidadeMinima = 5,
            AtualizadoEm = DateTime.UtcNow
        };

    public static ContaFidelidade ContaFidelidade(
        int id = 1,
        int clienteId = 1,
        int saldo = 100,
        bool ativa = true) =>
        new()
        {
            Id = id,
            ClienteId = clienteId,
            SaldoPontos = saldo,
            Ativa = ativa,
            CriadoEm = DateTime.UtcNow
        };

    public static Pedido Pedido(
        int id = 1,
        int unidadeId = 1,
        CanalPedido canal = CanalPedido.App,
        StatusPedido status = StatusPedido.Criado,
        decimal valorTotal = 100m) =>
        new()
        {
            Id = id,
            UnidadeId = unidadeId,
            CanalPedido = canal,
            Status = status,
            ValorTotal = valorTotal,
            CriadoEm = DateTime.UtcNow
        };

    public static Pedido PedidoComItem(int produtoId = 1, int quantidade = 2, decimal valorUnitario = 50m)
    {
        var pedido = Pedido();
        pedido.AdicionarItem(produtoId, quantidade, valorUnitario);
        return pedido;
    }

    public static Pagamento Pagamento(
        int id = 1,
        int pedidoId = 1,
        string provedor = "PIX",
        string chaveIdempotencia = "chave-123",
        StatusPagamento status = StatusPagamento.Solicitado) =>
        new()
        {
            Id = id,
            PedidoId = pedidoId,
            Provedor = provedor,
            Status = status,
            ChaveIdempotencia = chaveIdempotencia,
            SolicitadoEm = DateTime.UtcNow
        };

    public static Promocao Promocao(
        string tipoDesconto = "PERCENTUAL",
        decimal valorDesconto = 10m,
        int? produtoId = null,
        CanalPedido? canal = null,
        bool ativa = true) =>
        new()
        {
            Id = 1,
            Nome = "Promo Teste",
            TipoDesconto = tipoDesconto,
            ValorDesconto = valorDesconto,
            InicioVigencia = DateTime.UtcNow.AddDays(-1),
            FimVigencia = DateTime.UtcNow.AddDays(30),
            Ativa = ativa,
            ProdutoId = produtoId,
            CanalPedido = canal
        };

    public static CardapioUnidade CardapioItem(
        int unidadeId = 1,
        int produtoId = 1,
        decimal preco = 25m,
        bool disponivel = true) =>
        new()
        {
            UnidadeId = unidadeId,
            ProdutoId = produtoId,
            Preco = preco,
            Disponivel = disponivel
        };
}
