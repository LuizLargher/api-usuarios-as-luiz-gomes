# API de Gerenciamento de Usuários

## Descrição

O projeto trata-se de uma API Rest de Gerenciamento de Usuários para plataformas digitais, seguindo as boas práticas de desenvolvimento e padrões de projeto apresentados na disciplina de Desenvolvimento BackEnd.

Desenvolvida inteiramente em C# e ASP.NET com Minimal API, utilizando Entity Framework Core, FluentValidation, SQLite como Banco de Dados de padrão Code First utilizando Migrations e Postman para testes de requisição da API.

As principais funcionalidades da API são:

- Cadastro de usuários;
- Consulta de usuários por ID;
- Atualização de usuários;
- Remoção de usuários do sistema.

## Link do Vídeo Explicativo

> https://drive.google.com/file/d/1koH5aOJNByLAXDoCYQXt3sAV00wTC-AV/view?usp=sharing

## Tecnologias Utilizadas

- .NET 9.0
- SQLite
- Entity Framework Core
- FluentValidation

## Padrões de Projeto Implementados

- Repository Pattern
- Service Pattern
- DTO Pattern
- Dependency Injection

## Como Executar o Projeto

### Pré-requisitos

- .NET SDK 9.0 ou superior

### Passos

1 - Clone o repositório

```bash
git clone <link_repositorio>
cd APIUsuarios
```

2 - Execute as migrations

```sh
dotnet ef database update
```

3 - Execute a aplicação

```sh
dotnet restore
dotnet build
dotnet run
```

## Exemplos de Requisições

### GET | Listar todos os usuários

Send:

```bash
GET http://localhost:5000/usuarios
```

Saída Esperada: 200 OK

### GET | Buscar usuário pelo ID

Send:

```bash
GET http://localhost:5000/usuarios/1
```

Saída Esperada: 200 OK

### POST | Criar novo usuário

Send:

```bash
POST http://localhost:5000/usuarios
```

Acesse: Body -> Raw -> JSON

```json
{
    "nome": "Luiz Gomes",
    "email": "luiz@example.com",
    "senha": "123456",
    "dataNascimento": "2002-02-02",
    "ativo": false,
    "telefone": "11999999999"
}
```

Saída Esperada: 201 Created

### PUT | Atualizar usuário completo

Send:

```bash
PUT http://localhost:5000/usuarios/1
```

Acesse: Body -> Raw -> JSON

```json
{
    "nome": "Novo Luiz Gomes",
    "email": "novoemaildoluiz@example.com",
    "senha": "123456",
    "dataNascimento": "2002-02-02",
    "ativo": true,
    "telefone": "11999999999"
}
```

Saída Esperada: 200 OK

### DELETE | Remover usuário (Soft Delete)

Send:

```bash
DELETE http://localhost:5000/usuarios/1
```

Saída Esperada: 204 No Content

## Estrutura do Projeto

O projeto segue os princípios de Clean Architecture, dividindo as responsabilidades em camadas para garantir desacoplamento e testabilidade.

### APIUsuario

Contém o ponto de entrada da aplicação e configurações globais.

- **Program.cs**: Arquivo principal que configura a Injeção de Dependência, o Banco de Dados e define os Endpoints da API.

### Domain

O núcleo do sistema. Esta camada não depende de nenhuma outra.

- **Entities**: Contém a classe Usuario.cs, que representa a tabela do banco de dados e as regras de negócio corporativas.

### Application

Contém a lógica de negócio e os casos de uso. Depende apenas do Domain.

- **DTOs**: Data Transfer Objects. Classes usadas para transportar dados entre a API e o usuário.

- **Interfaces**: Contratos que definem o que o sistema faz, mas não como. Permite a inversão de dependência.

- **Services**: Implementação da lógica de negócio.

- **Validators**: Regras de validação de entrada utilizando a biblioteca FluentValidation.

### Infrastructure

Responsável pela comunicação com o mundo externo.

- **Persistence**: Contém o AppDbContext, que faz a ponte entre o código C# e o banco SQLite.

- **Repositories**: Implementação da interface IUsuarioRepository na classe UsuarioRepository onde o acesso ao banco de dados ocorre.

## Autor

Luiz Guilherme Largher Gomes

Curso: Análise e Desenvolvimento de Sistemas
