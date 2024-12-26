/// <reference types="cypress"/>
const { faker } = require('@faker-js/faker');

// Importar "cypress.config.js"
const apiBaseUrl = Cypress.env('apiBaseUrl');

// Gerar email e senha aleatórios
const nomeAleatorio = faker.name.fullName();
const emailAleatorio = faker.internet.email();
const senhaAleatoria = faker.internet.password();

describe('Cadastro de Usuário', () => {

  it('Deve cadastrar usuário com sucesso', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "name": nomeAleatorio,
        "email": emailAleatorio,
        "password": senhaAleatoria
      }
    })
      .then((response) => {
        console.log('MINHA RESPOSTA: ', response)

        // Validar que o status da resposta é 200
        expect(response.status).to.equal(200);

        // Validar que a mensagem retornada no corpo da resposta é igual a tal
        expect(response.body.message).to.equal("Usuário cadastrado com sucesso.");
      })
  })

  it('Deve retornar erro ao tentar cadastrar usuário com email já registrado', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "name": nomeAleatorio,
        "email": emailAleatorio,
        "password": senhaAleatoria
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        console.log('MINHA RESPOSTA: ', response)

        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro para email duplicado
        expect(response.body.message).to.equal("Este email já está registrado.");
      })
  })

  it('Deve retornar erro ao tentar cadastrar usuário sem preencher o campo "nome"', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "name": "",
        "email": emailAleatorio,
        "password": senhaAleatoria
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro para campo obrigatório
        expect(response.body.message).to.equal("O campo 'name' é obrigatório.");
      })
  })

  it('Deve retornar erro ao tentar cadastrar usuário sem preencher o campo "email"', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "name": nomeAleatorio,
        "email": "",
        "password": senhaAleatoria
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

  it('Deve retornar erro ao tentar cadastrar usuário sem preencher o campo "senha"', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "name": nomeAleatorio,
        "email": emailAleatorio,
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

  it('Deve retornar erro ao tentar cadastrar usuário sem enviar o campo "nome" no JSON', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "email": emailAleatorio,
        "password": senhaAleatoria
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro no campo "Name"
        expect(response.body.errors.Name[0]).to.equal('The Name field is required.');
      })
  })

  it('Deve retornar erro ao tentar cadastrar usuário sem enviar o campo "email" no JSON', () => {
    cy.api({
      method: 'POST',
      url: `${apiBaseUrl}/register`,
      body: {
        "name": nomeAleatorio,
        "password": senhaAleatoria
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro no campo "Email"
        expect(response.body.errors.Email[0]).to.equal('The Email field is required.');
      })
  })

  it('Deve retornar erro ao tentar cadastrar usuário sem enviar o campo "senha" no JSON', () => {
    cy.api({
      method: 'POST',
      url: 'https://localhost:44362/api/Auth/register',
      body: {
        "name": nomeAleatorio,
        "email": emailAleatorio,
      },
      failOnStatusCode: false // Adicionado para permitir o status 400
    })
      .then((response) => {
        // Validar que o status da resposta é 400
        expect(response.status).to.equal(400);

        // Validar a mensagem de erro no campo "Password"
        expect(response.body.errors.Password[0]).to.equal('The Password field is required.');
      })
  })

})