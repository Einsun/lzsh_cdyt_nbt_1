
import requestApi from '@/request/requestApi'
export default {
  state: {
    dangerRectificationList: [],
  },
  mutations: {
    setDangerRectificationList (state, list) {
      state.dangerRectificationList = list
    },
  },
  actions: {
    getDangerRectificationList ({ commit, state }, params = {}) {
      return requestApi.fireManage.dangerRectification({
        method: 'GET',
        params
      }).then(res => {
        commit('setDangerRectificationList', res.data)
        return res
      })
    },
    deleteDangerRectification ({ commit, state }, data = {}) {
      return requestApi.fireManage.dangerRectification({
        method: 'DELETE',
        data
      }).then(res => {
      })
    },
    updateDangerRectification ({ commit, state }, data = {}) {
      return requestApi.fireManage.dangerRectification({
        method: 'PUT',
        data
      }).then(res => {
      })
    },
    createDangerRectification ({ commit, state }, data = {}) {
      return requestApi.fireManage.dangerRectification({
        method: 'POST',
        data
      }).then(res => {
      })
    },
  }
}
