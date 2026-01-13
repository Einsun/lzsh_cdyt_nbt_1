import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    fireStationList: [],
  },
  mutations: {
    setFireStationList (state, list) {
      state.fireStationList = list
    },
  },
  actions: {
    // device
    getFireStationList ({ commit, state }, params = {}) {
      return requestApi.fireManage.miniFireStation({
        method: 'GET',
        params
      }).then(res => {
        commit('setFireStationList', res.data)
        return res
      })
    },
    deleteFireStation ({ commit, state }, data = {}) {
      return requestApi.fireManage.miniFireStation({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    updateFireStation ({ commit, state }, data = {}) {
      return requestApi.fireManage.miniFireStation({
        method: 'PUT',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    createFireStation ({ commit, state }, data = {}) {
      return requestApi.fireManage.miniFireStation({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
  }
}
