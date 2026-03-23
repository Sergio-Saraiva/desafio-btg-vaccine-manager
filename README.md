# Vaccine Manager

Sistema fullstack para gerenciamento de vacinação, permitindo o cadastro de pessoas, vacinas e registros de vacinação com carteira de vacinação digital.

## Sumario

- [Visao Geral](#visao-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Decisoes Tecnicas](#decisoes-tecnicas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pre-requisitos](#pre-requisitos)
- [Como Executar](#como-executar)
- [API Endpoints](#api-endpoints)
- [Testes](#testes)

## Visao Geral

O Vaccine Manager e um sistema completo de gerenciamento de vacinacao que permite:

- **Autenticacao**: Cadastro e login de usuarios com JWT
- **Gestao de Pessoas**: CRUD completo com validacao de documentos (CPF e Passaporte)
- **Gestao de Vacinas**: CRUD com geracao automatica de codigos unicos
- **Registros de Vacinacao**: Controle de doses aplicadas com limite por vacina
- **Carteira de Vacinacao**: Consulta consolidada de todas as vacinas de uma pessoa

A arquitetura foi pensada visando escalabilidade futura — em um cenario real, o sistema poderia atender toda a populacao brasileira mantendo separacao clara de responsabilidades e facilidade de manutencao.

## Arquitetura

O projeto segue uma arquitetura em camadas inspirada em Clean Architecture com CQRS (Command Query Responsibility Segregation):

```
┌─────────────────────────────────────────────────────┐
│                    Frontend (UI)                     │
│              React + TanStack Router                 │
│              http://localhost:5173                   │
└──────────────────────┬──────────────────────────────┘
                       │ HTTP (axios)
                       ▼
┌─────────────────────────────────────────────────────┐
│                  API (Presentation)                  │
│         ASP.NET Core Controllers + Swagger           │
│         JWT Authentication Middleware                 │
│              http://localhost:5254                    │
└──────────────────────┬──────────────────────────────┘
                       │ MediatR
                       ▼
┌─────────────────────────────────────────────────────┐
│                   Application                        │
│   Commands / Queries / Handlers / Validators         │
│   Pipeline Behaviors (Logging, Validation, Exception)│
└──────────────────────┬──────────────────────────────┘
                       │ Interfaces
                       ▼
┌──────────────────┐  ┌──────────────────────────────┐
│     Domain       │  │       Infrastructure          │
│  Entities, Enums │◄─│  EF Core, Repositories,       │
│  Repositories    │  │  SQLite, Sieve, JWT Service   │
│  (interfaces)    │  │  (implementacoes)             │
└──────────────────┘  └──────────────────────────────┘
```

### Fluxo de uma Requisicao

```
Request HTTP
    │
    ▼
Controller ──► MediatR.Send()
                   │
                   ▼
            ExceptionBehavior  (captura excecoes)
                   │
                   ▼
            LoggingBehavior    (loga request/response + tempo)
                   │
                   ▼
            ValidationBehavior (FluentValidation)
                   │
                   ▼
            Handler            (logica de negocio)
                   │
                   ▼
            Repository / UoW   (persistencia)
                   │
                   ▼
            Result<T>          (FluentResults)
                   │
                   ▼
            Controller ──► ApiResponse<T> (JSON)
```

## Tecnologias

### Backend (.NET 10)

| Tecnologia | Finalidade |
|---|---|
| **ASP.NET Core** | Framework web e API REST |
| **MediatR** | Mediador para CQRS (Commands e Queries) |
| **FluentValidation** | Validacao declarativa de requests |
| **FluentResults** | Encapsulamento de sucesso/falha sem excecoes |
| **Entity Framework Core** | ORM e migrations |
| **SQLite** | Banco de dados relacional embutido |
| **Sieve** | Filtragem, ordenacao e paginacao via query string |
| **Argon2id** | Hashing de senhas (Konscious.Security) |
| **JWT Bearer** | Autenticacao via tokens |

### Frontend (React 19)

| Tecnologia | Finalidade |
|---|---|
| **React 19** | Biblioteca UI |
| **TanStack Router** | Roteamento type-safe com code splitting |
| **TanStack React Query** | Cache de dados do servidor e mutacoes |
| **Tailwind CSS 4** | Estilizacao utility-first |
| **shadcn/ui + Radix** | Componentes acessiveis |
| **Axios** | Cliente HTTP com interceptors |
| **Vite** | Bundler e dev server |

### Testes

| Tecnologia | Finalidade |
|---|---|
| **xUnit** | Framework de testes |
| **NSubstitute** | Mocking de dependencias |
| **FluentAssertions** | Assertions expressivas |

## Decisoes Tecnicas

### Por que Argon2id para hashing de senhas?

Argon2id e o algoritmo vencedor da Password Hashing Competition e recomendado pela OWASP como a melhor opcao atual. Diferente de bcrypt ou PBKDF2, ele e resistente tanto a ataques de GPU quanto a ataques de side-channel, combinando resistencia a memoria (Argon2i) e resistencia a tradeoff (Argon2d). Os parametros utilizados (19 MB de memoria, 2 iteracoes) seguem as recomendacoes minimas da OWASP para ambientes interativos.

### Por que Sieve para filtragem e paginacao?

O Sieve permite expor filtragem, ordenacao e paginacao avancada via query string sem escrever logica manual em cada endpoint. O frontend monta os parametros (ex: `?Filters=Name@=*João&Sorts=-CreatedAt&Page=1&PageSize=10`) e o backend aplica automaticamente sobre os `IQueryable` do EF Core. Isso elimina codigo repetitivo e garante consistencia em todos os endpoints de listagem.

### Por que TanStack Router?

O TanStack Router oferece roteamento totalmente type-safe com inferencia de tipos para parametros de rota, search params e loader data. Combinado com o plugin Vite, gera automaticamente a arvore de rotas com code splitting, eliminando imports manuais e reduzindo o bundle inicial. O `beforeLoad` hook permite implementar guards de autenticacao de forma declarativa em cada rota.

### Por que SQLite?

SQLite foi escolhido por portabilidade e simplicidade de setup — o banco de dados e um unico arquivo que viaja junto com o projeto, sem necessidade de instalar ou configurar um servidor externo. Para o escopo atual e ideal, e a migração para PostgreSQL ou SQL Server futuramente requer apenas trocar o provider no EF Core.

### Por que CQRS com MediatR?

A separacao entre Commands (escrita) e Queries (leitura) via MediatR permite que cada operacao tenha seu proprio handler isolado, facilitando testes unitarios e manutencao. Os Pipeline Behaviors (Validation, Logging, Exception) sao aplicados automaticamente a todos os handlers sem duplicacao de codigo cross-cutting.

### Por que FluentResults ao inves de excecoes?

O uso de `Result<T>` para representar sucesso/falha evita o uso de excecoes para fluxo de controle. Erros de negocio (ex: email duplicado, vacina nao encontrada) sao retornados como `Result.Fail()` com um `ApiError` tipado que inclui o status HTTP correspondente. Isso torna o fluxo de erro explicito e testavel.

## Estrutura do Projeto

```
vaccine-manager/
├── apps/
│   ├── VaccineManager.Api/            # API ASP.NET Core
│   │   ├── Controllers/               # Endpoints REST
│   │   ├── Middlewares/                # Global exception handler
│   │   └── Program.cs                 # Configuracao do host (JWT, Swagger, CORS)
│   └── ui/                            # Frontend React
│       └── src/
│           ├── components/ui/          # Componentes shadcn/ui
│           ├── hooks/                  # Custom hooks (React Query wrappers)
│           ├── lib/                    # Axios, auth helpers, utils
│           ├── pages/                  # Componentes de pagina
│           ├── routes/                 # Rotas TanStack Router (file-based)
│           ├── services/               # Chamadas API tipadas
│           └── types/                  # Tipos TypeScript
├── libs/backend/
│   ├── VaccineManager.Domain/          # Entidades, Enums, Interfaces de repositorio
│   ├── VaccineManager.Application/     # Commands, Queries, Handlers, Validators, Behaviors
│   ├── VaccineManager.Infrastructure/  # EF Core, Repositories, Migrations, Sieve configs
│   └── VaccineManager.IOC/            # Composicao de DI
├── tests/backend/
│   └── VaccineManager.Application.Tests/  # Testes unitarios dos handlers e validators
└── docs/
    └── openapi.json                   # Especificacao OpenAPI 3.0
```

### Entidades do Dominio

| Entidade | Descricao |
|---|---|
| **Person** | Pessoa com nome, documento (CPF/Passaporte), nacionalidade. Suporta soft delete com cascata nos registros de vacinacao. |
| **Vaccine** | Vacina com nome, codigo unico (auto-gerado ou manual) e numero de doses requeridas. |
| **VaccinationRecord** | Registro de aplicacao de dose, vinculando Person e Vaccine com data de aplicacao. |
| **User** | Usuario do sistema com email e senha (hash Argon2id). |

Todas as entidades herdam de `BaseEntity` que fornece `Id` (GUID v7), `CreatedAt`, `UpdatedAt` e `DeletedAt` (soft delete). O `AppDbContext` aplica automaticamente um global query filter para excluir registros com `DeletedAt != null`.

## Pre-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) (recomendado 20+)
- npm (vem com o Node.js)

## Como Executar

### Backend

```bash
# Na pasta scripts/ef-core
./db-uptate.sh

# Na raiz do projeto para iniciar a API (porta 5254)
dotnet run --project apps/VaccineManager.Api
```

A API estara disponivel em `http://localhost:5254` com Swagger UI na rota raiz.

### Frontend

```bash
# Entrar no diretorio do frontend
cd apps/ui

# Instalar dependencias
npm install

# Iniciar o dev server (porta 5173)
npm run dev
```

O frontend estara disponivel em `http://localhost:5173`. Ao acessar, voce sera redirecionado para a tela de login. Crie uma conta em Sign Up e faca login para acessar o sistema.

## API Endpoints

Todos os endpoints (exceto autenticacao) requerem um token JWT no header `Authorization: Bearer <token>`.

### Autenticacao

| Metodo | Rota | Descricao |
|---|---|---|
| POST | `/api/Auth/signup` | Criacao de conta |
| POST | `/api/Auth/signin` | Login (retorna JWT) |

### Pessoas

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/api/Persons` | Listar com filtros e paginacao |
| POST | `/api/Persons` | Criar pessoa |
| PUT | `/api/Persons` | Atualizar pessoa |
| DELETE | `/api/Persons/{id}` | Excluir pessoa (soft delete) |
| GET | `/api/Persons/{id}/vaccination-card` | Carteira de vacinacao |

### Vacinas

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/api/Vaccines` | Listar com filtros e paginacao |
| POST | `/api/Vaccines` | Criar vacina |
| PUT | `/api/Vaccines` | Atualizar vacina |
| DELETE | `/api/Vaccines/{id}` | Excluir vacina |

### Registros de Vacinacao

| Metodo | Rota | Descricao |
|---|---|---|
| POST | `/api/VaccinationRecord` | Registrar dose |
| DELETE | `/api/VaccinationRecord/{id}` | Remover registro |

A documentacao completa da API esta disponivel no Swagger UI (`http://localhost:5254/swagger`) e no arquivo `docs/openapi.json`.

## Testes

```bash
# Executar todos os testes
dotnet test

# Executar apenas testes de um modulo
dotnet test --filter "FullyQualifiedName~Users"
dotnet test --filter "FullyQualifiedName~Vaccines"
dotnet test --filter "FullyQualifiedName~VaccinationRecords"
```

Os testes cobrem:

- **Handlers**: Logica de negocio dos commands e queries (criacao, atualizacao, exclusao, cenarios de erro)
- **Validators**: Regras de validacao do FluentValidation (campos obrigatorios, formatos, limites)

Cada teste segue o padrao AAA (Arrange, Act, Assert) com mocks via NSubstitute e assertions via FluentAssertions.
