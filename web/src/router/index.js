import Vue from 'vue'
import VueRouter from 'vue-router'

// 閺夆晜绋戠€规娊寮堕敓锟�
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'

import store from '@/store/index'

import util from '@/libs/util.js'

// 閻犱警鍨抽弫閬嶅极閻楀牆绁�
import routes from './routes'

Vue.use(VueRouter)

// 閻庣數鍘ч崵顓犳崉椤栨粍鏆� 闁革讣鎷� main.js 闂佹彃濂旀繛鍥偨閿燂拷
const router = new VueRouter({
  routes
})

/**
 * 閻犱警鍨抽弫閬嶅箯閿旇棄鐒�
 * 闁哄鍟村鐑橆殽瀹€鍐
 */
router.beforeEach((to, from, next) => {
  // 閺夆晜绋戠€规娊寮堕敓锟�
  NProgress.start()
  // 闁稿繑濞婂Λ鎾箹濠婂懎鍋嶉梻鍫涘灪濠拷
  store.commit('d2admin/search/set', false)
  // 濡ょ姴鐭侀惁澶庛亹閹惧啿顤呴悹渚灣閺侀亶骞嶉埀顒勫嫉婢跺本鐣遍柛鏍х秺閸樸倖绋夐鐔感﹂柛姘剧畵濞撳墎鎲版担瑙勭畳闁谎嗩嚙缂嶅秵顨ュ畝鍐闁活煉鎷�
  if (to.matched.some(r => r.meta.auth)) {
    // 閺夆晜鐟╅崳鐑藉汲閸屾稒顦ч悘蹇撴懚ookie闂佹彃鏈Σ鎼佸触閿曗偓閻°劑寮垫繅绱€ken濞达絾绮堢拹鐔割殽瀹€鍐闁哄嫷鍨伴幆渚€鎯傜拠鑼Э闁汇劌瀚ḿ顖涚閿燂拷
    // 閻犲洭鏀遍悧鎾箲椤斿灝娈伴棅顒夊亗缁楃喖宕濋敓鐘充粯閻熸洑妞掗幈銊╁绩閿燂拷
    const token = util.cookies.get('token')
    if (token && token !== 'undefined') {
      next()
    } else {
      // 婵炲备鍓濆﹢渚€鎯傜拠鑼Э闁汇劌瀚鍌炲磹濞嗘帞鍎查弶鐑嗗墮閸╁矂鎯傜拠鑼Э闁伙絽鐭傚ḿ锟�
      // 闁圭厧鎼悽顐ｇ▔婵犲嫭顏㈤梻鍕閸ㄦ岸宕濋悢鍓侇吅闁告艾閰ｅ〒鍓佹啺娴ｇ晫鍎查弶鐑嗗墰濞堟垶銇勯悽鍛婃〃閻庣懓鏈弳锝囨崉椤栨氨绐�
      next({
        name: 'login',
        query: {
          redirect: to.fullPath
        }
      })
      // // https://github.com/d2-projects/d2-admin/issues/138
      NProgress.done()
    }
  } else {
    // 濞戞挸绉瑰〒鍓佹啺娴ｄ粙鐓╁ù鐘哄Г閻楀孩顨ラ敓锟� 闁烩晛鐡ㄧ敮鎾焻濮樺磭绠�
    next()
  }
})

router.afterEach(to => {
  // 閺夆晜绋戠€规娊寮堕敓锟�
  NProgress.done()
  // 濠㈣埖宀搁妴澶愬箳瑜嶉崺锟� 闁瑰灚鎸哥槐鎴﹀棘閹殿喗鐣卞銈囨暬濞硷拷
  store.dispatch('d2admin/page/open', to)
  // 闁哄洤鐡ㄩ弫濂稿冀閸ヮ剦鏆�
  util.title(to.meta.title)
})

export default router
