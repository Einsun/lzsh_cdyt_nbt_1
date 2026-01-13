export default {
  namespaced: true,
  state: {
    homePageIsFullScreen: false
  },
  mutations: {
    toggleHomePageScreenStatus (state) {
      state.homePageIsFullScreen = !state.homePageIsFullScreen
    }
  }
}
