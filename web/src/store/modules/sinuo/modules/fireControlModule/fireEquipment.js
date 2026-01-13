
import requestApi from '@/request/requestApi'
export default {
  state: {
    fireEquipmentList: [],
  },
  mutations: {
    setFireEquipmentList (state, list) {
      state.fireEquipmentList = list
    },
  },
  actions: {
    getFireEquipmentList ({ commit, state }, params = {}) {
      return requestApi.fireManage.fireEquipment({
        method: 'GET',
        params
      }).then(res => {
        commit('setFireEquipmentList', res.data)
        return res
      })
    },
    deleteFireEquipment ({ commit, state }, data = {}) {
      return requestApi.fireManage.fireEquipment({
        method: 'DELETE',
        data
      }).then(res => {
      })
    },
    updateFireEquipment ({ commit, state }, data = {}) {
      return requestApi.fireManage.fireEquipment({
        method: 'PUT',
        data
      }).then(res => {
      })
    },
    createFireEquipment ({ commit, state }, data = {}) {
      return requestApi.fireManage.fireEquipment({
        method: 'POST',
        data
      }).then(res => {
      })
    },
  }
}
