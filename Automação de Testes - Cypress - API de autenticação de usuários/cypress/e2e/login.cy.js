/// <reference types="cypress"/>
const { faker } = require('@faker-js/faker');

// Importar "cypress.config.js"
const apiBaseUrl = Cypress.env('apiBaseUrl');

// Gerar email e senha aleatórios para o usuário cadastrado
const nomeCadastrado = faker.name.fullName();
const emailCadastrado = faker.internet.email();
const senhaCadastrada = faker.internet.password();

// Gerar email e senha para o usuário não cadastrado
const emailNaoCadastrado = faker.internet.email();
const senhaNaoCadastrada = faker.internet.password();

describe('Testes de Login', () => {

  before(() => {
    // Pré-requisito: Cadastrar o usuário antes de realizar o login
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        name: nomeCadastrado,
        email: emailCadastrado,
        password: senhaCadastrada,
      },
    }).then((response) => {
      // Validar que o status da resposta é 200
      expect(response.status).to.equal(200)

      // Validar que a mensagem retornada no corpo da resposta é a que o usuário foi cadastrado
      expect(response.body.message).to.equal('Usuário cadastrado com sucesso.');
    });
  });

  it('Deve realizar login com sucesso usando credenciais válidas', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/login`,
      body: {
        "email": emailCadastrado,
        "password": senhaCadastrada
      }
    })
      .then((response) => {
        // Validar que o status da resposta é 200
        expect(response.status).to.equal(200);

        // Validar que o token existe no corpo da resposta
        expect(response.body.token).to.exist;

        // Validar que o token é uma string não vazia
        expect(response.body.token).to.be.a('string').and.not.to.be.empty;
      })
  })

  it('Deve falhar no login ao usar credenciais de usuário não cadastrado', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/login`,
      body: {
        "email": emailNaoCadastrado,
        "password": senhaNaoCadastrada
      },
      failOnStatusCode: false // Adicionado para permitir o status 401 ou 400
    }).then((response) => {
      // Validar que o status da resposta é 401
      expect(response.status).to.equal(401);

      // Validar que a mensagem retornada no corpo da resposta é de falha
      expect(response.body.message).to.equal("E-mail ou senha incorretos.");
    });
  });

  it('Deve falhar no login ao usar email válido, mas senha inválida', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/login`,
      body: {
        "email": emailCadastrado,
        "password": senhaNaoCadastrada
      },
      failOnStatusCode: false // Permitir validação de erro
    }).then((response) => {
      // Validar que o status da resposta é 401
      expect(response.status).to.equal(401);

      // Validar que a mensagem retornada no corpo da resposta é de falha
      expect(response.body.message).to.equal("E-mail ou senha incorretos.");
    });
  });

  it('Deve falhar no login ao usar senha válida, mas email inválido', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/login`,
      body: {
        "email": emailNaoCadastrado,
        "password": senhaCadastrada
      },
      failOnStatusCode: false // Permitir validação de erro
    }).then((response) => {
      // Validar que o status da resposta é 401
      expect(response.status).to.equal(401);

      // Validar que a mensagem retornada no corpo da resposta é de falha
      expect(response.body.message).to.equal("E-mail ou senha incorretos.");
    });
  });

  it('Deve falhar no login ao tentar logar sem preencher o campo "email"', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/login`,
      body: {
        "email": "",
        "password": senhaCadastrada
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro para campo obrigatório
        expect(response.body.message).to.equal("O campo 'email' é obrigatório.");
      })
  })

  it('Deve falhar no login ao tentar logar sem preencher o campo "password"', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/login`,
      body: {
        "email": emailCadastrado,
        "password": ""
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro para campo obrigatório
        expect(response.body.message).to.equal("O campo 'password' é obrigatório.");
      })
  })
})