const files = require.context('./fireControlModule', false, /\.js$/)
const modules = {
  namespaced: true,
  state: {},
  mutations: {},
  actions: {}
}

files.keys().forEach(key => {
  const module = files(key).default
  for (const key in module) {
    Object.assign(modules[key], module[key])
  }
})

export default {
  namespaced: true,
  ...modules
}
