import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    deviceList: [],
    buildingList: [],
    planList: [],
    spotList: [],
  },
  mutations: {
    setDeviceList (state, list) {
      state.deviceList = list
    },
    setBuildingList (state, list) {
      state.buildingList = list
    },
    setPlanList (state, list) {
      state.planList = list
    },
    setSpotList (state, list) {
      state.spotList = list
    }
  },
  actions: {
    // device
    getDeviceList ({ commit, state }, params = {}) {
      return requestApi.patrol.device({
        method: 'GET',
        params
      }).then(res => {
        commit('setDeviceList', res.data)
      })
    },
    deleteDevice ({ commit, state }, data = {}) {
      return requestApi.patrol.device({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    updateDevice ({ commit, state }, data = {}) {
      return requestApi.patrol.device({
        method: 'PUT',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    createDevice ({ commit, state }, data = {}) {
      return requestApi.patrol.device({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    // plan
    getPlanList ({ commit, state }, params = {}) {
      return requestApi.patrol.plan({
        method: 'GET',
        params
      }).then(res => {
        commit('setPlanList', res.data)
      })
    },
    deletePlan ({ commit, state }, data = {}) {
      return requestApi.patrol.plan({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('planList', res)
      })
    },
    updatePlan ({ commit, state }, data = {}) {
      return requestApi.patrol.plan({
        method: 'PUT',
        data
      }).then(res => {
        // commit('planList', res)
      })
    },
    createPlan ({ commit, state }, data = {}) {
      return requestApi.patrol.plan({
        method: 'POST',
        data
      }).then(res => {
        // commit('planList', res)
      })
    },
    // spot
    getSpotList ({ commit, state }, params = {}) {
      return requestApi.patrol.spot({
        method: 'GET',
        params
      }).then(res => {
        commit('setSpotList', res.data)
      })
    },
    deleteSpot ({ commit, state }, data = {}) {
      return requestApi.patrol.spot({
        method: 'DELETE',
        data
      })
    },
    updateSpot ({ commit, state }, data = {}) {
      return requestApi.patrol.spot({
        method: 'PUT',
        data
      })
    },
    createSpot ({ commit, state }, data = {}) {
      return requestApi.patrol.spot({
        method: 'POST',
        data
      })
    },
    // building
    getBuildingList ({ commit, state }, params = {}) {
      return requestApi.patrol.building({
        method: 'GET',
        params
      }).then(res => {
        commit('setBuildingList', res.data)
      })
    },
    deleteBuilding ({ commit, state }, data = {}) {
      return requestApi.patrol.building({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('buildingList', res)
      })
    },
    updateBuilding ({ commit, state }, data = {}) {
      return requestApi.patrol.building({
        method: 'PUT',
        data
      }).then(res => {
        // commit('buildingList', res)
      })
    },
    createBuilding ({ commit, state }, data = {}) {
      return requestApi.patrol.building({
        method: 'POST',
        data
      }).then(res => {
        // commit('buildingList', res)
      })
    },
  }
}
