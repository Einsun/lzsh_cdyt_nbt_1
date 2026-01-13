import requestApi from '@/request/requestApi'
import productLine from "@/request/modules/productLine";
export default {
  namespaced: true,
  state: {
    productLineList: [],
  },
  mutations: {
    setProductLineList (state, list) {
      state.productLineList = list
    },

  },
  actions: {
    // device
    getProductLineList ({ commit, state }, params = {}) {
      return productLine.line({
        method: 'GET',
        params
      }).then(res => {
        commit('setProductLineList', res.data)
      })
    },
    deleteProductLine ({ commit, state }, data = {}) {
      return requestApi.productLine.line({
        method: 'DELETE',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    updateProductLine ({ commit, state }, data = {}) {
      return requestApi.productLine.line({
        method: 'PUT',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
    createProductLine ({ commit, state }, data = {}) {
      return requestApi.productLine.line({
        method: 'POST',
        data
      }).then(res => {
        // commit('deviceList', res)
      })
    },
  }
}
