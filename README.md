# API RESTful de Cadastro e Autenticação de Usuários  

Bem-vindo(a) ao repositório da API RESTful desenvolvida em **ASP.NET Core**! Este projeto implementa funcionalidades de cadastro e autenticação de usuários, seguindo boas práticas de desenvolvimento, segurança e organização em camadas.  

## 🚀 Funcionalidades  
- **Registro de Usuários:** Criação de novos usuários com validação de campos.
- **Login de Usuários:** Autenticação via email e senha com geração de token **JWT (JSON Web Token)**.
- **Logout:** Revogação de tokens JWT para impedir uso futuro.
- **Validação de Token:** Verificação de tokens revogados.
- **Armazenamento Seguro:** Senhas armazenadas de forma segura utilizando hashing **(BCrypt)**.
- Documentação da API gerada automaticamente com **Swagger**.  

## 🛠️ Tecnologias e Ferramentas Utilizadas  
- **ASP.NET Core**: Framework principal para desenvolvimento da API.  
- **Entity Framework Core**: ORM para interação com o banco de dados relacional.  
- **BCrypt**: Ferramenta para hashing de senhas.  
- **JWT**: Autenticação segura baseada em tokens.  
- **Swagger**: Documentação interativa da API.  

## 📚 Arquitetura do Projeto  
A API foi desenvolvida utilizando a **Arquitetura em Camadas**, separando responsabilidades para maior organização e escalabilidade:  
- **Presentation Layer:** Responsável por receber e retornar os dados das requisições.  
- **Application Layer:** Contém a lógica de negócios implementada em serviços.  
- **Persistence Layer:** Cuida das operações de banco de dados e uso do Entity Framework Core.  
- **Data Transfer Objects (DTOs):** Utilizados para transportar dados entre camadas sem expor entidades de domínio.  

### 🖼️ Estrutura de Pastas  
