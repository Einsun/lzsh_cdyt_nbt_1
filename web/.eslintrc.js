module.exports = {
  root: true,
  env: {
    node: true
  },
  globals: {
    axios: true,
    "AMap": 'AMap',
  },
  'extends': [
    'plugin:vue/essential',
    '@vue/standard'
  ],
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    'comma-dangle': 'off',
    'semi': 'off',
    'no-unused-vars': 'off',
    'camelcase': 'off',
    'quotes': 'off'
  },
  parserOptions: {
    parser: 'babel-eslint'
  }
}
