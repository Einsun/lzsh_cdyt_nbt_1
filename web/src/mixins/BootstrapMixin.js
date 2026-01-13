import Vue from 'vue'
import md5 from 'js-md5';
import { Base64 } from 'js-base64';
import { setCookie, getCookie, delCookie } from '@/assets/js/cookie'
import _ from 'lodash'

Vue.prototype.$cookieStore = { setCookie, getCookie, delCookie }
Vue.prototype.$MD5 = md5
Vue.prototype.$Base64 = Base64
Vue.prototype._ = _

export default {
}
