<template>
  <div
    class="d2-theme-header-menu header"
    ref="page"
    :class="{ 'is-scrollable': isScroll }"
    flex="cross:center"
  >
    <div class="d2-theme-header-menu__content" ref="content" flex-box="1" flex>
      <div
        class="d2-theme-header-menu__scroll"
        ref="scroll"
        flex-box="0"
        :style="'transform: translateX(' + currentTranslateX + 'px);'"
      >
        <el-menu
          mode="horizontal"
          :default-active="active"
          @select="handleMenuSelect"
        >
          <template>
            <template v-for="menu in leftHeader">
              <d2-layout-header-aside-menu-item
                v-if="menu.children === undefined"
                :menu="menu"
                :key="menu.title"
              />
              <d2-layout-header-aside-menu-sub
                v-else
                :menu="menu"
                :key="menu.title"
              />
            </template>
            <template>
              <li>
                <img
                  src="/image/theme/sinuo/logo/shengshi-logo.png"
                  alt=""
                  class="header-logo"
                />
                <span class="header-title">智慧管理平台</span>
              </li>
            </template>
            <template v-for="menu in rightHeader">
              <d2-layout-header-aside-menu-item
                v-if="menu.children === undefined"
                :menu="menu"
                :key="menu.title"
              />
              <d2-layout-header-aside-menu-sub
                v-else
                :menu="menu"
                :key="menu.title"
              />
            </template>
<!--            <template>-->
<!--              <li>-->
<!--                <div class="icon-wrapper flex-center-cross flex-justify-main">-->
<!--                  <div class="screen-wrapper" @click="toggleAlarmSwitch">-->
<!--                    <sn-icon-->
<!--                      name="notification"-->
<!--                      path="header"-->
<!--                      :size="20"-->
<!--                      class="notification-icon"-->
<!--                      :class="{'close-status': hasAlarm}"-->
<!--                    ></sn-icon>-->
<!--                  </div>-->
<!--                  <div class="screen-wrapper" @click="toggleFullScreen">-->
<!--                    <sn-icon-->
<!--                      name="full-screen"-->
<!--                      path="header"-->
<!--                      :size="20"-->
<!--                    ></sn-icon>-->
<!--                  </div>-->
<!--                </div>-->
<!--              </li>-->
<!--            </template>-->
            <!-- <template>
              <li>
                <d2-header-user flex-box="0" />
              </li>
            </template> -->
            <template>
              <li>
                <el-row>
                  <el-col :span="24">
                    <div class="login-wrapper flex-center-cross flex-justify-main">
                      <div class="screen-wrapper">
<!--                        <d2-header-weather :name="weather.wea_img" :city="weather.city" :tem="weather.tem" />-->
                      </div>
                      <div class="screen-wrapper">
                        <el-dropdown placement="bottom">
                          <el-button class="login-button" icon="el-icon-switch-button" type="text"></el-button>
                          <el-dropdown-menu slot="dropdown" class="sign-out">
                            <el-dropdown-item class="text" @click.native="logOff">退出</el-dropdown-item>
                          </el-dropdown-menu>
                        </el-dropdown>
                      </div>
                    </div>
                  </el-col>
                </el-row>
                <el-row>
                  <el-col :span="24">
                    <div class="color-primary">{{ nowtime | updatetime }}</div>
                  </el-col>
                </el-row>
              </li>
            </template>
          </template>
        </el-menu>
      </div>
    </div>
    <div
      v-if="isScroll"
      class="d2-theme-header-menu__prev"
      flex-box="0"
      @click="scroll('left')"
      flex="main:center cross:center"
    >
      <i class="el-icon-arrow-left"></i>
    </div>
    <div
      v-if="isScroll"
      class="d2-theme-header-menu__next"
      flex-box="0"
      @click="scroll('right')"
      flex="cross:center"
    >
      <i class="el-icon-arrow-right"></i>
    </div>
  </div>
</template>

<script>
import { throttle } from 'lodash';
import { mapState, mapActions } from 'vuex';
import menuMixin from '../mixin/menu';
import d2LayoutMainMenuItem from '../components/menu-item/index.vue';
import d2LayoutMainMenuSub from '../components/menu-sub/index.vue';
import headerMapAside from '../../../../menu/headerMapAside';
// import d2HeaderUser from '../header-user';
// import d2HeaderWeather from '../header-weather'
import moment from 'moment'
import util from "@/libs/util";

export default {
  name: 'd2-layout-header-aside-menu-header',
  mixins: [menuMixin],
  components: {
    'd2-layout-header-aside-menu-item': d2LayoutMainMenuItem,
    'd2-layout-header-aside-menu-sub': d2LayoutMainMenuSub,
    // 'd2-header-user': d2HeaderUser
    // 'd2-header-weather': d2HeaderWeather
  },
  computed: {
    ...mapState('d2admin/menu', ['header']),
    leftHeader () {
      return this.header.slice(0, 3);
    },
    rightHeader () {
      return this.header.slice(3);
    },
    hasAlarm () {
      return !this.$store.state.sinuo.functionSwitch.hasAlarm
    },
    weather () {
      return this.$store.state.sinuo.user.weather
    }
  },
  data () {
    return {
      active: '',
      isScroll: false,
      scrollWidth: 0,
      contentWidth: 0,
      currentTranslateX: 0,
      throttledCheckScroll: null,
      timer: {},
      nowtime: new Date()
    };
  },
  filters: {
    updatetime: function (value) {
      return moment(value).format('YYYY-MM-DD HH:mm:ss');
    }
  },
  watch: {
    '$route.matched': {
      handler (val) {
        // 注意 此处必须保证路由的写法，header中的path
        const header = this.$store.state.d2admin.menu.header;
        if (val.length > 1) {
          const currentPrimaryPath = val[1].path.split('/')[1];
          const activePathInfo = header.find(item => {
            return item.path.split('/')[1] === currentPrimaryPath;
          });
          const activePath = activePathInfo && activePathInfo.path;

          if (activePath) {
            this.$nextTick().then(() => {
              this.active = activePath;
            })
          }
        }
      },
      immediate: true
    },
    $route: {
      handler (to, from) {
        var path = to.path;
        this.active = path;
        const target = headerMapAside.find(item => {
          var i = 0;
          var asidePath;
          for (i = 0; i < item.aside.length; i++) {
            asidePath = item.aside.length ? item.aside[i].path : ''
            if (asidePath === path) break;
          }
          if (item.aside[i]) {
            return item.aside[i].path === path
          } else {
            return false
          }
        });
        if (target) {
          if (target.hiddenAside) {
            this.$store.commit('d2admin/menu/asideStatusSet', true);
          } else {
            this.$store.commit('d2admin/menu/asideStatusSet', false);
            this.$store.commit('d2admin/menu/asideSet', target.aside);
          }
        } else {
          this.$store.commit('d2admin/menu/asideStatusSet', true);
        }
      },
      immediate: true
    },
  },
  methods: {
    ...mapActions('d2admin/account', [
      'logout'
    ]),
    logOff () {
      this.logout({
        confirm: true
      })
    },
    scroll (direction) {
      if (direction === 'left') {
        // 向右滚动
        this.currentTranslateX = 0;
      } else {
        // 向左滚动
        if (
          this.contentWidth * 2 - this.currentTranslateX <=
          this.scrollWidth
        ) {
          this.currentTranslateX -= this.contentWidth;
        } else {
          this.currentTranslateX = this.contentWidth - this.scrollWidth;
        }
      }
    },
    checkScroll () {
      let contentWidth = this.$refs.content.clientWidth;
      let scrollWidth = this.$refs.scroll.clientWidth;
      if (this.isScroll) {
        // 页面依旧允许滚动的情况，需要更新width
        if (this.contentWidth - this.scrollWidth === this.currentTranslateX) {
          // currentTranslateX 也需要相应变化【在右端到头的情况时】
          this.currentTranslateX = contentWidth - scrollWidth;
          // 快速的滑动依旧存在判断和计算时对应的contentWidth变成正数，所以需要限制一下
          if (this.currentTranslateX > 0) {
            this.currentTranslateX = 0;
          }
        }
        // 更新元素数据
        this.contentWidth = contentWidth;
        this.scrollWidth = scrollWidth;
        // 判断何时滚动消失: 当scroll > content
        if (contentWidth > scrollWidth) {
          this.isScroll = false;
        }
      }
      // 判断何时滚动出现: 当scroll < content
      if (!this.isScroll && contentWidth < scrollWidth) {
        this.isScroll = true;
        // 注意，当isScroll变为true，对应的元素盒子大小会发生变化
        this.$nextTick(() => {
          contentWidth = this.$refs.content.clientWidth;
          scrollWidth = this.$refs.scroll.clientWidth;
          this.contentWidth = contentWidth;
          this.scrollWidth = scrollWidth;
          this.currentTranslateX = 0;
        });
      }
    },
    toggleFullScreen () {
      this.$store.commit('sinuo/setting/toggleHomePageScreenStatus');
    },
    toggleAlarmSwitch () {
      this.$store.commit('sinuo/functionSwitch/toggleFunction', 'hasAlarm')
    },
    getWeatherInfo () {
      const params = {}
      // var value = this.$store.dispatch('sinuo/user/getWeatherInfo')
    }
  },
  mounted () {
    // 初始化判断
    // 默认判断父元素和子元素的大小，以确定初始情况是否显示滚动
    window.addEventListener('load', this.checkScroll);
    // 全局窗口变化监听，判断父元素和子元素的大小，从而控制isScroll的开关
    this.throttledCheckScroll = throttle(this.checkScroll, 300);
    window.addEventListener('resize', this.throttledCheckScroll);
    // this.getWeatherInfo();
    this.timer = setInterval(() => {
      this.nowtime = new Date();
    }, 1000)
  },
  beforeDestroy () {
    // 取消监听
    window.removeEventListener('resize', this.throttledCheckScroll);
    window.removeEventListener('load', this.checkScroll);
    if (this.timer) {
      clearInterval(this.timer)
    }
  }
};
</script>

<style lang="scss" scoped>
.header-logo {
  min-width: 51px;
  vertical-align: middle;
  //min-width: 419px;
}
.header-title {
  height: 30px;
  vertical-align: middle;
  font-size: 30px;
  line-height: 30px;
  margin-left: 10px;
  font-weight: 530;
  background: linear-gradient(
    270deg,
    rgba(0, 120, 215, 1) 0%,
    rgba(58, 171, 212, 1) 100%
  );
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}
li {
  box-shadow: 0 0 0 0 !important;
  &:hover {
    border: none !important;
  }
  &:focus{
    outline: none;
  }
}
.icon-wrapper {
  width: 119px;
  justify-content: space-around;
  height: 32px;
  box-shadow: 0 0 0 0 !important;
  &:hover {
    border: none !important;
  }
  .notification-icon {
    position: relative;
    top: 3px;
  }
  .close-status{
    &::after{
      position: absolute;
      content: "/";
      color: #2ec0f3;
      font-weight: 100;
      font-size: 35px;
      left: 5px;
      top: -7px;
      transform: rotateZ(10deg);
    }
  }
}

.login-wrapper {
  width: 100%;
  justify-content: space-around;
  height: 32px;
  box-shadow: 0 0 0 0 !important;
  &:hover {
    border: none !important;
  }
  .notification-icon {
    position: relative;
    top: 3px;
  }
  .close-status{
    &::after{
      position: absolute;
      content: "/";
      color: #2ec0f3;
      font-weight: 100;
      font-size: 35px;
      left: 5px;
      top: -7px;
      transform: rotateZ(10deg);
    }
  }
  .login-button {
    color: #2FC0F3 !important;
    font-size: 20px;
  }
}

.sign-out {
  padding: 0 !important;
  background-color: #102547;
  border-color: #2FC0F3;
  .text {
    color: #2FC0F3;
  }
}

</style>
