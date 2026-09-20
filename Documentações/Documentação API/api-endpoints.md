# Documentacao de Pontos de Acesso

Este documento registra o contrato HTTP atual da API Raizes Nordeste. A autenticação e autorização por JWT (Firebase) com RBAC (controle de acesso baseado em papéis) e validação de titularidade (proprietário do recurso) para clientes estão totalmente configuradas e ativas no projeto.

## Padrao de erro

Todas as falhas devem responder com o mesmo JSON:

```json
{
  "status": 422,
  "codigo": "ERRO_VALIDACAO",
  "mensagem": "A requisicao contem campos invalidos.",
  "rastreamentoId": "0HN...",
  "caminho": "/api/pedido",
  "dataHora": "2026-06-04T10:00:00.0000000+00:00",
  "erros": {
    "itens": ["O pedido deve possuir ao menos um item."]
  }
}
```

Codigos esperados nas falhas: `400` requisicao malformada, `401` nao autenticado, `403` sem permissao / titularidade invalida, `404` nao encontrado, `409` regra de negocio/conflito, `422` validacao, `500` erro inesperado.

## Paginacao

Listagens aceitam `pagina` e `limite` por query string:

```http
GET /api/pedido?pagina=1&limite=10
```

Resposta paginada:

```json
{
  "pagina": 1,
  "limite": 10,
  "totalItens": 1,
  "totalPaginas": 1,
  "itens": []
}
```

## Pontos de Acesso

| Ponto de acesso | Metodo e rota | Autenticacao/permissao | Parametros | Corpo da requisicao | Resposta de sucesso | Codigos |
| --- | --- | --- | --- | --- | --- | --- |
| Criar unidade: cadastra uma unidade da rede. | `POST /api/unidade` | `JWT ADMIN / GERENTE`. | Nenhum. | `{"nome":"Unidade Centro","cidade":"Recife","estado":"PE","endereco":"Rua A, 100","tipoOperacao":"LOJA"}` | `201` com `{"id":1,"nome":"Unidade Centro","cidade":"Recife","estado":"PE","endereco":"Rua A, 100","tipoOperacao":"LOJA","ativa":true}` | `201,400,401,403,409,422` |
| Listar unidades: consulta unidades com filtro opcional. | `GET /api/unidade` | `JWT ADMIN / GERENTE`. | Consulta: `apenasAtivas`, `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<UnidadeResponse>`. | `200,400,401,403` |
| Obter unidade por ID: consulta uma unidade especifica. | `GET /api/unidade/{id}` | `JWT ADMIN / GERENTE`. | Caminho: `id`. | Nao se aplica. | `200` com `UnidadeResponse`. | `200,401,403,404` |
| Atualizar unidade: altera dados cadastrais da unidade. | `PUT /api/unidade/{id}` | `JWT ADMIN / GERENTE`. | Caminho: `id`. | `{"nome":"Unidade Centro","cidade":"Recife","estado":"PE","endereco":"Rua B, 200","tipoOperacao":"LOJA","ativa":true}` | `200` com `UnidadeResponse`. | `200,400,401,403,404,409,422` |
| Desativar unidade: desativa uma unidade da rede. | `POST /api/unidade/{id}/desativar` | `JWT ADMIN / GERENTE`. | Caminho: `id`. | Nao se aplica. | `204` sem corpo. | `204,401,403,404,409` |
| Consultar cardapio da unidade: lista produtos disponiveis em uma unidade. | `GET /api/unidade/{unidadeId}/cardapio` | `JWT ADMIN / GERENTE / CLIENTE`. | Caminho: `unidadeId`; Consulta: `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<CardapioResponse>`. | `200,400,401,403,404` |
| Associar produto ao cardapio: vincula produto, preco e disponibilidade a unidade. | `POST /api/unidade/{unidadeId}/cardapio` | `JWT ADMIN / GERENTE`. | Caminho: `unidadeId`. | `{"produtoId":1,"preco":29.90,"disponivel":true,"inicioVigencia":"2026-06-04T00:00:00","fimVigencia":null}` | `201` com `CardapioResponse`. | `201,400,401,403,404,409,422` |
| Atualizar produto do cardapio: altera preco e disponibilidade local. | `PUT /api/unidade/{unidadeId}/cardapio/{produtoId}` | `JWT ADMIN / GERENTE`. | Caminho: `unidadeId`, `produtoId`. | `{"preco":31.90,"disponivel":true,"inicioVigencia":"2026-06-04T00:00:00","fimVigencia":null}` | `200` com `CardapioResponse`. | `200,400,401,403,404,409,422` |
| Remover produto do cardapio: remove associacao produto/unidade. | `DELETE /api/unidade/{unidadeId}/cardapio/{produtoId}` | `JWT ADMIN / GERENTE`. | Caminho: `unidadeId`, `produtoId`. | Nao se aplica. | `204` sem corpo. | `204,401,403,404,409` |
| Criar pedido: registra pedido com unidade, cliente, itens, canal e status inicial. | `POST /api/pedido` | `JWT CLIENTE (titular) / ATENDENTE`. | Nenhum. | `{"clienteId":1,"unidadeId":1,"canalPedido":"TOTEM","itens":[{"produtoId":1,"quantidade":2}]}` | `201` com `PedidoResponse`. | `201,400,401,403,404,409,422` |
| Obter pedido por ID: consulta pedido especifico. | `GET /api/pedido/{id}` | `JWT ADMIN / GERENTE / CLIENTE (titular)`. | Caminho: `id`. | Nao se aplica. | `200` com `PedidoResponse`. | `200,401,403,404` |
| Listar pedidos: consulta pedidos e filtra por canal. | `GET /api/pedido` | `JWT ADMIN / GERENTE`. | Consulta: `canalPedido`, `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<PedidoResponse>`. | `200,400,401,403` |
| Atualizar status do pedido: move pedido no fluxo operacional. | `PUT /api/pedido/{id}/status` | `JWT ADMIN / GERENTE / ATENDENTE`. | Caminho: `id`. | `"EmPreparo"` | `200` com `PedidoResponse`. | `200,400,401,403,404,409,422` |
| Cancelar pedido: cancela pedido e registra motivo, usuario e data/hora. | `POST /api/pedido/{id}/cancelar` | `JWT ADMIN / GERENTE / ATENDENTE`. | Caminho: `id`. | `{"usuarioId":1,"motivo":"Cliente solicitou cancelamento"}` | `200` com `PedidoResponse`. | `200,400,401,403,404,409,422` |
| Consultar estoque da unidade: lista estoque por unidade. | `GET /api/estoque/unidade/{unidadeId}` | `JWT ADMIN / GERENTE`. | Caminho: `unidadeId`; Consulta: `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<EstoqueResponse>`. | `200,400,401,403,404` |
| Consultar estoque de produto: consulta estoque por unidade e produto. | `GET /api/estoque/unidade/{unidadeId}/produto/{produtoId}` | `JWT ADMIN / GERENTE`. | Caminho: `unidadeId`, `produtoId`. | Nao se aplica. | `200` com `EstoqueResponse`. | `200,401,403,404` |
| Registrar entrada de estoque: registra entrada por produto e unidade. | `POST /api/estoque/entrada` | `JWT ADMIN / GERENTE`. | Nenhum. | `{"unidadeId":1,"produtoId":1,"quantidade":10,"usuarioId":1,"motivo":"Compra"}` | `201` com `EstoqueResponse`. | `201,400,401,403,404,409,422` |
| Registrar saida de estoque: registra saida por venda, ajuste ou perda. | `POST /api/estoque/saida` | `JWT ADMIN / GERENTE / ATENDENTE`. | Nenhum. | `{"unidadeId":1,"produtoId":1,"quantidade":2,"usuarioId":1,"tipoMovimentacao":"Venda","motivo":"Pedido 10"}` | `201` com `EstoqueResponse`. | `201,400,401,403,404,409,422` |
| Aderir fidelidade: registra consentimento e conta de fidelidade. | `POST /api/fidelidade/adesao` | `JWT CLIENTE (titular)`. | Nenhum. | `{"clienteId":1,"finalidade":"FIDELIDADE","consentido":true,"versaoTermo":"v1.0"}` | `201` com `ContaFidelidadeResponse`. | `201,400,401,403,409,422` |
| Resgatar pontos: executa resgate simples de pontos. | `POST /api/fidelidade/resgate` | `JWT CLIENTE (titular)`. | Nenhum. | `{"clienteId":1,"pontos":100,"descricao":"Resgate de desconto"}` | `200` com `ContaFidelidadeResponse`. | `200,400,401,403,404,409,422` |
| Consultar saldo fidelidade: consulta saldo de pontos do cliente. | `GET /api/fidelidade/cliente/{clienteId}/saldo` | `JWT CLIENTE (titular) / ADMIN`. | Caminho: `clienteId`. | Nao se aplica. | `200` com `ContaFidelidadeResponse`. | `200,401,403,404` |
| Criar promocao: representa regra de promocao ou campanha. | `POST /api/promocao` | `JWT ADMIN / GERENTE`. | Nenhum. | `{"nome":"Desconto Totem","descricao":"10% no totem","tipoDesconto":"PERCENTUAL","valorDesconto":10,"inicioVigencia":"2026-06-04T00:00:00","fimVigencia":"2026-06-30T23:59:59","produtoId":1,"canalPedido":"TOTEM","perfilCliente":null}` | `201` com `PromocaoResponse`. | `201,400,401,403,409,422` |
| Listar promocoes: consulta campanhas cadastradas. | `GET /api/promocao` | `JWT ADMIN / GERENTE`. | Consulta: `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<PromocaoResponse>`. | `200,400,401,403` |
| Listar promocoes ativas: filtra campanhas aplicaveis. | `GET /api/promocao/ativa` | `JWT ADMIN / GERENTE / CLIENTE`. | Consulta: `produtoId`, `canalPedido`, `perfilCliente`, `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<PromocaoResponse>`. | `200,400,401,403` |
| Solicitar pagamento mock: envia pedido para servico de pagamento simulado. | `POST /api/pagamento` | `JWT CLIENTE (titular) / ATENDENTE`. | Nenhum. | `{"pedidoId":1,"provedor":"MockPay","chaveIdempotencia":"pedido-1"}` | `201` com `PagamentoResponse`. | `201,400,401,403,404,409,422` |
| Obter pagamento por ID: consulta status de pagamento especifico. | `GET /api/pagamento/{id}` | `JWT ADMIN / GERENTE / CLIENTE (titular)`. | Caminho: `id`. | Nao se aplica. | `200` com `PagamentoResponse`. | `200,401,403,404` |
| Obter pagamento por pedido: consulta pagamento associado ao pedido. | `GET /api/pagamento/pedido/{pedidoId}` | `JWT ADMIN / GERENTE / CLIENTE (titular)`. | Caminho: `pedidoId`. | Nao se aplica. | `200` com `PagamentoResponse`. | `200,401,403,404` |
| Listar auditorias: consulta logs sensiveis. | `GET /api/auditoria` | `JWT ADMIN`. | Consulta: `entidade`, `acao`, `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<Auditoria>`. | `200,400,401,403` |
| Listar auditorias por entidade: consulta logs de uma entidade. | `GET /api/auditoria/entidade/{entidade}` | `JWT ADMIN`. | Caminho: `entidade`; Consulta: `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<Auditoria>`. | `200,400,401,403` |
| Criar usuario: cria usuario operacional. | `POST /api/usuario` | `JWT ADMIN`. | Nenhum. | `{"nome":"Maria","email":"maria@raizes.local","role":"Gerente"}` | `201` com `Usuario`. | `201,400,401,403,409,422` |
| Listar usuarios: consulta usuarios cadastrados. | `GET /api/usuario` | `JWT ADMIN`. | Consulta: `pagina`, `limite`. | Nao se aplica. | `200` com `RespostaPaginada<Usuario>`. | `200,400,401,403` |
| Alterar role de usuario: altera permissao operacional. | `PUT /api/usuario/{id}/role` | `JWT ADMIN`. | Caminho: `id`. | `"Gerente"` | `200` com `Usuario`. | `200,400,401,403,404,422` |
| Executar carga inicial: popula base de dados de desenvolvimento. | `POST /api/carga-inicial` | Restrito a ambiente `Development` ou `JWT ADMIN`. | Consulta: `reset`. | Nao se aplica. | `200` com resumo da carga. | `200,400,401,403,500` |
