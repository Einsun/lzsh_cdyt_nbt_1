import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    fireAlarm: {
      list: [],
      summaryInfo: {
        totalCount: 0,
        normalCount: 0,
        alarmCount: 0,
        faultCount: 0
      }
    },
    electronicFire: {
      list: [],
      summaryInfo: []
    },
    waterGape: {
      list: [],
      summaryInfo: {
        totalCount: 0,
        normalCount: 0,
        alarmCount: 0,
        faultCount: 0
      }
    },
    waterSpray: {
      list: [],
      summaryInfo: {
        totalCount: 0,
        normalCount: 0,
        alarmCount: 0,
        faultCount: 0
      }
    },
    waterLevel: {
      list: [],
      summaryInfo: {
        totalCount: 0,
        normalCount: 0,
        alarmCount: 0,
        faultCount: 0
      }
    },
    waterPump: {
      list: [],
      summaryInfo: {
        totalCount: 0,
        normalCount: 0,
        alarmCount: 0,
        faultCount: 0
      }
    },
    smokeDetector: {
      list: [],
      summaryInfo: {
        totalCount: 0,
        normalCount: 0,
        alarmCount: 0,
        faultCount: 0
      }
    },
    videoList: {
      list: [],
    },
    deviceOverview: {
      list: [],
    },
    alarmOverview: {
      list: [],
    },
  },
  mutations: {
    setDeviceOverview (state, data) {
      state.deviceOverview.list = data
    },
    setAlarmOverview (state, data) {
      state.alarmOverview.list = data
    },
    // 火灾报警
    setFireAlarmList (state, data) {
      state.fireAlarm.list = data
    },
    setFireAlarmSummaryInfo (state, data) {
      let totalCount = data.length
      let normalCount = 0
      let alarmCount = 0
      let faultCount = 0
      data.forEach(item => {
        switch (item.message) {
          case '正常': {
            normalCount++
            break
          }
          case '故障': {
            faultCount++
            break
          }
          case '报警': {
            alarmCount++
            break
          }
        }
      })
      state.fireAlarm.summaryInfo = {
        totalCount,
        normalCount,
        alarmCount,
        faultCount
      }
    },
    // 电气
    setElectronicFireList (state, data) {
      state.electronicFire.list = data
    },
    setElectronicFireSummaryInfo (state, data) {
      // 格式转换
      const totalCount = data.total_count
      const offlineCount = data.offline_count
      const onlineCount = totalCount - offlineCount

      const alarmCount = data.alarm_count
      const normalCount = totalCount - alarmCount
      const todayAlarmCount = data.today_alarm_count
      const todayFaultCount = data.today_fault_count
      const todayNormalCount = totalCount - todayAlarmCount - todayFaultCount

      const chartsData = [
        {
          title: '在线率',
          chartData: {
            columns: ['label', 'count'],
            rows: [
              {
                label: '在线',
                count: onlineCount
              },
              {
                label: '离线',
                count: offlineCount
              },
            ]
          },
          count: totalCount,
          countDesc: '设备总数',
          describeInfo: [
            {
              label: '在线设备数',
              count: onlineCount,

            },
            {
              label: '离线设备数',
              count: offlineCount
            }
          ]
        }, {
          title: '正常率',
          chartData: {
            columns: ['label', 'count'],
            rows: [
              {
                label: '正常',
                count: normalCount
              },
              {
                label: '报警',
                count: alarmCount
              },
            ]
          },
          count: totalCount,
          countDesc: '设备总数',
          describeInfo: [
            {
              label: '设备数',
              count: normalCount,

            },
            {
              label: '报警数',
              count: alarmCount
            }
          ]
        }, {
          title: '今日统计',
          chartData: {
            columns: ['label', 'count'],
            rows: [
              {
                label: '报警',
                count: todayAlarmCount
              },
              {
                label: '故障',
                count: todayFaultCount
              },
              {
                label: '正常',
                count: todayNormalCount
              },
            ]
          },
          count: normalCount,
          countDesc: '设备总数',
          describeInfo: [
            {
              label: '报警记录',
              count: todayAlarmCount,

            },
            {
              label: '故障记录',
              count: todayFaultCount
            }
          ]
        },
      ]

      state.electronicFire.summaryInfo = chartsData
    },
    //  水压表
    setWaterGapeList (state, data) {
      state.waterGape.list = data
    },
    setWaterGapeSummaryInfo (state, data) {
      let totalCount = data.length
      let normalCount = 0
      let alarmCount = 0
      let faultCount = 0
      data.forEach(item => {
        switch (item.status) {
          case '正常': {
            normalCount++
            break
          }
          case '过低': {
            alarmCount++
            break
          }
          case '过高': {
            alarmCount++
            break
          }
        }
      })
      state.waterGape.summaryInfo = {
        totalCount,
        normalCount,
        alarmCount,
        faultCount
      }
    }, //  水压表
    setWaterSprayList (state, data) {
      state.waterSpray.list = data
    },
    setWaterSpraySummaryInfo (state, data) {
      let totalCount = data.length
      let normalCount = 0
      let alarmCount = 0
      let faultCount = 0
      data.forEach(item => {
        switch (item.status) {
          case '正常': {
            normalCount++
            break
          }
          case '过低': {
            alarmCount++
            break
          }
          case '过高': {
            alarmCount++
            break
          }
        }
      })
      state.waterSpray.summaryInfo = {
        totalCount,
        normalCount,
        alarmCount,
        faultCount
      }
    }, //  水压表
    setWaterLevelList (state, data) {
      state.waterLevel.list = data
    },
    setWaterLevelSummaryInfo (state, data) {
      let totalCount = data.length
      let normalCount = 0
      let alarmCount = 0
      let faultCount = 0
      data.forEach(item => {
        switch (item.status) {
          case '正常': {
            normalCount++
            break
          }
          case '过低': {
            alarmCount++
            break
          }
          case '过高': {
            alarmCount++
            break
          }
        }
      })
      state.waterLevel.summaryInfo = {
        totalCount,
        normalCount,
        alarmCount,
        faultCount
      }
    },
    // 水泵
    setWaterPumpList (state, data) {
      state.waterPump.list = data
    },
    setWaterPumpSummaryInfo (state, data) {
      let totalCount = data.length
      let normalCount = 0
      let alarmCount = 0
      let faultCount = 0
      data.forEach(item => {
        switch (item.message) {
          case '正常': {
            normalCount++
            break
          }
          case '故障': {
            faultCount++
            break
          }
          case '报警': {
            alarmCount++
            break
          }
        }
      })
      state.waterPump.summaryInfo = {
        totalCount,
        normalCount,
        alarmCount,
        faultCount
      }
    },
    // 烟感
    setSmokeDetectorList (state, data) {
      state.smokeDetector.list = data
    },
    setSmokeDetectorSummaryInfo (state, data) {
      let totalCount = data.length
      let normalCount = 0
      let alarmCount = 0
      let faultCount = 0
      data.forEach(item => {
        switch (item.message) {
          case '正常': {
            normalCount++
            break
          }
          case '故障': {
            faultCount++
            break
          }
          case '报警': {
            alarmCount++
            break
          }
        }
      })
      state.smokeDetector.summaryInfo = {
        totalCount,
        normalCount,
        alarmCount,
        faultCount
      }
    },
    // 烟感
    setVideoList (state, data) {
      state.videoList.list = data
    },
  },
  actions: {
    // 火灾报警
    getFireAlarmList ({ commit, state }, params = {}) {
      return requestApi.device.getFireAlarmList({ params }).then(res => {
        //  根据数据格式进行commit
        const allList = [...state.fireAlarm.list, ...res.data]
        commit('setFireAlarmSummaryInfo', allList)
        commit('setFireAlarmList', allList)
        return res
      })
    },
    // 电气
    getElectronicFireSummary ({ commit }, params = {}) {
      return requestApi.device.getElectronicFireSummary({ params }).then(res => {
        commit('setElectronicFireSummaryInfo', res)
      })
    },
    getElectronicFireInfo ({ commit, state }, params = {}) {
      return requestApi.device.getElectronicFireList({ params }).then(res => {
        //  根据数据格式进行commit
        commit('setElectronicFireList', [...state.electronicFire.list, ...res])
        return res
      })
    },
    //  水压表
    getWaterGapeList ({ commit, state }, params = {}) {
      return requestApi.device.getWaterGapeList({ params }).then(res => {
        const allList = [...state.waterGape.list, ...res]
        commit('setWaterGapeList', allList)
        commit('setWaterGapeSummaryInfo', allList)
        return res
      })
    },
    //  水压喷淋表
    getWaterSprayList ({ commit, state }, params = {}) {
      return requestApi.device.getWaterSprayList({ params }).then(res => {
        const allList = [...state.waterSpray.list, ...res]
        commit('setWaterSprayList', allList)
        commit('setWaterSpraySummaryInfo', allList)
        return res
      })
    },
    //  水位表
    getWaterLevelList ({ commit, state }, params = {}) {
      return requestApi.device.getWaterLevelList({ params }).then(res => {
        const allList = [...state.waterLevel.list, ...res]
        commit('setWaterLevelList', allList)
        commit('setWaterLevelSummaryInfo', allList)
        return res
      })
    },
    //  水泵表
    getWaterPumpList ({ commit, state }, params = {}) {
      return requestApi.device.getWaterPumpList({ params }).then(res => {
        // const allList = [...state.waterPump.list, ...res]
        // commit('setWaterPumpList', allList)
        // commit('setWaterPumpSummaryInfo', allList)
        commit('setWaterPumpList', res)
        commit('setWaterPumpSummaryInfo', res)
        return res
      })
    },
    // 烟感
    getSmokeDetectorList ({ commit, state }, params = {}) {
      return requestApi.device.getSmokeDetectorList({ params }).then(res => {
        // const allList = [...state.smokeDetector.data, ...res]
        // commit('setSmokeDetectorSummaryInfo', allList)
        // commit('setSmokeDetectorList', allList)
        commit('setSmokeDetectorList', res.data)
        return res
      })
    },
    // 视频
    getVideoDeviceList ({ commit, state }, params = {}) {
      return requestApi.device.getVideoDeviceList({ params }).then(res => {
        const allList = [...state.videoList.list, ...res]
        commit('setVideoList', allList)
        return res
      })
    },
    // 总览
    getDeviceOverview ({ commit, state }, params = {}) {
      return requestApi.device.getDeviceOverview({ params }).then(res => {
        commit('setDeviceOverview', res.data)
        return res
      })
    },
    // 数据分析总览
    getAlarmOverview ({ commit, state }, params = {}) {
      return requestApi.device.getAlarmOverview({ params }).then(res => {
        commit('setAlarmOverview', res.data)
        return res
      })
    },
  }
}
