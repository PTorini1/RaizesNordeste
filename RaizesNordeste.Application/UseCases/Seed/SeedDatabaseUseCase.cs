using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Interfaces.Repositories;
using RaizesNordeste.Domain.Interfaces.Services;

namespace RaizesNordeste.Application.UseCases.Seed;

public class SeedDatabaseUseCase(
    IDataStoreResetService dataStoreResetService,
    IUsuarioRepository usuarioRepository,
    IClienteRepository clienteRepository,
    IConsentimentoLGPDRepository consentimentoRepository,
    IContaFidelidadeRepository contaFidelidadeRepository,
    IUnidadeRepository unidadeRepository,
    IProdutoRepository produtoRepository,
    ICardapioUnidadeRepository cardapioRepository,
    IEstoqueRepository estoqueRepository,
    IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
    IPromocaoRepository promocaoRepository,
    IPedidoRepository pedidoRepository,
    IPagamentoRepository pagamentoRepository,
    IMovimentacaoPontosRepository movimentacaoPontosRepository,
    IAuditoriaRepository auditoriaRepository)
{
    public async Task<object> ExecutarAsync(bool reset = false, CancellationToken cancellationToken = default)
    {
        var agora = DateTime.UtcNow;

        if (reset)
        {
            await dataStoreResetService.ResetAsync(cancellationToken);
        }

        if (!reset)
        {
            var usuariosExistentes = await usuarioRepository.ListAsync(take: 1, cancellationToken: cancellationToken);
            if (usuariosExistentes.Count > 0)
            {
                return new { message = "O banco de dados ja possui registros de usuarios. Operacao de seed ignorada para evitar duplicidade." };
            }
        }

        var usuarioAdmin = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "Administrador Raizes",
            Email = "admin@raizesnordeste.com",
            FirebaseUid = "seed_admin_local",
            Role = RoleUsuario.Admin,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var usuarioAdminLoginInicial = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "José",
            Email = "jose@gmail.com",
            FirebaseUid = "83s6INoqz3R4PtOEHKtK1tdNkBx2",
            Role = RoleUsuario.Admin,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var usuarioGerente = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "Gerente Recife",
            Email = "gerente.recife@raizesnordeste.com",
            FirebaseUid = "seed_gerente_recife",
            Role = RoleUsuario.Gerente,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var usuarioAtendente = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "Atendente Balcao",
            Email = "atendente@raizesnordeste.com",
            FirebaseUid = "seed_atendente_balcao",
            Role = RoleUsuario.Atendente,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var usuarioCozinha = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "Equipe Cozinha",
            Email = "cozinha@raizesnordeste.com",
            FirebaseUid = "seed_cozinha",
            Role = RoleUsuario.Cozinha,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var usuarioCliente = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "Maria da Silva",
            Email = "maria@email.com",
            FirebaseUid = "seed_maria_local",
            Role = RoleUsuario.Cliente,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var usuarioClienteWeb = await usuarioRepository.InsertAsync(new Usuario
        {
            Nome = "Joao Pereira",
            Email = "joao@email.com",
            FirebaseUid = "seed_joao_local",
            Role = RoleUsuario.Cliente,
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var cliente = await clienteRepository.InsertAsync(new Cliente
        {
            UsuarioId = usuarioCliente.Id,
            Cpf = "123.456.789-00",
            Telefone = "(81) 98765-4321",
            DataNascimento = new DateTime(1990, 5, 15),
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        var clienteWeb = await clienteRepository.InsertAsync(new Cliente
        {
            UsuarioId = usuarioClienteWeb.Id,
            Cpf = "987.654.321-00",
            Telefone = "(85) 91234-5678",
            DataNascimento = new DateTime(1988, 11, 23),
            Ativo = true,
            CriadoEm = agora
        }, cancellationToken);

        await consentimentoRepository.InsertAsync(new ConsentimentoLGPD
        {
            ClienteId = cliente.Id,
            Finalidade = "FIDELIDADE",
            Consentido = true,
            VersaoTermo = "v1.0",
            ConcedidoEm = agora
        }, cancellationToken);

        await consentimentoRepository.InsertAsync(new ConsentimentoLGPD
        {
            ClienteId = clienteWeb.Id,
            Finalidade = "MARKETING",
            Consentido = false,
            VersaoTermo = "v1.0",
            RevogadoEm = agora.AddDays(-2)
        }, cancellationToken);

        var contaFidelidade = await contaFidelidadeRepository.InsertAsync(new ContaFidelidade
        {
            ClienteId = cliente.Id,
            SaldoPontos = 150,
            Ativa = true,
            CriadoEm = agora
        }, cancellationToken);

        var contaFidelidadeWeb = await contaFidelidadeRepository.InsertAsync(new ContaFidelidade
        {
            ClienteId = clienteWeb.Id,
            SaldoPontos = 30,
            Ativa = true,
            CriadoEm = agora
        }, cancellationToken);

        var unidadeSalvador = await unidadeRepository.InsertAsync(new Unidade
        {
            Nome = "Raizes Salvador Centro",
            Cidade = "Salvador",
            Estado = "BA",
            Endereco = "Pelourinho, 12",
            TipoOperacao = "Restaurante",
            Ativa = true
        }, cancellationToken);

        var unidadeRecife = await unidadeRepository.InsertAsync(new Unidade
        {
            Nome = "Raizes Recife Boa Viagem",
            Cidade = "Recife",
            Estado = "PE",
            Endereco = "Avenida Boa Viagem, 1000",
            TipoOperacao = "Quiosque",
            Ativa = true
        }, cancellationToken);

        var unidadeFortaleza = await unidadeRepository.InsertAsync(new Unidade
        {
            Nome = "Raizes Fortaleza Delivery",
            Cidade = "Fortaleza",
            Estado = "CE",
            Endereco = "Rua das Dunas, 45",
            TipoOperacao = "Delivery",
            Ativa = true
        }, cancellationToken);

        var produtoAcaraje = await produtoRepository.InsertAsync(new Produto
        {
            Nome = "Acaraje Tradicional",
            Descricao = "Acaraje frito no dende com vatapa, caruru e camarao.",
            PrecoBase = 15.00m,
            Categoria = "Comida",
            Sazonal = false,
            Ativo = true
        }, cancellationToken);

        var produtoBaiao = await produtoRepository.InsertAsync(new Produto
        {
            Nome = "Baiao de Dois",
            Descricao = "Baiao de dois cremoso com queijo coalho e carne de sol.",
            PrecoBase = 32.00m,
            Categoria = "Comida",
            Sazonal = false,
            Ativo = true
        }, cancellationToken);

        var produtoCajuina = await produtoRepository.InsertAsync(new Produto
        {
            Nome = "Cajuina Nordestina",
            Descricao = "Bebida clarificada de caju, garrafa de 500ml.",
            PrecoBase = 9.00m,
            Categoria = "Bebida",
            Sazonal = false,
            Ativo = true
        }, cancellationToken);

        var produtoTapioca = await produtoRepository.InsertAsync(new Produto
        {
            Nome = "Tapioca de Carne de Sol",
            Descricao = "Tapioca recheada com carne de sol, queijo coalho e cebola roxa.",
            PrecoBase = 22.00m,
            Categoria = "Comida",
            Sazonal = false,
            Ativo = true
        }, cancellationToken);

        var produtoCuscuz = await produtoRepository.InsertAsync(new Produto
        {
            Nome = "Cuscuz Junino",
            Descricao = "Cuscuz com manteiga de garrafa e queijo coalho.",
            PrecoBase = 18.00m,
            Categoria = "Comida",
            Sazonal = true,
            Ativo = true
        }, cancellationToken);

        var produtoCartola = await produtoRepository.InsertAsync(new Produto
        {
            Nome = "Cartola Pernambucana",
            Descricao = "Banana, queijo manteiga, acucar e canela.",
            PrecoBase = 16.00m,
            Categoria = "Sobremesa",
            Sazonal = false,
            Ativo = true
        }, cancellationToken);

        var cardapios = new List<CardapioUnidade>
        {
            new() { UnidadeId = unidadeSalvador.Id, ProdutoId = produtoAcaraje.Id, Preco = 16.50m, Disponivel = true },
            new() { UnidadeId = unidadeSalvador.Id, ProdutoId = produtoBaiao.Id, Preco = 34.00m, Disponivel = true },
            new() { UnidadeId = unidadeSalvador.Id, ProdutoId = produtoCajuina.Id, Preco = 9.00m, Disponivel = true },
            new() { UnidadeId = unidadeRecife.Id, ProdutoId = produtoTapioca.Id, Preco = 24.00m, Disponivel = true },
            new() { UnidadeId = unidadeRecife.Id, ProdutoId = produtoCuscuz.Id, Preco = 19.50m, Disponivel = true },
            new() { UnidadeId = unidadeRecife.Id, ProdutoId = produtoCartola.Id, Preco = 17.00m, Disponivel = true },
            new() { UnidadeId = unidadeFortaleza.Id, ProdutoId = produtoBaiao.Id, Preco = 33.00m, Disponivel = true },
            new() { UnidadeId = unidadeFortaleza.Id, ProdutoId = produtoCajuina.Id, Preco = 8.50m, Disponivel = true },
            new() { UnidadeId = unidadeFortaleza.Id, ProdutoId = produtoTapioca.Id, Preco = 23.50m, Disponivel = false }
        };

        foreach (var cardapio in cardapios)
        {
            await cardapioRepository.InsertAsync(cardapio, cancellationToken);
        }

        var estoques = new List<Estoque>
        {
            new() { UnidadeId = unidadeSalvador.Id, ProdutoId = produtoAcaraje.Id, QuantidadeAtual = 100, QuantidadeMinima = 10, AtualizadoEm = agora },
            new() { UnidadeId = unidadeSalvador.Id, ProdutoId = produtoBaiao.Id, QuantidadeAtual = 50, QuantidadeMinima = 5, AtualizadoEm = agora },
            new() { UnidadeId = unidadeSalvador.Id, ProdutoId = produtoCajuina.Id, QuantidadeAtual = 150, QuantidadeMinima = 20, AtualizadoEm = agora },
            new() { UnidadeId = unidadeRecife.Id, ProdutoId = produtoTapioca.Id, QuantidadeAtual = 70, QuantidadeMinima = 8, AtualizadoEm = agora },
            new() { UnidadeId = unidadeRecife.Id, ProdutoId = produtoCuscuz.Id, QuantidadeAtual = 30, QuantidadeMinima = 6, AtualizadoEm = agora },
            new() { UnidadeId = unidadeRecife.Id, ProdutoId = produtoCartola.Id, QuantidadeAtual = 40, QuantidadeMinima = 6, AtualizadoEm = agora },
            new() { UnidadeId = unidadeFortaleza.Id, ProdutoId = produtoBaiao.Id, QuantidadeAtual = 45, QuantidadeMinima = 8, AtualizadoEm = agora },
            new() { UnidadeId = unidadeFortaleza.Id, ProdutoId = produtoCajuina.Id, QuantidadeAtual = 120, QuantidadeMinima = 20, AtualizadoEm = agora },
            new() { UnidadeId = unidadeFortaleza.Id, ProdutoId = produtoTapioca.Id, QuantidadeAtual = 0, QuantidadeMinima = 5, AtualizadoEm = agora }
        };

        foreach (var estoque in estoques)
        {
            await estoqueRepository.InsertAsync(estoque, cancellationToken);
        }

        await movimentacaoEstoqueRepository.InsertAsync(new MovimentacaoEstoque
        {
            EstoqueId = estoques[0].Id,
            UsuarioId = usuarioGerente.Id,
            TipoMovimentacao = TipoMovimentacaoEstoque.Entrada,
            Quantidade = 100,
            Motivo = "Carga inicial de estoque",
            CriadoEm = agora
        }, cancellationToken);

        await movimentacaoEstoqueRepository.InsertAsync(new MovimentacaoEstoque
        {
            EstoqueId = estoques[1].Id,
            UsuarioId = usuarioAtendente.Id,
            TipoMovimentacao = TipoMovimentacaoEstoque.Venda,
            Quantidade = 2,
            Motivo = "Venda de teste",
            CriadoEm = agora.AddMinutes(5)
        }, cancellationToken);

        await movimentacaoEstoqueRepository.InsertAsync(new MovimentacaoEstoque
        {
            EstoqueId = estoques[8].Id,
            UsuarioId = usuarioGerente.Id,
            TipoMovimentacao = TipoMovimentacaoEstoque.Ajuste,
            Quantidade = 0,
            Motivo = "Produto indisponivel para testes",
            CriadoEm = agora.AddMinutes(10)
        }, cancellationToken);

        var promoProduto = await promocaoRepository.InsertAsync(new Promocao
        {
            Nome = "Promocao Cajuina Centenaria",
            Descricao = "Desconto de 2.00 reais na Cajuina.",
            TipoDesconto = "VALOR",
            ValorDesconto = 2.00m,
            InicioVigencia = agora.AddDays(-1),
            FimVigencia = agora.AddDays(30),
            Ativa = true,
            ProdutoId = produtoCajuina.Id
        }, cancellationToken);

        var promoCanal = await promocaoRepository.InsertAsync(new Promocao
        {
            Nome = "Campanha Totem 10%",
            Descricao = "Desconto de 10% para pedidos realizados via TOTEM.",
            TipoDesconto = "PERCENTUAL",
            ValorDesconto = 10.00m,
            InicioVigencia = agora.AddDays(-1),
            FimVigencia = agora.AddDays(30),
            Ativa = true,
            CanalPedido = CanalPedido.Totem
        }, cancellationToken);

        var promoPerfil = await promocaoRepository.InsertAsync(new Promocao
        {
            Nome = "Cliente Fiel 5%",
            Descricao = "Desconto para clientes com conta fidelidade ativa.",
            TipoDesconto = "PERCENTUAL",
            ValorDesconto = 5.00m,
            InicioVigencia = agora.AddDays(-1),
            FimVigencia = agora.AddDays(45),
            Ativa = true,
            PerfilCliente = "FIDELIDADE"
        }, cancellationToken);

        var pedidoEntregue = new Pedido
        {
            ClienteId = cliente.Id,
            UnidadeId = unidadeSalvador.Id,
            CanalPedido = CanalPedido.App,
            Status = StatusPedido.Entregue,
            ValorTotal = 50.50m,
            CriadoEm = agora.AddHours(-6),
            AtualizadoEm = agora.AddHours(-5)
        };
        pedidoEntregue.AdicionarItem(produtoAcaraje.Id, 1, 16.50m);
        pedidoEntregue.AdicionarItem(produtoBaiao.Id, 1, 34.00m);
        pedidoEntregue = await pedidoRepository.InsertAsync(pedidoEntregue, cancellationToken);

        var pedidoAguardandoPagamento = new Pedido
        {
            ClienteId = clienteWeb.Id,
            UnidadeId = unidadeRecife.Id,
            CanalPedido = CanalPedido.Totem,
            Status = StatusPedido.AguardandoPagamento,
            ValorTotal = 43.50m,
            CriadoEm = agora.AddMinutes(-45),
            AtualizadoEm = agora.AddMinutes(-40)
        };
        pedidoAguardandoPagamento.AdicionarItem(produtoTapioca.Id, 1, 24.00m);
        pedidoAguardandoPagamento.AdicionarItem(produtoCuscuz.Id, 1, 19.50m);
        pedidoAguardandoPagamento = await pedidoRepository.InsertAsync(pedidoAguardandoPagamento, cancellationToken);

        var pedidoCancelado = new Pedido
        {
            ClienteId = null,
            UnidadeId = unidadeFortaleza.Id,
            CanalPedido = CanalPedido.Balcao,
            Status = StatusPedido.Cancelado,
            ValorTotal = 33.00m,
            CriadoEm = agora.AddHours(-2),
            AtualizadoEm = agora.AddHours(-1),
            MotivoCancelamento = "Cliente desistiu do pedido",
            UsuarioCancelamentoId = usuarioAtendente.Id,
            CanceladoEm = agora.AddHours(-1)
        };
        pedidoCancelado.AdicionarItem(produtoBaiao.Id, 1, 33.00m);
        pedidoCancelado = await pedidoRepository.InsertAsync(pedidoCancelado, cancellationToken);

        var pagamentoAprovado = await pagamentoRepository.InsertAsync(new Pagamento
        {
            PedidoId = pedidoEntregue.Id,
            Provedor = "MockPay",
            Status = StatusPagamento.Aprovado,
            TransacaoExternaId = "seed_tx_aprovada_001",
            PayloadEnvio = "{\"valor\":50.50}",
            PayloadRetorno = "{\"status\":\"aprovado\"}",
            ChaveIdempotencia = $"seed-pagamento-{pedidoEntregue.Id}",
            SolicitadoEm = agora.AddHours(-6),
            RespondidoEm = agora.AddHours(-6).AddMinutes(1)
        }, cancellationToken);

        var pagamentoPendente = await pagamentoRepository.InsertAsync(new Pagamento
        {
            PedidoId = pedidoAguardandoPagamento.Id,
            Provedor = "MockPay",
            Status = StatusPagamento.Pendente,
            TransacaoExternaId = "seed_tx_pendente_001",
            PayloadEnvio = "{\"valor\":43.50}",
            ChaveIdempotencia = $"seed-pagamento-{pedidoAguardandoPagamento.Id}",
            SolicitadoEm = agora.AddMinutes(-40)
        }, cancellationToken);

        var pagamentoCancelado = await pagamentoRepository.InsertAsync(new Pagamento
        {
            PedidoId = pedidoCancelado.Id,
            Provedor = "MockPay",
            Status = StatusPagamento.Cancelado,
            TransacaoExternaId = "seed_tx_cancelada_001",
            PayloadEnvio = "{\"valor\":33.00}",
            PayloadRetorno = "{\"status\":\"cancelado\"}",
            ChaveIdempotencia = $"seed-pagamento-{pedidoCancelado.Id}",
            SolicitadoEm = agora.AddHours(-2),
            RespondidoEm = agora.AddHours(-1)
        }, cancellationToken);

        await movimentacaoPontosRepository.InsertAsync(new MovimentacaoPontos
        {
            ContaFidelidadeId = contaFidelidade.Id,
            PedidoId = pedidoEntregue.Id,
            TipoMovimentacao = TipoMovimentacaoPontos.Acumulo,
            Pontos = 50,
            Descricao = "Acumulo por pedido entregue",
            CriadoEm = agora.AddHours(-5)
        }, cancellationToken);

        await movimentacaoPontosRepository.InsertAsync(new MovimentacaoPontos
        {
            ContaFidelidadeId = contaFidelidade.Id,
            PedidoId = pedidoEntregue.Id,
            TipoMovimentacao = TipoMovimentacaoPontos.Resgate,
            Pontos = 20,
            Descricao = "Resgate aplicado em pedido de teste",
            CriadoEm = agora.AddHours(-5).AddMinutes(10)
        }, cancellationToken);

        await movimentacaoPontosRepository.InsertAsync(new MovimentacaoPontos
        {
            ContaFidelidadeId = contaFidelidadeWeb.Id,
            PedidoId = null,
            TipoMovimentacao = TipoMovimentacaoPontos.Estorno,
            Pontos = 10,
            Descricao = "Estorno administrativo de teste",
            CriadoEm = agora.AddDays(-1)
        }, cancellationToken);

        await auditoriaRepository.InsertAsync(new Auditoria
        {
            UsuarioId = usuarioAdmin.Id,
            Entidade = nameof(Produto),
            EntidadeId = produtoAcaraje.Id,
            Acao = "CREATE",
            DadosNovos = "{\"origem\":\"seed\"}",
            CriadoEm = agora
        }, cancellationToken);

        await auditoriaRepository.InsertAsync(new Auditoria
        {
            UsuarioId = usuarioGerente.Id,
            Entidade = nameof(Estoque),
            EntidadeId = estoques[8].Id,
            Acao = "UPDATE",
            DadosAnteriores = "{\"quantidadeAtual\":10}",
            DadosNovos = "{\"quantidadeAtual\":0}",
            CriadoEm = agora.AddMinutes(10)
        }, cancellationToken);

        await auditoriaRepository.InsertAsync(new Auditoria
        {
            UsuarioId = usuarioAtendente.Id,
            Entidade = nameof(Pedido),
            EntidadeId = pedidoCancelado.Id,
            Acao = "CANCEL",
            DadosNovos = "{\"motivo\":\"Cliente desistiu do pedido\"}",
            CriadoEm = agora.AddHours(-1)
        }, cancellationToken);

        return new
        {
            message = "Banco de dados populado com sucesso para testes.",
            dadosCriados = new
            {
                Usuarios = new[]
                {
                    new { usuarioAdmin.Id, usuarioAdmin.Email, usuarioAdmin.Role },
                    new { usuarioAdminLoginInicial.Id, usuarioAdminLoginInicial.Email, usuarioAdminLoginInicial.Role },
                    new { usuarioGerente.Id, usuarioGerente.Email, usuarioGerente.Role },
                    new { usuarioAtendente.Id, usuarioAtendente.Email, usuarioAtendente.Role },
                    new { usuarioCozinha.Id, usuarioCozinha.Email, usuarioCozinha.Role },
                    new { usuarioCliente.Id, usuarioCliente.Email, usuarioCliente.Role },
                    new { usuarioClienteWeb.Id, usuarioClienteWeb.Email, usuarioClienteWeb.Role }
                },
                Clientes = new[] { cliente.Id, clienteWeb.Id },
                Unidades = new[]
                {
                    new { unidadeSalvador.Id, unidadeSalvador.Nome },
                    new { unidadeRecife.Id, unidadeRecife.Nome },
                    new { unidadeFortaleza.Id, unidadeFortaleza.Nome }
                },
                Produtos = new[]
                {
                    new { produtoAcaraje.Id, produtoAcaraje.Nome },
                    new { produtoBaiao.Id, produtoBaiao.Nome },
                    new { produtoCajuina.Id, produtoCajuina.Nome },
                    new { produtoTapioca.Id, produtoTapioca.Nome },
                    new { produtoCuscuz.Id, produtoCuscuz.Nome },
                    new { produtoCartola.Id, produtoCartola.Nome }
                },
                Promocoes = new[]
                {
                    new { promoProduto.Id, promoProduto.Nome },
                    new { promoCanal.Id, promoCanal.Nome },
                    new { promoPerfil.Id, promoPerfil.Nome }
                },
                Pedidos = new[]
                {
                    new { pedidoEntregue.Id, pedidoEntregue.Status, pedidoEntregue.ValorTotal },
                    new { pedidoAguardandoPagamento.Id, pedidoAguardandoPagamento.Status, pedidoAguardandoPagamento.ValorTotal },
                    new { pedidoCancelado.Id, pedidoCancelado.Status, pedidoCancelado.ValorTotal }
                },
                Pagamentos = new[]
                {
                    new { pagamentoAprovado.Id, pagamentoAprovado.Status },
                    new { pagamentoPendente.Id, pagamentoPendente.Status },
                    new { pagamentoCancelado.Id, pagamentoCancelado.Status }
                },
                Totais = new
                {
                    Usuarios = 7,
                    Clientes = 2,
                    ConsentimentosLgpd = 2,
                    ContasFidelidade = 2,
                    Unidades = 3,
                    Produtos = 6,
                    Cardapios = cardapios.Count,
                    Estoques = estoques.Count,
                    MovimentacoesEstoque = 3,
                    Promocoes = 3,
                    Pedidos = 3,
                    ItensPedido = 5,
                    Pagamentos = 3,
                    MovimentacoesPontos = 3,
                    Auditorias = 3
                }
            }
        };
    }
}
