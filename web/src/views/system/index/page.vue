<template>
  <div class="page">
    <div class="left-wrapper" :class="{ 'is-full-screen': isFullScreen }">
      <deviceCount></deviceCount>
      <alarmDevice></alarmDevice>
    </div>
    <div class="center-wrapper" :class="{ 'is-full-screen': isFullScreen }">
      <deviceCard></deviceCard>
    </div>
    <div class="right-wrapper" :class="{ 'is-full-screen': isFullScreen }">
      <alarmLevel></alarmLevel>
      <alarmStatistics></alarmStatistics>
    </div>
  </div>
</template>

<script>
// import pageMap from './components/map/index'

// top
import dataSummary from "./components/top/summary";
import searchInput from "./components/top/search-input";

// left
import deviceCount from "./components/left/device-count";
import alarmLevel from "./components/left/alarm-level";
import fireControlIndex from "./components/left/fire-control-index";
import alarmTrend from "./components/left/alarm-trend";
import unitLegend from "./components/left/unit-legend";

import fireAlarm from "./components/left/fire-alarm";

// right
import alarmStatistics from "./components/right/alarm-statistics";
import alarmDevice from "./components/right/alarm-device";
import deviceCard from "./components/right/device-card";
import unitVideo from "./components/right/unit-video";

// footer
import dashboardFooter from "./components/footer";

export default {
  components: {
    deviceCount,
    alarmLevel,
    alarmStatistics,
    alarmDevice,
    deviceCard,
  },
  computed: {
    isFullScreen() {
      return this.$store.state.sinuo.setting.homePageIsFullScreen;
    },
  },
  data() {
    return {
      socket: null,
      messages: [],
    };
  },
  mounted() {
  },
  beforeDestroy() {
  },
  methods: {},
};
</script>

<style lang="scss" scoped>
.page-content {
  top: -139px !important;
}
.page {
  position: relative;
  top: -139px !important;
  .left-wrapper {
    width: 460px;
    z-index: 99;
    position: absolute;
    top: 113px;
    left: 20px;
    transform-style: preserve-3d;
    perspective: 2000px;
    & > * {
      margin-top: 10px;
    }
    transform: rotateY(0);
    transform-origin: -60px 0;
    transition: all 1.5s cubic-bezier(0.68, -0.55, 0.27, 1.55);
    &.is-full-screen {
      transform: rotateY(98deg);
    }
  }
  .right-wrapper {
    @extend .left-wrapper;
    width: 460px;
    right: 20px;
    left: auto;
    transform-origin: 495px 0;
    & > * {
      //transform: rotateY(-8deg);
      margin-top: 10px;
    }
    &.is-full-screen {
      transform: rotateY(-98deg);
    }
  }
  .center-wrapper {
    overflow-x: hidden;
    overflow-y: auto;
    width: auto;
    height: auto;
    margin-right: 0px;
    margin-bottom: 20px;
    position: fixed;
    left: 500px;
    bottom: 0;
    right: 500px;
    top: 105px;
    transform-origin: bottom;
    transition: all 1.5s cubic-bezier(0.68, -0.55, 0.27, 1.55);
    &.is-full-screen {
      transform: translate(0, 100%);
    }
    //滚动条
    &::-webkit-scrollbar {
      width: 3px;
      height: 3px;
    }

    &::-webkit-scrollbar-track {
      background: rgb(239, 239, 239);
      border-radius: 2px;
    }

    &::-webkit-scrollbar-thumb {
      background: #389cc7;
      border-radius: 10px;
    }

    &::-webkit-scrollbar-thumb:hover {
      background: #389cc7;
    }

    &::-webkit-scrollbar-corner {
      background: #389cc7;
    }
  }
}
</style>

<style>
.el-form--inline .el-form-item {
  display: inline-block !important;
}
</style>