import requestApi from '@/request/requestApi'
export default {
  namespaced: true,
  state: {
    unitID: '',
    userInfo: null,
    companyList: [],
    weather: {},
    planDialogStatus: false
  },
  mutations: {
    setUnitID (state, ID) {
      state.unitID = ID
    },
    setUser (state, user) {
      state.userInfo = user
    },
    setCompanyList (state, list) {
      state.companyList = list
    },
    setWeather (state, list) {
      state.weather = list
    },
    setPlanDialogStatus (state, val) {
      state.planDialogStatus = val
    }
  },
  actions: {
    getCompanyList ({ commit, state }) {
      return requestApi.getCompanyList().then((res) => {
        commit('setCompanyList', res)
      }).catch()
    },
    // 获取天气信息
    getWeatherInfo ({ commit, state }, params = {}) {
      return requestApi.getWeatherInfo({ params }).then(res => {
        commit('setWeather', res)
        return res
      })
    }
  },
  getters: {
    companyListOptions: state => {
      return state.companyList.map(item => {
        return {
          label: item.unitName,
          value: item.unitID
        }
      })
    },
    companyName: (state) => {
      return (unitID) => {
        const companyInfo = state.companyList.find(item => {
          return item.unitID === unitID
        })
        const companyName = companyInfo ? companyInfo.unitName : ''
        return companyName
      }
    }
  }
}
