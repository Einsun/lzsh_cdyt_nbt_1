import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    maintenanceTaskList: [],
    maintenanceTaskHistory: []
  },
  mutations: {
    setMaintenanceTasks (state, list) {
      state.maintenanceTaskList = list
    },
    setMaintenanceTaskHistory (state, list) {
      state.maintenanceTaskHistory = list
    }
  },
  actions: {
    getMaintenanceTasks ({ commit, state }, params = {}) {
      return requestApi.maintenance.maintenanceTask({
        method: 'GET',
        params
      }).then(res => {
        commit('setMaintenanceTasks', res.data)
        return res
      })
    },
    getMaintenanceTasksHistory ({ commit, state }, params = {}) {
      return requestApi.maintenance.maintenanceHistoryTask({
        method: 'GET',
        params
      }).then(res => {
        commit('setMaintenanceTaskHistory', res.data)
        return res
      })
    },
  }
}
