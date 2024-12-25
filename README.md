# API RESTful de Cadastro e Autenticação de Usuários  

Esta API de autenticação foi desenvolvida utilizando **ASP.NET Core** e implementa funcionalidades básicas de autenticação e autorização de usuários, como registro, login, logout e geração de tokens JWT. O projeto inclui também a lógica para revogação e validação de tokens, tornando-o adequado para aplicações que requerem segurança e controle de acesso.

## 🚀 Funcionalidades  
- **Registro de Usuários:** Criação de novos usuários com validação de campos.
- **Login de Usuários:** Autenticação via email e senha com geração de token **JWT (JSON Web Token)**.
- **Logout:** Revogação de tokens JWT para impedir uso futuro.
- **Validação de Token:** Verificação de tokens revogados.
- **Armazenamento Seguro:** Senhas armazenadas de forma segura utilizando hashing **(BCrypt)**.
- **Documentação da API:** Gerada automaticamente com **Swagger**.  

## 🛠️ Tecnologias e Ferramentas Utilizadas  
- **ASP.NET Core**: Framework principal para desenvolvimento da API.  
- **Entity Framework Core**: ORM para interação com o banco de dados relacional.  
- **BCrypt**: Ferramenta para hashing de senhas.  
- **JWT**: Autenticação segura baseada em tokens.  
- **Swagger**: Documentação interativa da API.
- Banco de dados relacional (compatível com SQLite, SQL Server, PostgreSQL, etc.).

## 📚 Arquitetura do Projeto  
A API foi desenvolvida utilizando a **Arquitetura em Camadas**, separando responsabilidades para maior organização e escalabilidade:  
- **Presentation Layer:** Responsável por receber e retornar os dados das requisições.  
- **Application Layer:** Contém a lógica de negócios implementada em serviços.  
- **Persistence Layer:** Cuida das operações de banco de dados e uso do Entity Framework Core.  
- **Data Transfer Objects (DTOs):** Utilizados para transportar dados entre camadas sem expor entidades de domínio.  

## 🖼️ Estrutura de Pastas 
- **Presentation** -> **Controllers:** Lógica de roteamento e resposta HTTP (ex.: `AuthController.cs`).
- **Application** -> **Services:** Contém as regras de negócio (ex.: `UserService.cs`).
- **Infrastructure** -> **Repositories:** Lógica de acesso ao banco de dados (ex.: `UserRepository.cs`).
- **Application** -> **DTOs:** Modelos para transferência de dados (ex.: `UserDto.cs`, `LoginDTO.cs`).
- **Domain** -> **Entities:** Representação das tabelas no banco de dados (ex.: `User.cs`, `RevokedToken.cs`).
- **Domain** -> **Interfaces:** Interfaces da aplicação (ex.: `IUserRepository.cs`, `IUserService.cs`).
  
- **/src
- **├── Presentation/ # Controllers e configurações de rotas.
- **├── Application/ # Regras de negócio e validações.
- **├── Persistence/ # Acesso a banco de dados, repositórios e contexto.
- **└── Domain/ # Entidades e contratos.
