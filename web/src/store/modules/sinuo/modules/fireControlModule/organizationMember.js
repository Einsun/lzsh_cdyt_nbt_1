
import requestApi from '@/request/requestApi'
export default {
  state: {
    organizationMemberList: [],
  },
  mutations: {
    setOrganizationMemberList (state, list) {
      state.organizationMemberList = list
    },
  },
  actions: {
    getOrganizationMemberList ({ commit, state }, params = {}) {
      return requestApi.fireManage.organizationMember({
        method: 'GET',
        params
      }).then(res => {
        commit('setOrganizationMemberList', res.data)
        return res
      })
    },
    deleteOrganizationMember ({ commit, state }, data = {}) {
      return requestApi.fireManage.organizationMember({
        method: 'DELETE',
        data
      }).then(res => {
      })
    },
    updateOrganizationMember ({ commit, state }, data = {}) {
      return requestApi.fireManage.organizationMember({
        method: 'PUT',
        data
      }).then(res => {
      })
    },
    createOrganizationMember ({ commit, state }, data = {}) {
      return requestApi.fireManage.organizationMember({
        method: 'POST',
        data
      }).then(res => {
      })
    },
  }
}
