// Vue
import Vue from 'vue'
import App from './App'
// 核心插件
import d2Admin from '@/plugin/d2admin'
// store
import store from '@/store/index'
// 多国语
import i18n from './i18n'

// 菜单和路由设置
import router from './router'
import menuHeader from '@/menu/header'
import menuAside from '@/menu/aside'
import { frameInRoutes } from '@/router/routes'
import axios from '@/plugin/axios'
import './assets/style/helper.scss'

// 接口集合
import requestApi from '@/request/requestApi'

// import Echo from 'laravel-echo'
// 核心插件
Vue.use(d2Admin)

Vue.prototype.$api = requestApi
Vue.prototype.$getUnitID = () => { return store.state.sinuo.user.unitID }

Vue.prototype.$axios = axios;

new Vue({
  router,
  store,
  i18n,
  render: h => h(App),
  created () {
    // 处理路由 得到每一级的路由设置
    this.$store.commit('d2admin/page/init', frameInRoutes)
    // 设置顶栏菜单
    const user = store.state.sinuo.user
    this.$store.commit('d2admin/menu/headerSet', menuHeader)
    // 设置侧边栏菜单 侧栏菜单持久化读或重新设
    // this.$store.commit('d2admin/menu/asideSet', menuAside)
    // 初始化菜单搜索功能
    this.$store.commit('d2admin/search/init', menuHeader)
  },
  mounted () {
    // 展示系统信息
    this.$store.commit('d2admin/releases/versionShow')
    // 用户登录后从数据库加载一系列的设置
    this.$store.dispatch('d2admin/account/load')
    // 获取并记录用户 UA
    this.$store.commit('d2admin/ua/get')
    // 初始化全屏监听
    this.$store.dispatch('d2admin/fullscreen/listen')
    // 初始化报警弹框状态
    this.$store.commit('sinuo/user/setPlanDialogStatus', false)

    function adjustInitialScale () {
      // debugger
      const screenWidth = window.innerWidth
      const scale = screenWidth / 1920
      const $app = document.querySelector('body')
      $app.style.zoom = scale
    }
    adjustInitialScale()
    window.onresize = () => {
      let timer = null
      timer = setTimeout(() => {
        clearTimeout(timer);
        adjustInitialScale()
      }, 200)
    }
  }
}).$mount('#app')
