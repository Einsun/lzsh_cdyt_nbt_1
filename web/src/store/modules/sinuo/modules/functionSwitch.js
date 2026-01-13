export default {
  namespaced: true,
  state: {
    hasAlarm: true
  },
  mutations: {
    openFunction (state, key) {
      state[key] = true
    },
    closeFunction (state, key) {
      state[key] = false
    },
    toggleFunction (state, key) {
      state[key] = !state[key]
    }
  }
}
