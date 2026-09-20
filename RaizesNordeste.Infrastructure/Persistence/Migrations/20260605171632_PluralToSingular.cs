using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaizesNordeste.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PluralToSingular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditorias_Usuarios_UsuarioId",
                table: "Auditorias");

            migrationBuilder.DropForeignKey(
                name: "FK_CardapiosUnidade_Produtos_ProdutoId",
                table: "CardapiosUnidade");

            migrationBuilder.DropForeignKey(
                name: "FK_CardapiosUnidade_Unidades_UnidadeId",
                table: "CardapiosUnidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Usuarios_UsuarioId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsentimentosLgpd_Clientes_ClienteId",
                table: "ConsentimentosLgpd");

            migrationBuilder.DropForeignKey(
                name: "FK_ContasFidelidade_Clientes_ClienteId",
                table: "ContasFidelidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Unidades_UnidadeId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedido_Pedidos_PedidoId",
                table: "ItensPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedido_Produtos_ProdutoId",
                table: "ItensPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_Estoques_EstoqueId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_Usuarios_UsuarioId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesPontos_ContasFidelidade_ContaFidelidadeId",
                table: "MovimentacoesPontos");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesPontos_Pedidos_PedidoId",
                table: "MovimentacoesPontos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Pedidos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Unidades_UnidadeId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioCancelamentoId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Promocoes_Produtos_ProdutoId",
                table: "Promocoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unidades",
                table: "Unidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promocoes",
                table: "Promocoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pagamentos",
                table: "Pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimentacoesPontos",
                table: "MovimentacoesPontos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimentacoesEstoque",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedido",
                table: "ItensPedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estoques",
                table: "Estoques");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContasFidelidade",
                table: "ContasFidelidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsentimentosLgpd",
                table: "ConsentimentosLgpd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardapiosUnidade",
                table: "CardapiosUnidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auditorias",
                table: "Auditorias");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Usuario");

            migrationBuilder.RenameTable(
                name: "Unidades",
                newName: "Unidade");

            migrationBuilder.RenameTable(
                name: "Promocoes",
                newName: "Promocao");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "Produto");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "Pedido");

            migrationBuilder.RenameTable(
                name: "Pagamentos",
                newName: "Pagamento");

            migrationBuilder.RenameTable(
                name: "MovimentacoesPontos",
                newName: "MovimentacaoPontos");

            migrationBuilder.RenameTable(
                name: "MovimentacoesEstoque",
                newName: "MovimentacaoEstoque");

            migrationBuilder.RenameTable(
                name: "ItensPedido",
                newName: "ItemPedido");

            migrationBuilder.RenameTable(
                name: "Estoques",
                newName: "Estoque");

            migrationBuilder.RenameTable(
                name: "ContasFidelidade",
                newName: "ContaFidelidade");

            migrationBuilder.RenameTable(
                name: "ConsentimentosLgpd",
                newName: "ConsentimentoLgpd");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "Cliente");

            migrationBuilder.RenameTable(
                name: "CardapiosUnidade",
                newName: "CardapioUnidade");

            migrationBuilder.RenameTable(
                name: "Auditorias",
                newName: "Auditoria");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_Email",
                table: "Usuario",
                newName: "IX_Usuario_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Promocoes_ProdutoId",
                table: "Promocao",
                newName: "IX_Promocao_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_UsuarioCancelamentoId",
                table: "Pedido",
                newName: "IX_Pedido_UsuarioCancelamentoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_UnidadeId",
                table: "Pedido",
                newName: "IX_Pedido_UnidadeId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedido",
                newName: "IX_Pedido_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamento",
                newName: "IX_Pagamento_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamentos_ChaveIdempotencia",
                table: "Pagamento",
                newName: "IX_Pagamento_ChaveIdempotencia");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacoesPontos_PedidoId",
                table: "MovimentacaoPontos",
                newName: "IX_MovimentacaoPontos_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacoesPontos_ContaFidelidadeId",
                table: "MovimentacaoPontos",
                newName: "IX_MovimentacaoPontos_ContaFidelidadeId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacoesEstoque_UsuarioId",
                table: "MovimentacaoEstoque",
                newName: "IX_MovimentacaoEstoque_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacoesEstoque_EstoqueId",
                table: "MovimentacaoEstoque",
                newName: "IX_MovimentacaoEstoque_EstoqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensPedido_ProdutoId",
                table: "ItemPedido",
                newName: "IX_ItemPedido_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "ItemPedido",
                newName: "IX_ItemPedido_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Estoques_UnidadeId_ProdutoId",
                table: "Estoque",
                newName: "IX_Estoque_UnidadeId_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_Estoques_ProdutoId",
                table: "Estoque",
                newName: "IX_Estoque_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_ContasFidelidade_ClienteId",
                table: "ContaFidelidade",
                newName: "IX_ContaFidelidade_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsentimentosLgpd_ClienteId",
                table: "ConsentimentoLgpd",
                newName: "IX_ConsentimentoLgpd_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_UsuarioId",
                table: "Cliente",
                newName: "IX_Cliente_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_Cpf",
                table: "Cliente",
                newName: "IX_Cliente_Cpf");

            migrationBuilder.RenameIndex(
                name: "IX_CardapiosUnidade_UnidadeId_ProdutoId",
                table: "CardapioUnidade",
                newName: "IX_CardapioUnidade_UnidadeId_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_CardapiosUnidade_ProdutoId",
                table: "CardapioUnidade",
                newName: "IX_CardapioUnidade_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_Auditorias_UsuarioId",
                table: "Auditoria",
                newName: "IX_Auditoria_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unidade",
                table: "Unidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promocao",
                table: "Promocao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produto",
                table: "Produto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pagamento",
                table: "Pagamento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimentacaoPontos",
                table: "MovimentacaoPontos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimentacaoEstoque",
                table: "MovimentacaoEstoque",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPedido",
                table: "ItemPedido",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estoque",
                table: "Estoque",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContaFidelidade",
                table: "ContaFidelidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsentimentoLgpd",
                table: "ConsentimentoLgpd",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardapioUnidade",
                table: "CardapioUnidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auditoria",
                table: "Auditoria",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditoria_Usuario_UsuarioId",
                table: "Auditoria",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardapioUnidade_Produto_ProdutoId",
                table: "CardapioUnidade",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardapioUnidade_Unidade_UnidadeId",
                table: "CardapioUnidade",
                column: "UnidadeId",
                principalTable: "Unidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Usuario_UsuarioId",
                table: "Cliente",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsentimentoLgpd_Cliente_ClienteId",
                table: "ConsentimentoLgpd",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContaFidelidade_Cliente_ClienteId",
                table: "ContaFidelidade",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoque_Produto_ProdutoId",
                table: "Estoque",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoque_Unidade_UnidadeId",
                table: "Estoque",
                column: "UnidadeId",
                principalTable: "Unidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Pedido_PedidoId",
                table: "ItemPedido",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Produto_ProdutoId",
                table: "ItemPedido",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoEstoque_Estoque_EstoqueId",
                table: "MovimentacaoEstoque",
                column: "EstoqueId",
                principalTable: "Estoque",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoEstoque_Usuario_UsuarioId",
                table: "MovimentacaoEstoque",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoPontos_ContaFidelidade_ContaFidelidadeId",
                table: "MovimentacaoPontos",
                column: "ContaFidelidadeId",
                principalTable: "ContaFidelidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoPontos_Pedido_PedidoId",
                table: "MovimentacaoPontos",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamento_Pedido_PedidoId",
                table: "Pagamento",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Cliente_ClienteId",
                table: "Pedido",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Unidade_UnidadeId",
                table: "Pedido",
                column: "UnidadeId",
                principalTable: "Unidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Usuario_UsuarioCancelamentoId",
                table: "Pedido",
                column: "UsuarioCancelamentoId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promocao_Produto_ProdutoId",
                table: "Promocao",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditoria_Usuario_UsuarioId",
                table: "Auditoria");

            migrationBuilder.DropForeignKey(
                name: "FK_CardapioUnidade_Produto_ProdutoId",
                table: "CardapioUnidade");

            migrationBuilder.DropForeignKey(
                name: "FK_CardapioUnidade_Unidade_UnidadeId",
                table: "CardapioUnidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Usuario_UsuarioId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsentimentoLgpd_Cliente_ClienteId",
                table: "ConsentimentoLgpd");

            migrationBuilder.DropForeignKey(
                name: "FK_ContaFidelidade_Cliente_ClienteId",
                table: "ContaFidelidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoque_Produto_ProdutoId",
                table: "Estoque");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoque_Unidade_UnidadeId",
                table: "Estoque");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Pedido_PedidoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Produto_ProdutoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoEstoque_Estoque_EstoqueId",
                table: "MovimentacaoEstoque");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoEstoque_Usuario_UsuarioId",
                table: "MovimentacaoEstoque");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoPontos_ContaFidelidade_ContaFidelidadeId",
                table: "MovimentacaoPontos");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoPontos_Pedido_PedidoId",
                table: "MovimentacaoPontos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamento_Pedido_PedidoId",
                table: "Pagamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Cliente_ClienteId",
                table: "Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Unidade_UnidadeId",
                table: "Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Usuario_UsuarioCancelamentoId",
                table: "Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Promocao_Produto_ProdutoId",
                table: "Promocao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unidade",
                table: "Unidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promocao",
                table: "Promocao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produto",
                table: "Produto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pagamento",
                table: "Pagamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimentacaoPontos",
                table: "MovimentacaoPontos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimentacaoEstoque",
                table: "MovimentacaoEstoque");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPedido",
                table: "ItemPedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estoque",
                table: "Estoque");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContaFidelidade",
                table: "ContaFidelidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsentimentoLgpd",
                table: "ConsentimentoLgpd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardapioUnidade",
                table: "CardapioUnidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auditoria",
                table: "Auditoria");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Unidade",
                newName: "Unidades");

            migrationBuilder.RenameTable(
                name: "Promocao",
                newName: "Promocoes");

            migrationBuilder.RenameTable(
                name: "Produto",
                newName: "Produtos");

            migrationBuilder.RenameTable(
                name: "Pedido",
                newName: "Pedidos");

            migrationBuilder.RenameTable(
                name: "Pagamento",
                newName: "Pagamentos");

            migrationBuilder.RenameTable(
                name: "MovimentacaoPontos",
                newName: "MovimentacoesPontos");

            migrationBuilder.RenameTable(
                name: "MovimentacaoEstoque",
                newName: "MovimentacoesEstoque");

            migrationBuilder.RenameTable(
                name: "ItemPedido",
                newName: "ItensPedido");

            migrationBuilder.RenameTable(
                name: "Estoque",
                newName: "Estoques");

            migrationBuilder.RenameTable(
                name: "ContaFidelidade",
                newName: "ContasFidelidade");

            migrationBuilder.RenameTable(
                name: "ConsentimentoLgpd",
                newName: "ConsentimentosLgpd");

            migrationBuilder.RenameTable(
                name: "Cliente",
                newName: "Clientes");

            migrationBuilder.RenameTable(
                name: "CardapioUnidade",
                newName: "CardapiosUnidade");

            migrationBuilder.RenameTable(
                name: "Auditoria",
                newName: "Auditorias");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_Email",
                table: "Usuarios",
                newName: "IX_Usuarios_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Promocao_ProdutoId",
                table: "Promocoes",
                newName: "IX_Promocoes_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_UsuarioCancelamentoId",
                table: "Pedidos",
                newName: "IX_Pedidos_UsuarioCancelamentoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_UnidadeId",
                table: "Pedidos",
                newName: "IX_Pedidos_UnidadeId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_ClienteId",
                table: "Pedidos",
                newName: "IX_Pedidos_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamento_PedidoId",
                table: "Pagamentos",
                newName: "IX_Pagamentos_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamento_ChaveIdempotencia",
                table: "Pagamentos",
                newName: "IX_Pagamentos_ChaveIdempotencia");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoPontos_PedidoId",
                table: "MovimentacoesPontos",
                newName: "IX_MovimentacoesPontos_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoPontos_ContaFidelidadeId",
                table: "MovimentacoesPontos",
                newName: "IX_MovimentacoesPontos_ContaFidelidadeId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoEstoque_UsuarioId",
                table: "MovimentacoesEstoque",
                newName: "IX_MovimentacoesEstoque_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoEstoque_EstoqueId",
                table: "MovimentacoesEstoque",
                newName: "IX_MovimentacoesEstoque_EstoqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPedido_ProdutoId",
                table: "ItensPedido",
                newName: "IX_ItensPedido_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPedido_PedidoId",
                table: "ItensPedido",
                newName: "IX_ItensPedido_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Estoque_UnidadeId_ProdutoId",
                table: "Estoques",
                newName: "IX_Estoques_UnidadeId_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_Estoque_ProdutoId",
                table: "Estoques",
                newName: "IX_Estoques_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_ContaFidelidade_ClienteId",
                table: "ContasFidelidade",
                newName: "IX_ContasFidelidade_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsentimentoLgpd_ClienteId",
                table: "ConsentimentosLgpd",
                newName: "IX_ConsentimentosLgpd_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_UsuarioId",
                table: "Clientes",
                newName: "IX_Clientes_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_Cpf",
                table: "Clientes",
                newName: "IX_Clientes_Cpf");

            migrationBuilder.RenameIndex(
                name: "IX_CardapioUnidade_UnidadeId_ProdutoId",
                table: "CardapiosUnidade",
                newName: "IX_CardapiosUnidade_UnidadeId_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_CardapioUnidade_ProdutoId",
                table: "CardapiosUnidade",
                newName: "IX_CardapiosUnidade_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_Auditoria_UsuarioId",
                table: "Auditorias",
                newName: "IX_Auditorias_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unidades",
                table: "Unidades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promocoes",
                table: "Promocoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pagamentos",
                table: "Pagamentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimentacoesPontos",
                table: "MovimentacoesPontos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimentacoesEstoque",
                table: "MovimentacoesEstoque",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedido",
                table: "ItensPedido",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estoques",
                table: "Estoques",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContasFidelidade",
                table: "ContasFidelidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsentimentosLgpd",
                table: "ConsentimentosLgpd",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardapiosUnidade",
                table: "CardapiosUnidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auditorias",
                table: "Auditorias",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditorias_Usuarios_UsuarioId",
                table: "Auditorias",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardapiosUnidade_Produtos_ProdutoId",
                table: "CardapiosUnidade",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardapiosUnidade_Unidades_UnidadeId",
                table: "CardapiosUnidade",
                column: "UnidadeId",
                principalTable: "Unidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Usuarios_UsuarioId",
                table: "Clientes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsentimentosLgpd_Clientes_ClienteId",
                table: "ConsentimentosLgpd",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContasFidelidade_Clientes_ClienteId",
                table: "ContasFidelidade",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Unidades_UnidadeId",
                table: "Estoques",
                column: "UnidadeId",
                principalTable: "Unidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedido_Pedidos_PedidoId",
                table: "ItensPedido",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedido_Produtos_ProdutoId",
                table: "ItensPedido",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_Estoques_EstoqueId",
                table: "MovimentacoesEstoque",
                column: "EstoqueId",
                principalTable: "Estoques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_Usuarios_UsuarioId",
                table: "MovimentacoesEstoque",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesPontos_ContasFidelidade_ContaFidelidadeId",
                table: "MovimentacoesPontos",
                column: "ContaFidelidadeId",
                principalTable: "ContasFidelidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesPontos_Pedidos_PedidoId",
                table: "MovimentacoesPontos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Pedidos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteId",
                table: "Pedidos",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Unidades_UnidadeId",
                table: "Pedidos",
                column: "UnidadeId",
                principalTable: "Unidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioCancelamentoId",
                table: "Pedidos",
                column: "UsuarioCancelamentoId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promocoes_Produtos_ProdutoId",
                table: "Promocoes",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id");
        }
    }
}
