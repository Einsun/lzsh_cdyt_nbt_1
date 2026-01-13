import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    mapActiveUnit: {},
    shouldPlayVideoUnit: {},
    zoom: 15,
    industryAlarmStatistics: [],
    alarmAndFaultData: [],
    alarmDevice: [],
    hiddenDangerList: [],
    deviceCount: {},
    alarmActiveTabType: '',
    alarmList: [],
    fireAlarm: [],
    waterGage: [],
    waterLevel: [],
    electricAlarm: [],
    VUE_APP_UNOPENED_PROMPT: process.env.VUE_APP_UNOPENED_PROMPT ? process.env.VUE_APP_UNOPENED_PROMPT : '此版本为基础版'
  },
  mutations: {
    setMapActiveUnit (state, item) {
      state.mapActiveUnit = item
    },
    setShouldPlayVideoUnit (state, item) {
      state.shouldPlayVideoUnit = item
    },
    setZoom (state, zoom) {
      state.zoom = zoom
    },
    setIndustryAlarmStatistics (state, statistics) {
      state.industryAlarmStatistics = statistics
    },
    setAlarmAndFaultData (state, alarmAndFaultData) {
      state.alarmAndFaultData = alarmAndFaultData
    },
    setAlarmDevice (state, alarmDevice) {
      state.alarmDevice = alarmDevice
    },
    setHiddenDangerList (state, hiddenDangerList) {
      state.hiddenDangerList = hiddenDangerList
    },
    setDeviceCount (state, data) {
      state.deviceCount = data
    },
    setAlarmList (state, list) {
      state.alarmList = list
    },
    setFireAlarm (state, list) {
      state.fireAlarm = list
    },
    setWaterGage (state, list) {
      state.waterGage = list
    },
    setWaterLevel (state, list) {
      state.waterLevel = list
    },
    setElectricAlarm (state, list) {
      state.electricAlarm = list
    },
    setAlarmActiveTabType (state, type) {
      state.alarmActiveTabType = type
    }
  },
  actions: {
    getIndustryAlarmStatistics ({ commit, state }, params = {}) {
      return requestApi.index.getIndustryAlarmStatistics({ params }).then(res => {
        commit('setIndustryAlarmStatistics', res.data)
      })
    },
    getAlarmAndFaultData ({ commit, state }, params = {}) {
      return requestApi.index.getAlarmTrend({ params }).then(res => {
        commit('setAlarmAndFaultData', res)
      })
    },
    getAlarmDevice ({ commit, state }, params = {}) {
      return requestApi.index.getAlarmDevice({ params }).then(res => {
        commit('setAlarmDevice', res.data)
      })
    },
    getHiddenDangerList ({ commit, state }, params = {}) {
      return requestApi.index.getHiddenDangerList({ params }).then(res => {
        commit('setHiddenDangerList', res)
      })
    },
    getDeviceCount ({ commit, state }, params = {}) {
      return requestApi.index.getDeviceCount({ params }).then(res => {
        commit('setDeviceCount', res.data)
      })
    },
    getAlarmList ({ commit, state }, params) {
      params = {
        ...params,
        perPage: 10
      }
      return requestApi.alarm.getAlarmList({ params }).then(res => {
        commit('setAlarmList', res.data)
      });
    },
  }
}
