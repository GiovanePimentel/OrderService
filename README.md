# Order Service

API REST desenvolvida em ASP.NET Core 9 para gerenciamento de pedidos, utilizando PostgreSQL, Entity Framework Core, JWT Authentication e Docker.

## Tecnologias

- ASP.NET Core 9
- Entity Framework Core 9
- PostgreSQL
- JWT Authentication
- FluentValidation
- xUnit
- Docker / Docker Compose

## Funcionalidades

- Criar pedido
- Confirmar pedido
- Cancelar pedido
- Consultar pedido por ID
- Listar pedidos
- Autenticação JWT
- Migrations automáticas ao iniciar a aplicação

## Regras de Negócio

### Criação de Pedido

- Pedido deve possuir ao menos um item
- Quantidade deve ser maior que zero
- Produto deve existir
- Quantidade solicitada não pode exceder o estoque disponível
- Total do pedido é calculado automaticamente

Status inicial:

```text
Placed
```

### Confirmação

- Apenas pedidos em status Placed podem ser confirmados
- Confirmação realiza baixa de estoque
- Operação é idempotente

Transição:

```text
Placed -> Confirmed
```

### Cancelamento

- Permite cancelar pedidos Placed ou Confirmed
- Estoque é restaurado quando necessário
- Operação é idempotente

Transição:

```text
Placed/Confirmed -> Canceled
```

## Como Executar

### Pré-requisitos

- Docker Desktop
- Docker Compose

### Subindo a aplicação

Na raiz do projeto execute:

```bash
docker compose up --build
```

A API ficará disponível em:

```text
http://localhost:8080
```

Swagger:

```text
http://localhost:8080/swagger
```

## Autenticação

Obter token:

### POST /auth/token

Request:

```json
{
  "username": "admin",
  "password": "admin"
}
```

Response:

```json
{
  "token": "jwt-token"
}
```

Utilize o token no header:

```text
Authorization: Bearer {token}
```

## Endpoints

### POST /auth/token

Gera token JWT.

### POST /orders

Cria um pedido.

### POST /orders/{id}/confirm

Confirma um pedido.

### POST /orders/{id}/cancel

Cancela um pedido.

### GET /orders/{id}

Consulta um pedido específico.

### GET /orders

Lista pedidos com paginação.

## Executando Testes

```bash
dotnet test
```

## Banco de Dados

A aplicação utiliza PostgreSQL executando via Docker.

As migrations são aplicadas automaticamente durante a inicialização da API.

## Estrutura do Projeto

```text
src
├── api
│   ├── Controllers
│   ├── Services
│   ├── Models
│   ├── Validators
│   ├── Data
│   └── Migrations
│
└── test
    └── Unit Tests
```
