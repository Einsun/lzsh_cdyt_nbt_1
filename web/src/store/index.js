import Vue from 'vue'
import Vuex from 'vuex'

import d2admin from './modules/d2admin'
import sinuo from './modules/sinuo'

import createPersistedState from 'vuex-persistedstate'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    d2admin,
    sinuo
  },
  plugins: [createPersistedState()]
})
