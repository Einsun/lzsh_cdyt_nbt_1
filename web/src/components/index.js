import Vue from 'vue'

import d2Container from './d2-container'
import form from './sn-form/index.vue'

// 注意 有些组件使用异步加载会有影响
Vue.component('d2-container', d2Container)
Vue.component('d2-icon', () => import('./d2-icon'))
Vue.component('d2-count-up', () => import('./d2-count-up'))
Vue.component('d2-icon-svg', () => import('./d2-icon-svg/index.vue'))
Vue.component('sn-dialog', () => import('./sn-dialog/index.vue'))
Vue.component('sn-line', () => import('./sn-line/index.vue'))
Vue.component('sn-icon', () => import('./sn-icon'))
Vue.component('sn-icon-scaleout', () => import('./sn-icon-scaleout'))
// 将form直接注册为全局组件,因为异步加载时不能及时渲染导致loading位置不正确，且form是常用组件，不需要异步加载
Vue.component('sn-form', form)
