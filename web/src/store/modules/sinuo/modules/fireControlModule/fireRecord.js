
import requestApi from '@/request/requestApi'
export default {
  state: {
    fireRecordList: [],
  },
  mutations: {
    setFireRecordList (state, list) {
      state.fireRecordList = list
    },
  },
  actions: {
    getFireRecordList ({ commit, state }, params = {}) {
      return requestApi.fireManage.fireRecord({
        method: 'GET',
        params
      }).then(res => {
        commit('setFireRecordList', res.data)
      })
    },
    deleteFireRecord ({ commit, state }, data = {}) {
      return requestApi.fireManage.fireRecord({
        method: 'DELETE',
        data
      }).then(res => {
      })
    },
    updateFireRecord ({ commit, state }, data = {}) {
      return requestApi.fireManage.fireRecord({
        method: 'PUT',
        data
      }).then(res => {
      })
    },
    createFireRecord ({ commit, state }, data = {}) {
      return requestApi.fireManage.fireRecord({
        method: 'POST',
        data
      }).then(res => {
      })
    },
  }
}
