import requestApi from '@/request/requestApi'
export default {
  state: {
    rulesList: [],
  },
  mutations: {
    setRulesList (state, list) {
      state.rulesList = list
    },
  },
  actions: {
    getRulesList ({ commit, state }, params = {}) {
      return requestApi.fireManage.rules({
        method: 'GET',
        params
      }).then(res => {
        commit('setRulesList', res.data)
        return res
      })
    },
    deleteRules ({ commit, state }, data = {}) {
      return requestApi.fireManage.rules({
        method: 'DELETE',
        data
      }).then(res => {
      })
    },
    updateRules ({ commit, state }, data = {}) {
      return requestApi.fireManage.rules({
        method: 'PUT',
        data
      }).then(res => {
      })
    },
    createRules ({ commit, state }, data = {}) {
      return requestApi.fireManage.rules({
        method: 'POST',
        data
      }).then(res => {
      })
    },
  }
}
