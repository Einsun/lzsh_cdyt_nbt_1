import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    safetyIndexList: [],
    dailyScoreHistory: [],
  },
  mutations: {
    setSafetyIndexList (state, list) {
      state.safetyIndexList = list
    },
    setDailyScoreHistory (state, list) {
      state.dailyScoreHistory = list
    },
  },
  actions: {
    getSafetyIndexList ({ commit, state }, params = {}) {
      return requestApi.fireManage.safetyindex({
        method: 'GET',
        params
      }).then(res => {
        commit('setSafetyIndexList', res.data)
        return res
      })
    },
    getDailyScoreHistory ({ commit, state }, params = {}) {
      return requestApi.fireManage.dailyScoreHistory({
        method: 'GET',
        params
      }).then(res => {
        commit('setDailyScoreHistory', res)
        return res
      })
    },
  }
}
