# RaizesNordeste

API REST em .NET para gestão de unidades, cardápios, produtos, pedidos, estoque, fidelidade, promoções, pagamentos, usuários e auditoria.

---

## Sumário

- [Visão Geral](#visão-geral)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Execução com Docker (Recomendado)](#execução-com-docker-recomendado)
- [Execução sem Docker](#execução-sem-docker)
- [Banco de Dados e Migrations](#banco-de-dados-e-migrations)
- [Carga Inicial para Testes](#carga-inicial-para-testes)
- [Login Inicial](#login-inicial)
- [Endpoints Disponíveis](#endpoints-disponíveis)
- [Executando os Testes](#executando-os-testes)
- [Comandos Úteis](#comandos-úteis)

---

## Visão Geral

O **RaizesNordeste** é uma API desenvolvida em .NET com arquitetura em camadas (Domain, Application, Infrastructure e API). Ela oferece recursos completos para gestão de um negócio do setor alimentício, incluindo controle de estoque, cardápio, pedidos, pagamentos, programa de fidelidade, promoções e auditoria de acessos.

---

## Tecnologias

| Tecnologia | Versão |
|---|---|
| .NET / ASP.NET Core | 10 |
| Entity Framework Core | — |
| SQL Server | 2022 |
| Redis | 7.4 |
| Firebase | Authentication |
| xUnit | Testes unitários |
| Docker / Docker Compose | — |

---

## Arquitetura

O projeto segue a estrutura de camadas:

```
RaizesNordeste/
├── RaizesNordeste.Domain/          # Entidades, interfaces, validadores e exceções de domínio
├── RaizesNordeste.Application/     # Casos de uso, DTOs, mapeadores e serviços de aplicação
├── RaizesNordeste.Infrastructure/  # Repositórios, contexto EF, cache (Redis) e Firebase
├── RaizesNordesteApi/              # Controllers, middlewares, filtros e configuração da API
└── RaizesNordeste.UnitTests/       # Testes unitários
```

---

## Pré-requisitos

### Para execução com Docker
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução
- Portas `8080`, `1433` e `6379` livres na máquina

### Para execução sem Docker
- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) instalado
- SQL Server disponível localmente na porta `1433`
- Redis disponível localmente na porta `6379`

---

## Execução com Docker (Recomendado)

Esta é a forma mais simples de subir o ambiente completo. O Docker Compose levanta automaticamente a API, o SQL Server e o Redis.

**1. Clone o repositório**

```bash
git clone https://github.com/PTorini1/RaizesNordeste.git
cd RaizesNordeste
```

**2. Suba os containers**

Na raiz do projeto, execute:

```powershell
docker compose up --build
```

> O primeiro build pode levar alguns minutos. O SQL Server precisa ficar saudável antes de a API inicializar — isso é controlado automaticamente pelo `healthcheck` no `docker-compose.yml`.

**3. Aguarde a inicialização**

Quando a API estiver pronta, você verá no log uma mensagem semelhante a:

```
Now listening on: http://[::]:8080
```

**4. Acesse o Swagger**

```
http://localhost:8080/swagger
```

### Parando os containers

```powershell
# Para os containers sem apagar os dados
docker compose down

# Para os containers e apaga os volumes (banco e cache zerados)
docker compose down -v
```

---

## Execução sem Docker

**1. Clone o repositório**

```bash
git clone https://github.com/PTorini1/RaizesNordeste.git
cd RaizesNordeste
```

**2. Verifique as configurações de conexão**

As connection strings para o ambiente de desenvolvimento estão em `RaizesNordesteApi/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RaizesNordesteDb;User Id=sa;Password=RaizesNordeste@2026;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True",
    "Redis": "localhost:6379"
  }
}
```

> Se a sua máquina usar outro usuário, senha, porta ou instância do SQL Server, ajuste esse arquivo antes de executar.

**3. Restaure as dependências e compile**

```powershell
dotnet restore
dotnet build
```

**4. Execute a API**

```powershell
dotnet run --project RaizesNordesteApi
```

**5. Acesse o Swagger**

Com o perfil HTTP padrão, o Swagger ficará disponível em:

```
http://localhost:5286/swagger
```

---

## Banco de Dados e Migrations

Ao iniciar, a API executa automaticamente as migrations do Entity Framework Core. **Normalmente não é necessário rodar nenhum comando manual de migration.**

Caso precise rodar as migrations manualmente (por exemplo, após criar uma nova migration em desenvolvimento):

```powershell
dotnet ef database update --project RaizesNordeste.Infrastructure --startup-project RaizesNordesteApi
```

Para criar uma nova migration:

```powershell
dotnet ef migrations add NomeDaMigration --project RaizesNordeste.Infrastructure --startup-project RaizesNordesteApi
```

---

## Carga Inicial para Testes

Depois que a API estiver em execução, é necessário popular o banco com dados de teste. Utilize o endpoint de carga inicial:

**Com Docker:**

```http
POST http://localhost:8080/api/carga-inicial?reset=true
```

**Sem Docker (perfil local):**

```http
POST http://localhost:5286/api/carga-inicial?reset=true
```

Você também pode executar pelo Swagger, acessando o endpoint `POST /api/carga-inicial`.

> **Parâmetro `reset=true`:** apaga e recria o banco antes de inserir os dados. Use quando quiser voltar para uma base limpa.

### O que é criado pela carga inicial

| Entidade | Descrição |
|---|---|
| Usuários | Usuários do sistema com roles |
| Clientes | Clientes cadastrados |
| Consentimentos LGPD | Registros de aceite de termos |
| Contas de fidelidade | Pontos e histórico |
| Unidades | Filiais do negócio |
| Produtos | Itens do cardápio |
| Cardápios | Agrupamentos de produtos por unidade |
| Estoques | Quantidades disponíveis por unidade |
| Movimentações de estoque | Entradas e saídas |
| Promoções | Descontos e ofertas |
| Pedidos | Pedidos realizados por clientes |
| Itens de pedido | Produtos dentro de cada pedido |
| Pagamentos | Transações financeiras |
| Movimentações de pontos | Acúmulo e resgate de fidelidade |
| Auditorias | Histórico de acessos aos endpoints |

> **Atenção:** o endpoint de carga inicial está sem autenticação (`[Authorize]`) propositalmente, para facilitar testes e avaliação. Em produção, esse endpoint deve ser protegido ou removido.

---

## Login Inicial

Após executar a carga inicial, use o usuário administrador abaixo para fazer o primeiro login:

```
Email: jose@gmail.com
Senha: !J0s23s1lv4
```

**Endpoint:**

```http
POST /api/auth/login-credentials
```

**Corpo da requisição:**

```json
{
  "email": "jose@gmail.com",
  "senha": "!J0s23s1lv4"
}
```

A resposta retorna um token JWT. No Swagger, clique em **Authorize** (ícone de cadeado) e informe o token no formato:

```
<seu-token-aqui>
```

### Criando seu próprio usuário

Após autenticar como administrador, você pode criar novos usuários pelo endpoint:

```http
POST /api/usuario
```

**Exemplo de corpo:**

```json
{
  "nome": "Professor",
  "email": "professor@email.com",
  "senha": "Senha@123",
  "role": "Admin",
  "ativo": true
}
```

**Roles disponíveis:** `Admin`, `Gerente`, `Cozinha`

---

## Endpoints Disponíveis

Todos os endpoints estão documentados e testáveis via Swagger. Abaixo um resumo por módulo:

| Módulo | Prefixo |
|---|---|
| Autenticação | `/api/auth` |
| Usuários | `/api/usuario` |
| Unidades | `/api/unidade` |
| Produtos | `/api/produto` |
| Cardápios | `/api/cardapio` |
| Estoque | `/api/estoque` |
| Pedidos | `/api/pedido` |
| Pagamentos | `/api/pagamento` |
| Promoções | `/api/promocao` |
| Fidelidade | `/api/fidelidade` |
| Auditoria | `/api/auditoria` |
| Carga Inicial | `/api/carga-inicial` |

---

## Executando os Testes

Para rodar todos os testes unitários:

```powershell
dotnet test
```

Para rodar com relatório detalhado:

```powershell
dotnet test --verbosity normal
```

---

## Comandos Úteis

```powershell
# Restaurar pacotes NuGet
dotnet restore

# Compilar a solução
dotnet build

# Executar a API localmente
dotnet run --project RaizesNordesteApi

# Rodar os testes
dotnet test

# Ver logs da API no Docker
docker compose logs -f api

# Ver status dos containers
docker compose ps

# Recriar apenas o container da API (sem recriar banco/redis)
docker compose up --build api

# Parar e remover containers e volumes
docker compose down -v
```
