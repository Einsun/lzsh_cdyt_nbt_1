import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    firePlanList: [],
  },
  mutations: {
    setFirePlanList (state, list) {
      state.firePlanList = list
    },
  },
  actions: {
    // device
    getFirePlanList ({ commit, state }, params = {}) {
      return requestApi.fireManage.firePlan({
        method: 'GET',
        params
      }).then(res => {
        commit('setFirePlanList', res.data)
        return res
      })
    },
    deleteFirePlan ({ commit, state }, data = {}) {
      return requestApi.fireManage.firePlan({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    updateFirePlan ({ commit, state }, data = {}) {
      return requestApi.fireManage.firePlan({
        method: 'PUT',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    createFirePlan ({ commit, state }, data = {}) {
      return requestApi.fireManage.firePlan({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    pushFireMessage ({ commit, state }, data = {}) {
      return requestApi.fireManage.pushFireMessage({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
  }
}
