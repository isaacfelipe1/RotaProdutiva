# RotaProdutiva

API que conecta **pessoas em busca de qualificação** a **voluntários dispostos a ensinar**, promovendo cursos de geração de renda alinhados ao **ODS 4 (Educação de Qualidade)** e **ODS 10 (Redução das Desigualdades)**. Qualquer pessoa cadastrada pode se inscrever em cursos oferecidos por tutores/voluntários, com fluxo de aprovação administrativa para cadastro de tutores. Desenvolvida em **.NET 8** seguindo os princípios de **Clean Architecture**.

## 🏗️ Arquitetura

O projeto é dividido em camadas, cada uma em seu próprio projeto:

| Projeto                        | Responsabilidade                                                                              |
| ------------------------------ | --------------------------------------------------------------------------------------------- |
| `RotaProdutiva.Domain`         | Entidades, enums, exceções de domínio e interfaces de repositório (sem dependências externas) |
| `RotaProdutiva.Application`    | Regras de negócio, serviços, DTOs e interfaces de aplicação                                   |
| `RotaProdutiva.Infrastructure` | Implementação de repositórios, `DbContext` (EF Core), autenticação/hash de senha e migrations |
| `RotaProdutiva.API`            | Controllers REST, configuração de autenticação JWT, Swagger e composição da aplicação         |
| `RotaProdutiva.Tests`          | Testes automatizados                                                                          |

## 🛠️ Tecnologias

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core (PostgreSQL)
- Autenticação JWT Bearer
- Swagger / OpenAPI

## 👥 Perfis de usuário

- **Jovem**: tipo de conta padrão para quem busca se qualificar. Apesar do nome, não há restrição de idade ou público — qualquer pessoa pode se cadastrar com esse perfil.
- **Tutor**: voluntário que pode criar e gerenciar cursos, mas depende de **aprovação de um administrador** antes de conseguir publicar conteúdo. Tutores também podem se inscrever em cursos de outros tutores (exceto nos próprios cursos que criaram).
- **Admin**: aprova ou rejeita solicitações de tutores e tem acesso total à plataforma.

A inscrição em cursos não é restrita a um perfil: **qualquer pessoa**, cadastrada ou não na plataforma, pode se inscrever.

> ℹ️ A inscrição em cursos é **pública**: não é necessário estar autenticado ou possuir cadastro na plataforma para se inscrever. Basta informar nome, e-mail e WhatsApp para contato.

### Fluxo de aprovação de Tutor

1. Ao se registrar como `Tutor`, o usuário é criado com `StatusAprovacao = Pendente`.
2. Um `Admin` consulta a lista de tutores pendentes e aprova ou rejeita a solicitação.
3. Um tutor **rejeitado** não consegue fazer login.
4. Um tutor **pendente** consegue fazer login, mas não pode criar cursos até ser aprovado.
5. Somente tutores com `StatusAprovacao = Aprovado` (ou administradores) podem criar cursos.

## 🔐 Autenticação

A API utiliza autenticação via **JWT Bearer**. Após login/registro, um token é retornado e deve ser enviado no header:

```
Authorization: Bearer {seu token}
```

## 📋 Principais Endpoints

### Auth (`/api/auth`)

| Método | Rota                  | Descrição                                        | Acesso  |
| ------ | --------------------- | ------------------------------------------------ | ------- |
| POST   | `/api/auth/registrar` | Registra um novo usuário (Jovem, Tutor ou Admin) | Público |
| POST   | `/api/auth/login`     | Autentica o usuário e retorna o token JWT        | Público |

### Cursos (`/api/cursos`)

| Método | Rota                          | Descrição                | Acesso                    |
| ------ | ----------------------------- | ------------------------ | ------------------------- |
| GET    | `/api/cursos`                 | Lista todos os cursos    | Público                   |
| GET    | `/api/cursos/{id}`            | Obtém um curso por Id    | Público                   |
| GET    | `/api/cursos/tutor/{tutorId}` | Lista cursos de um tutor | Público                   |
| POST   | `/api/cursos`                 | Cria um novo curso       | Tutor (aprovado) ou Admin |

Ao criar um curso, além de `Titulo` e `Descricao`, é necessário informar:

| Campo          | Tipo       | Descrição                                                              |
| -------------- | ---------- | ---------------------------------------------------------------------- |
| `DataInicio`   | `DateTime` | Data de início do curso (deve ser futura)                              |
| `CargaHoraria` | `int`      | Carga horária total, em horas (maior que zero)                         |
| `Vagas`        | `int`      | Número de vagas disponíveis (maior que zero)                           |
| `Modalidade`   | `string`   | `Presencial` ou `Online` (`ModalidadeCurso`)                           |
| `Local`        | `string?`  | Obrigatório quando `Modalidade = Presencial`; ignorado quando `Online` |

Cada curso também expõe `VagasDisponiveis`, calculado como `Vagas` menos o número de inscrições ativas/concluídas (inscrições canceladas não contam).

### Inscrições (`/api/inscricoes`)

| Método | Rota                              | Descrição                                               | Acesso       |
| ------ | --------------------------------- | ------------------------------------------------------- | ------------ |
| POST   | `/api/inscricoes`                 | Inscreve uma pessoa (nome, email, whatsapp) em um curso | Público      |
| GET    | `/api/inscricoes/curso/{cursoId}` | Lista inscritos de um curso                             | Tutor, Admin |

Ao se inscrever, é necessário informar:

| Campo      | Tipo     | Descrição                       |
| ---------- | -------- | ------------------------------- |
| `CursoId`  | `Guid`   | Id do curso a se inscrever      |
| `Nome`     | `string` | Nome completo do interessado    |
| `Email`    | `string` | E-mail para contato             |
| `WhatsApp` | `string` | Número de WhatsApp para contato |

Regras de inscrição (`InscricaoService.CriarAsync`):

1. Curso deve existir.
2. Não pode haver inscrição ativa duplicada do mesmo e-mail no mesmo curso.
3. Deve haver `VagasDisponiveis > 0`.

### Admin (`/api/admin`)

| Método | Rota                               | Descrição                          | Acesso |
| ------ | ---------------------------------- | ---------------------------------- | ------ |
| GET    | `/api/admin/tutores/pendentes`     | Lista tutores aguardando aprovação | Admin  |
| POST   | `/api/admin/tutores/{id}/aprovar`  | Aprova o cadastro de um tutor      | Admin  |
| POST   | `/api/admin/tutores/{id}/rejeitar` | Rejeita o cadastro de um tutor     | Admin  |

## ⚙️ Configuração

As configurações ficam em `RotaProdutiva.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=inclusao_produtiva;Username=postgres;Password=sua_senha_aqui"
  },
  "JwtSettings": {
    "Chave": "ChaveSecretaSuperSeguraParaAssinaturaDoTokenJWT-TrocarEmProducao",
    "Emissor": "RotaProdutiva.API",
    "Audiencia": "RotaProdutiva.Clientes",
    "ExpiracaoMinutos": 60
  },
  "AdminSeed": {
    "Nome": "Administrador",
    "Email": "admin@rotaprodutiva.com",
    "Senha": "Admin@123",
    "WhatsApp": "00000000000"
  }
}
```

> ⚠️ Altere `JwtSettings:Chave` e as credenciais de `AdminSeed` antes de subir para produção.

Ao iniciar, a aplicação aplica as migrations do EF Core automaticamente e cria (via _seed_) um usuário administrador com base nas configurações de `AdminSeed`, caso ainda não exista.

## 🚀 Como executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL em execução (local ou container)

### Passos

1. Configure a `ConnectionStrings:DefaultConnection` em `appsettings.json` (ou `appsettings.Development.json`) apontando para o seu banco PostgreSQL.
2. Restaure as dependências e execute a API:

```powershell
dotnet restore
dotnet run --project RotaProdutiva.API
```

3. As migrations serão aplicadas automaticamente ao iniciar.
4. Acesse a documentação interativa (Swagger) em:

```
https://localhost:{porta}/swagger
```

### Executando os testes

```powershell
dotnet test
```

## 📁 Estrutura de pastas (resumo)

```
RotaProdutiva.Domain/
  Entities/        -> Usuario, Curso, Inscricao (Nome, Email, WhatsApp), EntidadeBase
  Enums/           -> TipoUsuario, StatusAprovacaoTutor, StatusInscricao, ModalidadeCurso
  Interfaces/       -> IUsuarioRepository, ICursoRepository, IInscricaoRepository
  Exceptions/       -> DomainException

RotaProdutiva.Application/
  DTOs/            -> Auth, Cursos, Inscricoes, Admin
  Interfaces/       -> IAuthService, ICursoService, IInscricaoService, IAdminService, ...
  Services/         -> AuthService, CursoService, InscricaoService, AdminService

RotaProdutiva.Infrastructure/
  Data/            -> AppDbContext, Migrations
  Repositories/     -> UsuarioRepository, CursoRepository, InscricaoRepository
  Security/         -> TokenService, PasswordHasher, JwtSettings

RotaProdutiva.API/
  Controllers/      -> AuthController, CursosController, InscricoesController, AdminController
  Program.cs         -> Composição da aplicação, autenticação JWT, seed de admin
```

## 👤 Autor

Por: Isaac Felipe dos Santos Lima
Email: ifdsl.lic20@uea.edu
