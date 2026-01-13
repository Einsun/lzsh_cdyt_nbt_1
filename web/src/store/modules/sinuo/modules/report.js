import requestApi from '@/request/requestApi'
import dayjs from 'dayjs'
export default {
  namespaced: true,
  state: {
    hasReportDateList: []
  },
  mutations: {
    setHasReportDateList (state, list) {
      state.hasReportDateList = list
    }
  },
  actions: {
    updateHasReportDateList ({ commit, state }, unitID) {
      const params = {
        page: 1,
        perPage: 99999,
        unitID
      }
      // 必须保证获取报告月的上月也有数据
      requestApi.report.getMonthReport({
        params
      }).then(res => {
        const list = [...res.data]
        const dateList = list.map(item => {
          return `${item.year}-${item.month}`
        })
        commit('setHasReportDateList', dateList.slice(0))
      })
    }
  }
}
