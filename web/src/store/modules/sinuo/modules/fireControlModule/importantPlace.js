import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    importantPlaceList: [],
  },
  mutations: {
    setImportantPlaceList (state, list) {
      state.importantPlaceList = list
    },

  },
  actions: {
    // device
    getImportantPlaceList ({ commit, state }, params = {}) {
      return requestApi.fireManage.importantPlace({
        method: 'GET',
        params
      }).then(res => {
        commit('setImportantPlaceList', res.data)
        return res
      })
    },
    deleteImportantPlace ({ commit, state }, data = {}) {
      return requestApi.fireManage.importantPlace({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    updateImportantPlace ({ commit, state }, data = {}) {
      return requestApi.fireManage.importantPlace({
        method: 'PUT',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    createImportantPlace ({ commit, state }, data = {}) {
      return requestApi.fireManage.importantPlace({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
  }
}
