import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    controlRoomList: [],
  },
  mutations: {
    setControlRoomList (state, list) {
      state.controlRoomList = list
    },

  },
  actions: {
    // device
    getControlRoomList ({ commit, state }, params = {}) {
      return requestApi.fireManage.controlRoom({
        method: 'GET',
        params
      }).then(res => {
        commit('setControlRoomList', res.data)
        return res
      })
    },
    deleteControlRoom ({ commit, state }, data = {}) {
      return requestApi.fireManage.controlRoom({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    updateControlRoom ({ commit, state }, data = {}) {
      return requestApi.fireManage.controlRoom({
        method: 'PUT',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    createControlRoom ({ commit, state }, data = {}) {
      return requestApi.fireManage.controlRoom({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
  }
}
