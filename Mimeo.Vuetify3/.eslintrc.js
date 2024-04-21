module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: [
    "eslint:recommended",
    "plugin:vue/vue3-essential",
    "@vue/eslint-config-typescript",
    "prettier",
    "@vue/eslint-config-prettier",
  ],
  rules: {
    "vetur.validation.template": false,
    "vue/multi-word-component-names": "off",
  },
};
