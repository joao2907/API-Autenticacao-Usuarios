# API RESTful de Cadastro e Autenticação de Usuários  

Esta API de autenticação foi desenvolvida utilizando **ASP.NET Core** e implementa funcionalidades básicas de autenticação e autorização de usuários, como registro, login, logout e geração de tokens JWT. O projeto inclui também a lógica para revogação e validação de tokens, tornando-o adequado para aplicações que requerem segurança e controle de acesso.

## 🚀 Funcionalidades  
- **Registro de Usuários:** Criação de novos usuários com validação de campos.
- **Login de Usuários:** Realiza a autenticação de usuários via e-mail e senha, com geração de token **JWT (JSON Web Token)** para autenticação em futuras requisições..
- **Logout:** Revogação de tokens JWT para impedir uso futuro.
- **Validação de Token:** Permite a verificação de tokens revogados para garantir que não sejam reutilizados.
- **Armazenamento Seguro de Senhas:** Utiliza **BCrypt** para realizar o hash seguro das senhas antes de armazená-las no banco de dados.
- **Documentação da API:** A documentação interativa é gerada automaticamente utilizando **Swagger**, facilitando o entendimento e a integração com a API.  

## 🛠️ Tecnologias e Ferramentas Utilizadas  
- **ASP.NET Core**: Framework principal para desenvolvimento da API.  
- **Entity Framework Core**: ORM para interação com o banco de dados relacional.  
- **BCrypt**: Biblioteca para realizar o hash de senhas de forma segura. 
- **JWT**: Sistema de autenticação baseado em tokens para garantir a segurança nas requisições.  
- **Swagger**: Ferramenta para gerar e disponibilizar a documentação interativa da API.
- Banco de Dados Relacional: Compatível com diversos bancos como SQLite, SQL Server, PostgreSQL, entre outros.

## 📚 Arquitetura do Projeto  
A API foi desenvolvida utilizando a **Arquitetura em Camadas**, separando responsabilidades para maior organização e escalabilidade:  
- **Presentation Layer:** Responsável por controlar as requisições HTTP e enviar respostas ao cliente (ex.: Controllers, como o `AuthController.cs`).
- **Application Layer:** Contém a lógica de negócios, sendo composta por serviços que implementam as regras da aplicação (ex.: `UserService.cs`). 
- **Persistence Layer:** Responsável pela interação com o banco de dados, utilizando o Entity Framework Core para realizar operações de CRUD (ex.: `UserRepository.cs`).  
- **Data Transfer Objects (DTOs):** Modelos utilizados para a transferência de dados entre as camadas, sem expor diretamente as entidades do banco de dados (ex.: `UserDto.cs`, `LoginDTO.cs`).
- **Domain Layer:** Contém as entidades de domínio que representam as tabelas no banco de dados (ex.: `User.cs`, `RevokedToken.cs`).
- **Interfaces:** Definem contratos para os repositórios e serviços, facilitando a implementação e a manutenção do código (ex.: `IUserRepository.cs`, `IUserService.cs`).

## 🖼️ Estrutura de Pastas 
- **Presentation** -> **Controllers:** Lógica de roteamento e resposta HTTP (ex.: `AuthController.cs`).
- **Application** -> **Services:** Contém as regras de negócio (ex.: `UserService.cs`).
- **Infrastructure** -> **Repositories:** Lógica de acesso ao banco de dados (ex.: `UserRepository.cs`).
- **Application** -> **DTOs:** Modelos para transferência de dados (ex.: `UserDto.cs`, `LoginDTO.cs`).
- **Domain** -> **Entities:** Representação das tabelas no banco de dados (ex.: `User.cs`, `RevokedToken.cs`).
- **Domain** -> **Interfaces:** Interfaces da aplicação (ex.: `IUserRepository.cs`, `IUserService.cs`).

## 🛡️ Segurança  
- **Hashing de Senhas**: As senhas são armazenadas de forma segura utilizando BCrypt, garantindo que não possam ser recuperadas mesmo em caso de vazamento de dados. 
- **Autenticação JWT**: A autenticação é baseada em tokens JWT, que são gerados durante o login e utilizados para validar a identidade do usuário em requisições subsequentes.

## 🛤️ Endpoints
| Método |      Endpoint      |                  Descrição        	      | Autenticação Requerida |
|--------|--------------------|-------------------------------------------|------------------------|
|  POST  | /api/auth/register | Registro de novos usuários                |          Não           |
|  POST  | /api/auth/login    | Autenticação de usuários e geração de JWT |          Não           |
|  POST  | /api/auth/logout   | Logout do usuário e revogação do token    |          Sim           |

## 📋 Como Executar  

### Pré-requisitos  
- **.NET SDK** instalado (versão 6.0 ou superior).  
- **Banco de Dados** (Ex.: SQL Server, configurado no `appsettings.json`).
