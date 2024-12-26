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

describe('Testes de Logout', () => {
  let authTokens = []; // Array para armazenar os tokens de autenticação

  // Pré-condição 1: Registrar o usuário
  before(() => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        name: nomeCadastrado,
        email: emailCadastrado,
        password: senhaCadastrada,
      },
    }).then((response) => {
      expect(response.status).to.equal(200);
      expect(response.body.message).to.equal('Usuário cadastrado com sucesso.');
    });

    // Pré-condição 2: Gerar duas sessões para o mesmo usuário
    for (let i = 0; i < 2; i++) {
      cy.api({
        method: 'POST',
        url: `${apiBaseUrl}/login`,
        body: {
          email: emailCadastrado,
          password: senhaCadastrada,
        },
      }).then((response) => {
        expect(response.status).to.equal(200);
        expect(response.body.token).to.exist;
        expect(response.body.token).to.be.a('string').and.not.to.be.empty;

        // Armazenar o token na lista
        authTokens.push(response.body.token); 
      });
    }
  });

  // Teste 1: Realizar logout com sucesso usando o primeiro token
  it('Deve realizar logout com sucesso usando o primeiro token', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/logout`,
      headers: {
        Authorization: `Bearer ${authTokens[0]}`, // Usando o primeiro token
      },
    }).then((response) => {
      expect(response.status).to.equal(200);
      expect(response.body.message).to.equal('Logout realizado com sucesso.');
    });
  });

  // Teste 2: Realizar logout com sucesso usando o segundo token
  it('Deve realizar logout com sucesso usando o segundo token', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/logout`,
      headers: {
        Authorization: `Bearer ${authTokens[1]}`, // Usando o segundo token
      },
    }).then((response) => {
      expect(response.status).to.equal(200);
      expect(response.body.message).to.equal('Logout realizado com sucesso.');
    });
  });

  // Teste 3: Não permitir logout de um usuário já desconectado de duas sessões (tokens inválidos)
  it('Não deve permitir logout com tokens/sessões inválidas', () => {
    for (let i = 0; i < 2; i++) {
      cy.api({
        method: 'POST',
        url: `${apiBaseUrl}/logout`,
        headers: {
          Authorization: `Bearer ${authTokens[i]}`, 
        },
        failOnStatusCode: false // Adicionado para permitir o status 400
      }).then((response) => {
        expect(response.status).to.equal(401);
      });
    }
  });

   // Teste 4: Não permitir logout sem autenticação
   it('Não deve permitir logout sem autenticação', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/logout`,
      failOnStatusCode: false, // Não falha automaticamente
    }).then((response) => {
      expect(response.status).to.equal(401);
    });
  });
})
