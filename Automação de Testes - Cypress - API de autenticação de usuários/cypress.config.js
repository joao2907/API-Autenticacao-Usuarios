const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    env: {
      apiBaseUrl: 'https://localhost:44362/api/Auth'
    },
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
});
